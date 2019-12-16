using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

/// <summary>
/// ヒストグラムデータ
/// </summary>
class ComHistgramData
{
    private SoftwareBitmap m_bitmapOrigianl;
    private SoftwareBitmap m_bitmapAfter;

    /// <summary>
    /// オリジナルのビットマップ
    /// </summary>
    public SoftwareBitmap SoftwareBitmapOriginal
    {
        set { m_bitmapOrigianl = value; }
        get { return m_bitmapOrigianl; }
    }

    /// <summary>
    /// 画像処理後のビットマップ
    /// </summary>
    public SoftwareBitmap SoftwareBitmapAfter
    {
        set { m_bitmapAfter = value; }
        get { return m_bitmapAfter; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ComHistgramData()
    {
        m_bitmapOrigianl = null;
        m_bitmapAfter = null;
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~ComHistgramData()
    {
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        m_bitmapOrigianl = null;
        m_bitmapAfter = null;
    }
}
