using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

/// <summary>
/// 2値化のロジック
/// </summary>
class Binarization : ComImgProc
{
    private byte m_nThresh;

    /// <summary>
    /// 閾値
    /// </summary>
    public byte Thresh
    {
        set { m_nThresh = value; }
        get { return m_nThresh; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_bitmap">ビットマップ</param>
    public Binarization(SoftwareBitmap _softwareBitmap) : base(_softwareBitmap)
    {
        m_nThresh = 0;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_bitmap">ビットマップ</param>
    /// <param name="_nThresh">閾値</param>
    public Binarization(SoftwareBitmap _softwareBitmap, byte _nThresh) : base(_softwareBitmap)
    {
        m_nThresh = _nThresh;
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~Binarization()
    {
        base.m_softwareBitmap = null;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        m_nThresh = 0;
        base.Init();
    }

    /// <summary>
    /// 2値化の実行
    /// </summary>
    /// <param name="_token">キャンセルトークン</param>
    /// <returns>実行結果 成功/失敗</returns>
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

                            byte nBinarization = nGrayScale >= m_nThresh ? (byte)255 : (byte)0;
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
