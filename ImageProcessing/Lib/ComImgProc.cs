using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

/// <summary>
/// 画像処理の共通のロジック
/// </summary>
abstract public class ComImgProc
{
    protected SoftwareBitmap m_softwareBitmap;
    protected int m_nStatus;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_bitmap">ビットマップ</param>
    public ComImgProc(SoftwareBitmap _softwareBitmap)
    {
        m_softwareBitmap = _softwareBitmap;
        m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ComImgProc()
    {
        m_softwareBitmap = null;
        m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~ComImgProc()
    {
    }

    /// <summary>
    /// 初期化
    /// </summary>
    virtual public void Init()
    {
        m_softwareBitmap = null;
        m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
    }

    /// <summary>
    /// ソフトウェアビットマップ
    /// </summary>
    public SoftwareBitmap SoftwareBitmap
    {
        set { m_softwareBitmap = value; }
        get { return m_softwareBitmap; }
    }

    /// <summary>
    /// ステータス
    /// </summary>
    public int Status
    {
        set { m_nStatus = value; }
        get { return m_nStatus; }
    }

    /// <summary>
    /// 画像処理実行の抽象
    /// </summary>
    /// <param name="_token">キャンセルトークン</param>
    /// <returns>実行結果 成功/失敗</returns>
    abstract public bool GoImgProc(CancellationToken _token);
}
