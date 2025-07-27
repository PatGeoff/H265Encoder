/*
 * copyright (c) 2011-21 Lynn Buchanan (Evans & Sutherland Computer Corporation)
 *
 * This file is NOT part of the standard release of FFmpeg.
 *
 * FFmpeg is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * FFmpeg is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with FFmpeg; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

/**
 * @file
 * video distortion filter
 *
 * This filter was designed for distorting images suitable for display in a domed theater.
 *
 * The input parameter specifies the path to a distortion data file.  All values are little-endian
 * 0000 'xfes'
 * 0004 output image width
 * 0008 output image height
 * 000c input image width
 * 0010 input image height
 * 0014 PixelSpec value for first row, first column
 * 0018 PixelSpec value for first row, second column
 * ... continues with PixelSpec data items for every column in each row of the output image
 *
 * PixelSpec data comprises two fixed-point integer values for every pixel in the output image
 * (the number of PixelSpec elements is output image width * output image height).
 * The column and row data specify the pixel location from the input image.  Each fixed-point
 * column or row value is 32 bits with the upper-16 being the integer part and the lower-16 the
 * fractional part.  The PixelSpec data is used in bilinear_interpolation() to extract a pixel.
 *
 * NOTE: all data will be byte-swapped if the word at 0000 is 'sefx' (big-endian)
 */


/**
 * @file
 * domed theater distortion filter
 */

#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <fileapi.h>

#include "avfilter.h"
#include "formats.h"
#include "drawutils.h"
#include "internal.h"
#include "video.h"

#include "libavutil/avstring.h"
#include "libavutil/eval.h"
#include "libavutil/internal.h"
#include "libavutil/opt.h"
#include "libavutil/intreadwrite.h"
#include "libavutil/opt.h"
#include "libavutil/parseutils.h"
#include "libavutil/pixdesc.h"
#include "libavutil/imgutils.h"
#include "libavutil/avassert.h"

typedef struct
{
    unsigned int column;
    unsigned int row;
} PixelSpec;

typedef struct {
    const AVClass *class;
    int outh, outw;
    int nb_planes;
    int use_bilinear;
	int use_bicubic;
    int write_timestamp;
    int timestamp_offset;
    int debug_level;
	double gamma;
    FFDrawContext draw;
    
	char *file_str;
	int w, h;               ///< output dimensions, a value of 0 will result in the input size
    int x, y;               ///< offsets of the input area with respect to the padded area
    
    uint8_t *data[8];       ///< picture data for each plane - temporary buffer while data is being distorted
    int linesize[8];        ///< number of bytes per line
	int pixelbytes[8];		///< bytes per pixel
	int hs[8];
	int vs[8];
	
    int hsub, vsub;         ///< chroma subsampling values
    
    int nRasterX;
    int nRasterY;
    int nDomeviewX;
    int nDomeviewY;
    PixelSpec *pPixelSpec;
	uint8_t *pMask;
	void *pGamma;
    uint8_t *(*interpolate)(uint8_t *dst_color, void *pGamma,
                                    const uint8_t *src, int src_linesize, int src_linestep,
                                    int x, int y, int max_x, int max_y);
} DistortContext;

typedef struct ThreadData {
    AVFrame *in, *out;
    int inw,  inh;
    int outw, outh;
    int hsub, vsub;
    int frame;
    int plane;
} ThreadData;

#define OFFSET(x) offsetof(DistortContext, x)
#define FLAGS AV_OPT_FLAG_FILTERING_PARAM|AV_OPT_FLAG_VIDEO_PARAM

static const AVOption distort_options[] = {
	{ "file",	   "distortion parameter file",	   OFFSET(file_str), AV_OPT_TYPE_STRING, {.str=""}, CHAR_MIN, CHAR_MAX, .flags=FLAGS },
    { "bilinear",  "use bilinear interpolation",   OFFSET(use_bilinear),  AV_OPT_TYPE_INT, {.i64=1}, 0, 1, .flags=FLAGS },
    { "bicubic",  "use bicubic interpolation",   OFFSET(use_bicubic),  AV_OPT_TYPE_INT, {.i64=0}, 0, 1, .flags=FLAGS },
    { "gamma",  "gamma correction",   OFFSET(gamma),  AV_OPT_TYPE_DOUBLE, {.dbl=1},  0, 10, .flags=FLAGS },
    { "timestamp",  "write timestamp",   OFFSET(write_timestamp),  AV_OPT_TYPE_INT, {.i64=0}, 0, 1, .flags=FLAGS },
    { "tsoffset",  "timestamp offset",   OFFSET(timestamp_offset),  AV_OPT_TYPE_INT, {.i64=0}, 0, 0xffffff, .flags=FLAGS },
    { "debug",  "display debug messages",   OFFSET(debug_level),  AV_OPT_TYPE_INT, {.i64=0}, 0, 3, .flags=FLAGS },
    { NULL }
};

AVFILTER_DEFINE_CLASS(distort);

static int swap(int nValue)
{
	union {
		uint8_t c[4];
		uint32_t i;
	} u;
	uint8_t c;
	
	u.i = nValue;
	c = u.c[0];
	u.c[0] = u.c[3];
	u.c[3] = c;
	c = u.c[1];
	u.c[1] = u.c[2];
	u.c[2] = c;
	
	return u.i;
}

static av_cold int init(AVFilterContext *ctx)
{
    DistortContext *distort = ctx->priv;
    if (distort->debug_level > 0)
        av_log(ctx, AV_LOG_INFO, "init() - debug=%d, bilinear=%s, timestamp=%s\n",
        	distort->debug_level, distort->use_bilinear ? "yes" : "no", distort->write_timestamp ? "yes" : "no");
    
    return 0;
}

static av_cold void uninit(AVFilterContext *ctx)
{
    DistortContext *distort = ctx->priv;
    
    if (distort->debug_level > 0) {
        av_log(ctx, AV_LOG_INFO, "uninit()\n");
	}
    
	if (distort->pPixelSpec) {
        av_free(distort->pPixelSpec);
		distort->pPixelSpec = NULL;
	}
	if (distort->pMask) {
		av_free(distort->pMask);
		distort->pMask = NULL;
	}
	if (distort->pGamma) {
		av_free(distort->pGamma);
		distort->pGamma = NULL;
	}
}

static int query_formats(AVFilterContext *ctx)
{
    static const enum AVPixelFormat pix_fmts[] = {
        AV_PIX_FMT_GBRP,   AV_PIX_FMT_GBRAP,
        AV_PIX_FMT_ARGB,   AV_PIX_FMT_RGBA,
        AV_PIX_FMT_ABGR,   AV_PIX_FMT_BGRA,
        AV_PIX_FMT_0RGB,   AV_PIX_FMT_RGB0,
        AV_PIX_FMT_0BGR,   AV_PIX_FMT_BGR0,
        AV_PIX_FMT_RGB24,  AV_PIX_FMT_BGR24,
        AV_PIX_FMT_GRAY8,
        AV_PIX_FMT_YUV410P,
        AV_PIX_FMT_YUV444P,  AV_PIX_FMT_YUVJ444P,
        AV_PIX_FMT_YUV420P,  AV_PIX_FMT_YUVJ420P,
        AV_PIX_FMT_YUVA444P, AV_PIX_FMT_YUVA420P,
        AV_PIX_FMT_YUV420P10LE, AV_PIX_FMT_YUVA420P10LE,
        AV_PIX_FMT_YUV444P10LE, AV_PIX_FMT_YUVA444P10LE,
        AV_PIX_FMT_YUV420P12LE,
        AV_PIX_FMT_YUV444P12LE,
        AV_PIX_FMT_YUV444P16LE, AV_PIX_FMT_YUVA444P16LE,
        AV_PIX_FMT_YUV420P16LE, AV_PIX_FMT_YUVA420P16LE,
        AV_PIX_FMT_YUV444P9LE, AV_PIX_FMT_YUVA444P9LE,
        AV_PIX_FMT_YUV420P9LE, AV_PIX_FMT_YUVA420P9LE,
        AV_PIX_FMT_NONE
    };

    AVFilterFormats *fmts_list = ff_make_format_list(pix_fmts);
    if (!fmts_list)
        return AVERROR(ENOMEM);
    return ff_set_common_formats(ctx, fmts_list);
}

/**
 * Interpolate the color in src at position x and y using bilinear
 * interpolation.
 */
static uint8_t *interpolate_bilinear8(uint8_t *dst_color, void *pGamma8,
                                     const uint8_t *src, int src_linesize, int src_linestep,
                                     int x, int y, int max_x, int max_y)
{
    uint32_t int_x = av_clip(x>>16, 0, max_x);
    uint32_t int_y = av_clip(y>>16, 0, max_y);
    uint32_t frac_x = x&0xFFFF;
    uint32_t frac_y = y&0xFFFF;
    uint32_t int_x1 = FFMIN(int_x+1, max_x);
    uint32_t int_y1 = FFMIN(int_y+1, max_y);
	uint8_t *p00 = (uint8_t*)&src[int_x * src_linestep + int_y * src_linesize];
	uint8_t *p01 = (uint8_t*)&src[int_x1 * src_linestep + int_y * src_linesize];
	uint8_t *p10 = (uint8_t*)&src[int_x * src_linestep + int_y1 * src_linesize];
	uint8_t *p11 = (uint8_t*)&src[int_x1 * src_linestep + int_y1 * src_linesize];
	uint8_t *dst = dst_color;
	uint8_t *pGamma = (uint8_t*)pGamma8;
	int i;

	for (i = 0; i < src_linestep; i++) {
		uint32_t s0 = *p00++ * (65536 - frac_x) + *p01++ * frac_x;
		uint32_t s1 = *p10++ * (65536 - frac_x) + *p11++ * frac_x;
		*dst++ = *(pGamma + (uint8_t)(((uint64_t)s0 * (65536 - frac_y) + (uint64_t)s1 * frac_y + 65535) >> 32));
    }
	
    return dst_color;
}

/**
 * Interpolate the color in src at position x and y using bilinear
 * interpolation.
 */
static uint8_t *interpolate_bilinear16(uint8_t *dst_color, void *pGamma16,
                                       const uint8_t *src, int src_linesize, int src_linestep,
                                       int x, int y, int max_x, int max_y)
{
    uint32_t int_x = av_clip(x>>16, 0, max_x);
    uint32_t int_y = av_clip(y>>16, 0, max_y);
    uint32_t frac_x = x&0xFFFF;
    uint32_t frac_y = y&0xFFFF;
    uint32_t int_x1 = FFMIN(int_x+1, max_x);
    uint32_t int_y1 = FFMIN(int_y+1, max_y);
	uint16_t *p00 = (uint16_t*)&src[int_x * src_linestep + int_y * src_linesize];
	uint16_t *p01 = (uint16_t*)&src[int_x1 * src_linestep + int_y * src_linesize];
	uint16_t *p10 = (uint16_t*)&src[int_x * src_linestep + int_y1 * src_linesize];
	uint16_t *p11 = (uint16_t*)&src[int_x1 * src_linestep + int_y1 * src_linesize];
	uint16_t *dst = (uint16_t*)dst_color;
	uint16_t *pGamma = (uint16_t*)pGamma16;
	int i;

	for (i = 0; i < src_linestep; i++) {
		uint64_t s0 = (uint64_t)*p00++ * (65536 - frac_x) + (uint64_t)*p01++ * frac_x;
		uint64_t s1 = (uint64_t)*p10++ * (65536 - frac_x) + (uint64_t)*p11++ * frac_x;
		*dst++ = *(pGamma + (uint16_t)((s0 * (65536 - frac_y) + s1 * frac_y + 65535) >> 32));
    }

    return dst_color;
}

static uint8_t *interpolate_bicubic8(uint8_t *dst_color, void *pGamma8,
                                     const uint8_t *src, int src_linesize, int src_linestep,
                                     int x, int y, int max_x, int max_y)
{
    int int_x = av_clip(x>>16, 0, max_x);
    int int_y = av_clip(y>>16, 0, max_y);

	uint8_t *pp = dst_color;
	uint8_t *pGamma = (uint8_t*)pGamma8;

	if (int_x >= 1 && int_x < (max_x - 2) && int_y >= 1 && int_y < (max_y - 2))
	{
		int i, j, k;
		float c[4];
		float factorX[4];
		float factorY[4];
	    uint8_t *p = (uint8_t*)&src[src_linestep * (int_x - 1) + src_linesize * (int_y - 1)];
		
		float frac_x = (x & 0xffff) / 65536.0f;
		float frac_y = (y & 0xffff) / 65536.0f;
		
		for (int ii = 0; ii < 4; ii++)
		{
			float a[4];
			float b[4];
	
			float yf = (float)(ii - 1) - frac_y;
			float xf = frac_x - (float)(ii - 1);
		
			for (int jj = 0; jj < 4; jj++)
			{
				float dxf = xf + 2.0f - (float)jj;
				float dyf = yf + 2.0f - (float)jj;
			
				a[jj] = dyf < 0.0f ? 0.0f : dyf * dyf * dyf;
				b[jj] = dxf < 0.0f ? 0.0f : dxf * dxf * dxf;
			}

			factorY[ii] = (a[0] - (4.0f * a[1]) + (6.0f * a[2]) - (4.0f * a[3])) / 6.0f;
			factorX[ii] = (b[0] - (4.0f * b[1]) + (6.0f * b[2]) - (4.0f * b[3])) / 6.0f;
		}
		
		for (k = 0; k < src_linestep; k++)
			c[k] = 0.0;

		for(i = 0; i < 4; i++)
		{
			unsigned char *p0 = p;

			for(j = 0; j < 4; j++)
			{
				float dFactor = factorY[i] * factorX[j];

				for (k = 0; k < src_linestep; k++)
					c[k] += *p++ * dFactor;
			}

			p = p0 + src_linesize;
		}

		for (k = 0; k < src_linestep; k++)
			*pp++ = *(pGamma + (uint8_t)c[k]);
	}
	else
	{
		int k;
		
	    uint8_t *p = (uint8_t*)&src[src_linestep * int_x + src_linesize * int_y];
		for (k = 0; k < src_linestep; k++)
			*pp++ = *(pGamma + *p++);
	}
	
	return dst_color;
}

static uint8_t *interpolate_bicubic16(uint8_t *dst_color, void *pGamma16,
                                     const uint8_t *src, int src_linesize, int src_linestep,
                                     int x, int y, int max_x, int max_y)
{
    int int_x = av_clip(x>>16, 0, max_x);
    int int_y = av_clip(y>>16, 0, max_y);

	uint16_t *pp = (uint16_t*)dst_color;
	uint16_t *pGamma = (uint16_t*)pGamma16;

	if (int_x >= 1 && int_x < (max_x - 2) && int_y >= 1 && int_y < (max_y - 2))
	{
		int i, j, k;
		float c[4];
		float factorX[4];
		float factorY[4];
	    uint16_t *p = (uint16_t*)&src[src_linestep * (int_x - 1) + src_linesize * (int_y - 1)];
		
		float frac_x = (x & 0xffff) / 65536.0f;
		float frac_y = (y & 0xffff) / 65536.0f;
		
		for (int ii = 0; ii < 4; ii++)
		{
			float a[4];
			float b[4];
	
			float yf = (float)(ii - 1) - frac_y;
			float xf = frac_x - (float)(ii - 1);
		
			for (int jj = 0; jj < 4; jj++)
			{
				float dxf = xf + 2.0f - (float)jj;
				float dyf = yf + 2.0f - (float)jj;
			
				a[jj] = dyf < 0.0f ? 0.0f : dyf * dyf * dyf;
				b[jj] = dxf < 0.0f ? 0.0f : dxf * dxf * dxf;
			}

			factorY[ii] = (a[0] - (4.0f * a[1]) + (6.0f * a[2]) - (4.0f * a[3])) / 6.0f;
			factorX[ii] = (b[0] - (4.0f * b[1]) + (6.0f * b[2]) - (4.0f * b[3])) / 6.0f;
		}
		
		for (k = 0; k < src_linestep; k++)
			c[k] = 0.0;

		for(i = 0; i < 4; i++)
		{
			uint16_t *p0 = p;

			for(j = 0; j < 4; j++)
			{
				float dFactor = factorY[i] * factorX[j];

				for (k = 0; k < src_linestep; k++)
					c[k] += *p++ * dFactor;
			}

			p = (uint16_t*)((uint8_t*)p0 + src_linesize);
		}

		for (k = 0; k < src_linestep; k++)
			*pp++ = *(pGamma + (uint16_t)c[k]);
	}
	else
	{
		int k;
		
	    uint16_t *p = (uint16_t*)&src[src_linestep * int_x + src_linesize * int_y];
		for (k = 0; k < src_linestep; k++)
			*pp++ = *(pGamma + *p++);
	}
	
	return dst_color;
}

static void compute_gamma(int nGammaSize, double dGamma, void **pGamma)
{
	// Compute gamma lookup table
	int i;

	if (nGammaSize == 256)
	{
		unsigned char *pnGamma;
		
		*pGamma = av_malloc(256);
		pnGamma = (unsigned char*)(*pGamma);

		for (i = 0; i < 256; i++)
			pnGamma[i] = (unsigned char)(255.0 * pow(i / 255.0, 1.0 / dGamma));
	}
	else
	{
		unsigned short *pnGamma;
		
		*pGamma = av_malloc(65536 * sizeof(unsigned short));
		pnGamma = (unsigned short*)(*pGamma);

		for (i = 0; i < 65536; i++)
			pnGamma[i] = (unsigned short)(65535.0 * pow(i / 65535.0, 1.0 / dGamma));
	}
}

static int config_props(AVFilterLink *outlink)
{
    AVFilterContext *ctx = outlink->src;
    DistortContext *distort = ctx->priv;
    AVFilterLink *inlink = ctx->inputs[0];
    const AVPixFmtDescriptor *pixdesc = av_pix_fmt_desc_get(inlink->format);
    int ret = -1;
	int len1, len2;
	int i;
    char *p;
    char *pBuffer;
	int16_t *tempPath;
    FILE *fp;
    
    if (p = strstr(distort->file_str, "//")) {
        *p++ = ':';
        *p = '\\';
    }
    
    while (p = strstr(distort->file_str, "/")) {
        *p = '\\';
    }
    
    if (distort->debug_level > 0) {
        av_log(ctx, AV_LOG_INFO, "config_props() - %s\n", distort->file_str);
	}
    
	pBuffer = NULL;
	distort->pPixelSpec = NULL;
	distort->pMask = NULL;
	
	if (access(distort->file_str, 0) != 0) {	// if the file is not found, try using the user temp file path
		len1 = GetTempPathW(0, NULL);			// determine length of temporary path
		if (len1)
		{
			len2 = strlen(distort->file_str);	// length of specified file name
			tempPath = malloc((len1 + len2 + 2) * sizeof(int16_t));			// allocate memory for file path
			if ((len1 = GetTempPathW(len1 + len2 + 2, tempPath))) {
				char tpath[256];
				for (i = 0; i < len2; i++) {
					*(tempPath + len1 + i) = (int16_t)distort->file_str[i];	// append file name to path
				}
				*(tempPath + len1 + i) = L'\0';
				for (i = 0; tempPath[i] >= L' '; i++)
					tpath[i] = (char)tempPath[i];
				tpath[i] = '\0';
				fp = _wfopen(tempPath, L"rb");	// open the distortion data file
				if (fp != NULL)
					av_log(ctx, AV_LOG_INFO, "Distortion data file \"%s\"\n", tpath);
				else
					av_log(ctx, AV_LOG_ERROR, "Distortion data file \"%s\" not found.\n", tpath);
				free(tempPath);
			}
		}
	}
	else
		fp = fopen(distort->file_str, "rb");	// open the file as specified

    if (fp  != NULL)
    {
        int nToken;
        union {
            char c[4];
            int l;
        } endianType[2];
        
        endianType[0].c[0] = 'x';
        endianType[0].c[1] = 'f';
        endianType[0].c[2] = 'e';
        endianType[0].c[3] = 's';
        endianType[1].c[0] = 's';
        endianType[1].c[1] = 'e';
        endianType[1].c[2] = 'f';
        endianType[1].c[3] = 'x';
        
        for (;;) {
			fread(&nToken, 1, sizeof(int), fp);
		
			if (nToken == endianType[0].l || nToken == endianType[1].l) {
				int i;
				int nBase;
				int nSizePixelSpec;
				int nSizeFrame;
				int nSizeFile;
			
				fread(&distort->nRasterX, 1, sizeof(int), fp);
				fread(&distort->nRasterY, 1, sizeof(int), fp);
				fread(&distort->nDomeviewX, 1, sizeof(int), fp);
				fread(&distort->nDomeviewY, 1, sizeof(int), fp);

				if (nToken == endianType[0].l) {	// swap values if not correct endian-ness
					distort->nRasterX = swap(distort->nRasterX);
					distort->nRasterY = swap(distort->nRasterY);
					distort->nDomeviewX = swap(distort->nDomeviewX);
					distort->nDomeviewY = swap(distort->nDomeviewY);
				}
							
				nBase = ftell(fp);
				fseek(fp, 0, SEEK_END);
				nSizeFile = ftell(fp) - nBase;
				fseek(fp, nBase, SEEK_SET);
			
				if ((distort->nRasterX & -32768) == 0 && (distort->nRasterY & -32768) == 0) {
					char interpMethod[24];
					
					nSizeFrame = distort->nRasterX * distort->nRasterY;
					nSizePixelSpec = nSizeFrame * sizeof(PixelSpec);
										
					if (distort->use_bicubic) {
						strcpy(interpMethod, "bicubic");
						distort->use_bilinear = 0;
					}
					else if (distort->use_bilinear)
						strcpy(interpMethod, "bilinear");
					else
						strcpy(interpMethod, "nearest neighbor");
			

					if (nSizePixelSpec > nSizeFile)	// exit if we don't have enough file data...
						break;
					
					if (pBuffer != NULL)
						pBuffer = av_realloc(pBuffer, nSizePixelSpec);	// reallocate buffer
					else
						pBuffer = av_malloc(nSizePixelSpec);				// allocate buffer
						
					fread(pBuffer, nSizePixelSpec, sizeof(char), fp);	// read distortion data
			
					if (distort->nDomeviewX != inlink->w || distort->nDomeviewY != inlink->h)
						continue;								// continue if this is not the resolution...
						
					if (nToken == endianType[0].l) {			// swap values if not correct endian-ness
						PixelSpec *ps;
						int i, j;
						
						ps = (PixelSpec*)pBuffer;
				
						for (i = 0; i < distort->nRasterY; i++)
						{
							for (j = 0; j < distort->nRasterX; j++)
							{
								ps->column = swap(ps->column);
								ps->row = swap(ps->row);
								ps++;
							}
						}
					}
			
					if ((nSizeFile - nSizePixelSpec) >= nSizeFrame)
					{
						if (distort->pMask != NULL)
							distort->pMask = av_realloc(pBuffer, nSizeFrame);	// reallocate buffer
						else
							distort->pMask = av_malloc(nSizeFrame);				// allocate buffer
						
						if (distort->pMask != NULL)
							fread(distort->pMask, nSizeFrame, sizeof(char), fp);	// read mask data
					}
					
					distort->pPixelSpec = (PixelSpec*)pBuffer;
					
					if (distort->pMask)
						av_log(ctx, AV_LOG_INFO, "Distortion data found (with mask): %dx%d -> %dx%d [%s]\n", distort->nDomeviewX, distort->nDomeviewY, distort->nRasterX, distort->nRasterY, interpMethod);
					else
						av_log(ctx, AV_LOG_INFO, "Distortion data found: %dx%d -> %dx%d [%s]\n", distort->nDomeviewX, distort->nDomeviewY, distort->nRasterX, distort->nRasterY, interpMethod);
			
					for (i = 0; i < 8; i++) {
						distort->data[i] = NULL;	// indicate that no image buffers have been allocated
						distort->linesize[i] = 0;
					}
			
					ret = 0;
					break;
				}
			}
        }
        
        fclose(fp);
    }
    
    if (ret != 0) {
    	if (pBuffer != NULL) {
    		av_free(pBuffer);
    		pBuffer = NULL;
    	}
    	
    	if (distort->file_str[0] == '\0') {
    		distort->pPixelSpec = NULL;
    		distort->use_bilinear = 0;
    		ret = 0;
			ff_draw_init(&distort->draw, inlink->format, 0);
	
			distort->hsub = pixdesc->log2_chroma_w;
			distort->vsub = pixdesc->log2_chroma_h;
	
			/* compute number of planes */
			distort->nb_planes = av_pix_fmt_count_planes(inlink->format);
			outlink->w = inlink->w;
			outlink->h = inlink->h;
			ret = 0;
			av_log(ctx, AV_LOG_ERROR, "No distortion data file.\n");
    	}
    	else
			av_log(ctx, AV_LOG_ERROR, "Distortion data file \"%s\" not valid.\n", distort->file_str);
	}
    else {
		ff_draw_init(&distort->draw, inlink->format, 0);
	
		distort->hsub = pixdesc->log2_chroma_w;
		distort->vsub = pixdesc->log2_chroma_h;
	
		/* compute number of planes */
		distort->nb_planes = av_pix_fmt_count_planes(inlink->format);
		outlink->w = distort->nRasterX;
		outlink->h = distort->nRasterY;
    }
	
	if (pixdesc->comp[0].depth == 8) {
		compute_gamma(256, distort->gamma, &distort->pGamma);
		
		if (distort->use_bilinear)
			distort->interpolate = interpolate_bilinear8;
		else if (distort->use_bicubic)
			distort->interpolate = interpolate_bicubic8;
		else
			distort->interpolate = NULL;
	}
    else {
		compute_gamma(65546, distort->gamma, &distort->pGamma);
		
        if (distort->use_bilinear)
			distort->interpolate = interpolate_bilinear16;
		else if (distort->use_bicubic)
			distort->interpolate = interpolate_bicubic16;
		else
			distort->interpolate = NULL;
	}

    
    return ret;
}

#define DFLT_COLOR 128

static int filter_slice(AVFilterContext *ctx, void *arg, int job, int nb_jobs)
{
    ThreadData *td = arg;
    AVFrame *in = td->in;
    AVFrame *out = td->out;
    DistortContext *distort = ctx->priv;
    const int outw = td->outw, outh = td->outh;
    const int inw = td->inw, inh = td->inh;
    const int plane = td->plane;
    const int start = (outh *  job   ) / nb_jobs;
    const int end   = (outh * (job+1)) / nb_jobs;
    int i, j, x, y;
    uint8_t inp_interp[4]; /* interpolated input value */
    unsigned char gray[4] = { DFLT_COLOR, DFLT_COLOR, DFLT_COLOR, DFLT_COLOR };
    union {
    	uint8_t c[4];
    	uint16_t s[2];
    	uint32_t l;
    } black;
	union {
		uint8_t c[4];
		uint16_t s[2];
		uint32_t l;
	} u;
	
	black.l = 0;
    
    if (distort->debug_level > 2)
        av_log(ctx, AV_LOG_INFO, "filter_slice(%d,%d), frame %d, plane %d, lines %d to %d, step %d (%d)\n",
        job, nb_jobs, td->frame, plane, start, end - 1, distort->draw.pixelstep[plane], 1 << td->hsub);
    for (j = start; j < end; j++) {
        PixelSpec *ps = distort->pPixelSpec;
		uint8_t *pm = distort->pMask;
		
        if (ps != NULL) {
        	ps += (j << td->hsub) * distort->nRasterX;
		}
		if (pm != NULL) {
        	pm += (j << td->hsub) * distort->nRasterX;
		}
        
        for (i = 0; i < outw; i++) {
            int x1, y1;
            uint8_t *pin, *pout;
            uint8_t bDefaultColor = 0;
           	int xVal = i << td->hsub;
           	int yVal = j << td->vsub;
           	int bTimeStamp;
            
            pout = out->data[plane] + j * out->linesize[plane] + i * distort->draw.pixelstep[plane];
            
            /* color the first 32 pixels of the first two lines based on the frame number */
            /* eight groups of 4 pixels - each RGB pixel group represents 3 bits of the frame number */
            bTimeStamp = distort->write_timestamp && yVal < 2 && xVal < 32;
            if (bTimeStamp) {
				int part = xVal >> 2;
				int value = ((td->frame + distort->timestamp_offset) >> (part * 3)) & 7;	// bits of the frame number
				
           		pin = inp_interp;
				*((uint32_t *)pin) = 0;
				
			 	if (distort->draw.pixelstep[plane] >= 3) {
			 		//value = 5;
			 		if (in->format == AV_PIX_FMT_BGR24) {
						inp_interp[0] = (value & 4) != 0 ? 254 : 0;
						inp_interp[1] = (value & 2) != 0 ? 254 : 0;
						inp_interp[2] = (value & 1) != 0 ? 254 : 0;
			 		}
			 		else {
						inp_interp[0] = (value & 1) != 0 ? 254 : 0;
						inp_interp[1] = (value & 2) != 0 ? 254 : 0;
						inp_interp[2] = (value & 4) != 0 ? 254 : 0;
					}
					inp_interp[3] = 254;
				}
				else if (distort->draw.pixelstep[plane] == 1)
				{
					if (plane == 0)			// y
						inp_interp[0] = (uint8_t)(((value & 1) ? 76 * 255 : 0) + ((value & 2) ? 150 * 255 : 0) + ((value & 4) ? 29 * 255 : 0) >> 8);
					else if (plane == 1) {	// u
						int8_t u = ((value & 1) ? -43 * 255 : 0) + ((value & 2) ? -84 * 255 : 0) + ((value & 4) ? 127 * 255 : 0) >> 8;
						inp_interp[0] = (uint8_t)(u + 128);
					}
					else {					// v
						int8_t v = ((value & 1) ? 127 * 255 : 0) + ((value & 2) ? -106 * 255 : 0) + ((value & 4) ? -21 * 255 : 0) >> 8;
						inp_interp[0] = (uint8_t)(v + 128);
					}
				}
				else
                    memcpy(inp_interp, gray, 4);
           	}
			else {
				if (ps == NULL) {
					pin = in->data[plane] + j * in->linesize[plane] + i * distort->draw.pixelstep[plane];
				}
				else {
					x = ps->column >> td->hsub;
					y = ps->row >> td->vsub;
					if (ps != NULL) {
						ps += 1 << td->hsub;
						x1 = x>>16;
						y1 = y>>16;
					}
					else {
						x1 = i;
						y1 = j;
					}
					pout = out->data[plane] + j * out->linesize[plane] + i * distort->draw.pixelstep[plane];

					/* the out-of-range values avoid border artifacts */
					if (x1 >= 0 && x1 <= inw && y1 >= 0 && y1 <= inh) {
						if (distort->interpolate != NULL) {
							pin = distort->interpolate(inp_interp, distort->pGamma,
													   in->data[plane], in->linesize[plane], distort->draw.pixelstep[plane],
													   x, y, inw-1, inh-1);
						}
						else {
							int x2 = av_clip(x1, 0, inw-1);
							int y2 = av_clip(y1, 0, inh-1);
							pin = in->data[plane] + y2 * in->linesize[plane] + x2 * distort->draw.pixelstep[plane];
						}
					}
					else {
						pin = &black.c[0];
						bDefaultColor = 1;
					}
				}
			}
            
			if (!bTimeStamp && distort->pMask != NULL)
			{
            	pm += 1 << td->vsub;
            	
				switch (distort->draw.pixelstep[plane]) {
					case 1:
						if (bDefaultColor)
							*pout = plane == 0 ? 0 : 128;
						else {
							if (plane == 0)
								*pout = (*pin * *pm) >> 8;
							else {
								*pout = (uint8_t)(((*pin - 128) * (int16_t)(*pm)) >> 8) + 128;
							//	*pout = (uint8_t)((((*pin + 128) * *pm) >> 8) - 128);
							}
						}
						break;
					case 2:
						if (bDefaultColor)
							memcpy(pout, &black.s, sizeof(uint16_t));
						//	*((uint16_t *)pout) = black.s;
						else {
							u.s[0] = *((uint16_t *)pin);
							u.c[0] = (u.c[0] * *pm) >> 8;
							u.c[1] = (u.c[1] * *pm) >> 8;
							*((uint16_t *)pout) = u.s[0];
						}
						break;
					case 3:
					case 4:
						if (bDefaultColor)
						//	*((uint32_t *)pout) = &black.l;
							memcpy(pout, &black.l, sizeof(uint32_t));
						else {
							u.l = *((uint32_t *)pin);
							u.c[0] = (u.c[0] * *pm) >> 8;
							u.c[1] = (u.c[1] * *pm) >> 8;
							u.c[2] = (u.c[2] * *pm) >> 8;
							u.c[3] = (u.c[3] * *pm) >> 8;
							*((uint32_t *)pout) = u.l;
						}
						break;
					default:
						if (bDefaultColor)
							memcpy(pout, &black.c[0], distort->draw.pixelstep[plane]);
						else
							memcpy(pout, pin, distort->draw.pixelstep[plane]);
						break;
				}
			}
			else
			{
				switch (distort->draw.pixelstep[plane]) {
					case 1:
						if (bDefaultColor)
							*pout = plane == 0 ? 0 : 128;
						else
							*pout = *pin;
						break;
					case 2:
						if (bDefaultColor)
							memcpy(pout, &black.s, sizeof(uint16_t));
						else
							*((uint16_t *)pout) = *((uint16_t *)pin);
						break;
					case 3:
					case 4:
						if (bDefaultColor)
							memcpy(pout, &black.s, sizeof(uint16_t));
						else
							*((uint32_t *)pout) = *((uint32_t *)pin);
						break;
					default:
						if (bDefaultColor)
							memcpy(pout, &black.c[0], distort->draw.pixelstep[plane]);
						else
							memcpy(pout, pin, distort->draw.pixelstep[plane]);
						break;
				}
			}
        }
    }
    
    return 0;
}

static int filter_frame(AVFilterLink *inlink, AVFrame *in)
{
    AVFilterContext *ctx = inlink->dst;
    AVFilterLink *outlink = ctx->outputs[0];
    AVFrame *out;
    DistortContext *distort = ctx->priv;
    int plane;
    
    out = ff_get_video_buffer(outlink, outlink->w, outlink->h);
    if (!out) {
        av_frame_free(&in);
        return AVERROR(ENOMEM);
    }
    av_frame_copy_props(out, in);
    
    for (plane = 0; plane < distort->nb_planes; plane++) {
        int hsub = plane == 1 || plane == 2 ? distort->hsub : 0;
        int vsub = plane == 1 || plane == 2 ? distort->vsub : 0;
        const int outw = FF_CEIL_RSHIFT(outlink->w, hsub);
        const int outh = FF_CEIL_RSHIFT(outlink->h, vsub);
        ThreadData td = { .in = in,   .out  = out,
            .inw  = FF_CEIL_RSHIFT(inlink->w, hsub),
            .inh  = FF_CEIL_RSHIFT(inlink->h, vsub),
            .hsub = hsub,
            .vsub = vsub,
            .outh = outh, .outw = outw,
            .frame = inlink->frame_count_in,
            .plane = plane };
        
        
        if (distort->debug_level > 1)
            av_log(ctx, AV_LOG_INFO, "filter_frame() - frame=%lld, plane%d, step=%d, hsub=%d, vsub=%d, inh=%d, inw=%d\n",
            									inlink->frame_count_in, plane, distort->draw.pixelstep[plane], hsub, vsub, td.inh, td.inw);
        
        ctx->internal->execute(ctx, filter_slice, &td, NULL, FFMIN(outh, ctx->graph->nb_threads));
    }
    
    av_frame_free(&in);
    if (distort->debug_level > 1)
    	av_log(ctx, AV_LOG_INFO, "filter_frame() - exit\n");
    return ff_filter_frame(outlink, out);
}

static const AVFilterPad distort_inputs[] = {
    {
        .name         = "default",
        .type         = AVMEDIA_TYPE_VIDEO,
        .filter_frame = filter_frame,
    },
    { NULL }
};

static const AVFilterPad distort_outputs[] = {
    {
        .name         = "default",
        .type         = AVMEDIA_TYPE_VIDEO,
        .config_props = config_props,
    },
    { NULL }
};

AVFilter ff_vf_distort = {
    .name          = "distort",
    .description   = NULL_IF_CONFIG_SMALL("Distort the input image."),
    .priv_size     = sizeof(DistortContext),
    .init          = init,
    .uninit        = uninit,
    .query_formats = query_formats,
    .inputs        = distort_inputs,
    .outputs       = distort_outputs,
    .priv_class    = &distort_class,
    .flags         = AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC | AVFILTER_FLAG_SLICE_THREADS,
};
