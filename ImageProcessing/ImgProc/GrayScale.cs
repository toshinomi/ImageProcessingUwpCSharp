using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

public class GrayScale : ComImgProc
{
    public GrayScale(SoftwareBitmap _softwareBitmap) : base(_softwareBitmap)
    {
    }

    ~GrayScale()
    {
        base.m_softwareBitmap = null;
    }

    public override void Init()
    {
        base.Init();
    }

    public override bool GoImgProc(CancellationToken _token)
    {
        bool bRst = true;
        base.m_nStatus = (int)ComInfo.ImageProcStatus.Implemented;

        int nIdxWidth;
        int nIdxHeight;
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

                            byte nPixelB = pData[nPixel + (int)ComInfo.Pixel.B];
                            byte nPixelG = pData[nPixel + (int)ComInfo.Pixel.G];
                            byte nPixelR = pData[nPixel + (int)ComInfo.Pixel.R];

                            byte nGrayScale = (byte)((nPixelB + nPixelG + nPixelR) / 3);

                            pData[nPixel + (int)ComInfo.Pixel.B] = nGrayScale;
                            pData[nPixel + (int)ComInfo.Pixel.G] = nGrayScale;
                            pData[nPixel + (int)ComInfo.Pixel.R] = nGrayScale;
                        }
                    }
                }
            }
        }

        return bRst;
    }
}
