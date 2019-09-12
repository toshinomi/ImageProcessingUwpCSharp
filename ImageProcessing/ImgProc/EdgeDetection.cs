using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

class EdgeDetection : ComImgProc
{
    public EdgeDetection()
    {
    }

    public EdgeDetection(SoftwareBitmap _softwareBitmap) : base(_softwareBitmap)
    {
    }

    ~EdgeDetection()
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

                                        lCalB += pData[nPixel2 + (int)ComInfo.Pixel.B] * nMask[nIdxWidthMask, nIdxHightMask];
                                        lCalG += pData[nPixel2 + (int)ComInfo.Pixel.G] * nMask[nIdxWidthMask, nIdxHightMask];
                                        lCalR += pData[nPixel2 + (int)ComInfo.Pixel.R] * nMask[nIdxWidthMask, nIdxHightMask];
                                    }
                                }
                            }
                            pData[nPixel + (int)ComInfo.Pixel.B] = ComFunc.LongToByte(lCalB);
                            pData[nPixel + (int)ComInfo.Pixel.G] = ComFunc.LongToByte(lCalG);
                            pData[nPixel + (int)ComInfo.Pixel.R] = ComFunc.LongToByte(lCalR);
                        }
                    }
                }
            }
        }

        return bRst;
    }
}
