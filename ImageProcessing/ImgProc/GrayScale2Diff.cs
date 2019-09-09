using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

class GrayScale2Diff : ComImgProc
{
    public GrayScale2Diff()
    {
    }

    public GrayScale2Diff(SoftwareBitmap _softwareBitmap) : base(_softwareBitmap)
    {
    }

    ~GrayScale2Diff()
    {
        base.m_softwareBitmap = null;
    }

    public override bool GoImgProc(CancellationToken _token)
    {
        bool bRst = true;
        base.m_nStatus = (int)ComInfo.ImageProcStatus.Implemented;

        short[,] nMask =
        {
            {1,  1, 1},
            {1, -8, 1},
            {1,  1, 1}
        };

        int nIdxWidth;
        int nIdxHeight;
        int nMasksize = nMask.GetLength(0);
        unsafe
        {
            using (var buffer = base.m_softwareBitmap.LockBuffer(BitmapBufferAccessMode.ReadWrite))
            using (var reference = buffer.CreateReference())
            {
                if (reference is IMemoryBufferByteAccess)
                {
                    byte* pData;
                    uint nCapacity;
                    ((IMemoryBufferByteAccess)reference).GetBuffer(out pData, out nCapacity);

                    var desc = buffer.GetPlaneDescription(0);

                    for (nIdxHeight = 0; nIdxHeight < desc.Height; nIdxHeight++)
                    {
                        if (_token.IsCancellationRequested)
                        {
                            bRst = false;
                            base.m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
                            break;
                        }

                        for (nIdxWidth = 0; nIdxWidth < desc.Width; nIdxWidth++)
                        {
                            if (_token.IsCancellationRequested)
                            {
                                bRst = false;
                                base.m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
                                break;
                            }

                            var nPixel = desc.StartIndex + desc.Stride * nIdxHeight + 4 * nIdxWidth;



                            long lCalB = 0;
                            long lCalG = 0;
                            long lCalR = 0;
                            double dCalAve = 0.0;
                            int nIdxWidthMask;
                            int nIdxHightMask;


                            for (nIdxHightMask = 0; nIdxHightMask < nMasksize; nIdxHightMask++)
                            {
                                for (nIdxWidthMask = 0; nIdxWidthMask < nMasksize; nIdxWidthMask++)
                                {
                                    if (nIdxWidth + nIdxWidthMask > 0 &&
                                        nIdxWidth + nIdxWidthMask < desc.Width &&
                                        nIdxHeight + nIdxHightMask > 0 &&
                                        nIdxHeight + nIdxHightMask < desc.Height)
                                    {

                                        var nPixel2 = desc.StartIndex + desc.Stride * (nIdxHeight + nIdxHightMask) + 4 * (nIdxWidth + nIdxWidthMask);

                                       // byte* pPixel2 = (byte*)m_wBitmap.BackBuffer + (nIdxHeight + nIdxHightMask) * m_wBitmap.BackBufferStride + (nIdxWidth + nIdxWidthMask) * 4;

                                        lCalB = pPixel2[(int)ComInfo.Pixel.B] * nMask[nIdxWidthMask, nIdxHightMask];
                                        lCalG = pPixel2[(int)ComInfo.Pixel.G] * nMask[nIdxWidthMask, nIdxHightMask];
                                        lCalR = pPixel2[(int)ComInfo.Pixel.R] * nMask[nIdxWidthMask, nIdxHightMask];

                                        double dcalGray = (lCalB + lCalG + lCalR) / 3;
                                        dCalAve = (dCalAve + dcalGray) / 2;
                                    }
                                }
                            }


                            byte nPixelB = pData[nPixel + (int)ComInfo.Pixel.B];
                            byte nPixelG = pData[nPixel + (int)ComInfo.Pixel.G];
                            byte nPixelR = pData[nPixel + (int)ComInfo.Pixel.R];

                            byte nGrayScale = (byte)((nPixelB + nPixelG + nPixelR) / 3);

                            byte nBinarization = nGrayScale >= 125 ? (byte)255 : (byte)0;
                            pData[nPixel + (int)ComInfo.Pixel.B] = nBinarization;
                            pData[nPixel + (int)ComInfo.Pixel.G] = nBinarization;
                            pData[nPixel + (int)ComInfo.Pixel.R] = nBinarization;
                        }
                    }
                }
            }
        }

        return bRst;
    }
}
