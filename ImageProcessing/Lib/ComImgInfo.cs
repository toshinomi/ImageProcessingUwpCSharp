using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 画像処理の設定管理のロジック
/// </summary>
public class ComImgInfo
{
    private string m_strCurImgName;
    private EdgeDetectionInfo m_edgeDetectoinInfo;
    private GrayScaleInfo m_grayScaleInfo;
    private BinarizationInfo m_binarizationInfo;
    private GrayScale2DiffInfo m_grayScale2DiffInfo;
    private ColorReversalInfo m_colorReversalInfo;

    /// <summary>
    /// 現在の画像処理の名称
    /// </summary>
    public string CurImgName
    {
        set { m_strCurImgName = value; }
        get { return m_strCurImgName; }
    }

    /// <summary>
    /// エッジ検出の設定
    /// </summary>
    public EdgeDetectionInfo EdgeDetectoinInfo
    {
        set { m_edgeDetectoinInfo = value; }
        get { return m_edgeDetectoinInfo; }
    }

    /// <summary>
    /// グレースケールの設定
    /// </summary>
    public GrayScaleInfo GrayScaleInfo
    {
        set { m_grayScaleInfo = value; }
        get { return m_grayScaleInfo; }
    }

    /// <summary>
    /// 2値化の設定
    /// </summary>
    public BinarizationInfo BinarizationInfo
    {
        set { m_binarizationInfo = value; }
        get { return m_binarizationInfo; }
    }

    /// <summary>
    /// グレースケール2次微分の設定
    /// </summary>
    public GrayScale2DiffInfo GrayScale2DiffInfo
    {
        set { m_grayScale2DiffInfo = value; }
        get { return m_grayScale2DiffInfo; }
    }

    /// <summary>
    /// 色反転の設定
    /// </summary>
    public ColorReversalInfo ColorReversalInfo
    {
        set { m_colorReversalInfo = value; }
        get { return m_colorReversalInfo; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ComImgInfo()
    {
        m_edgeDetectoinInfo = new EdgeDetectionInfo();
        m_grayScaleInfo = new GrayScaleInfo();
        m_binarizationInfo = new BinarizationInfo();
        m_grayScale2DiffInfo = new GrayScale2DiffInfo();
        m_colorReversalInfo = new ColorReversalInfo();
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~ComImgInfo()
    {
    }
}

/// <summary>
/// エッジ検出の設定管理のロジック
/// </summary>
public class EdgeDetectionInfo
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EdgeDetectionInfo()
    {
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~EdgeDetectionInfo()
    {
    }
}

/// <summary>
/// グレースケールの設定管理のロジック
/// </summary>
public class GrayScaleInfo
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GrayScaleInfo()
    {
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~GrayScaleInfo()
    {
    }
}

/// <summary>
/// 2値化の設定管理のロジック
/// </summary>
public class BinarizationInfo
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
    public BinarizationInfo()
    {
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~BinarizationInfo()
    {
    }
}

/// <summary>
/// グレースケール2次微分の設定管理のロジック
/// </summary>
public class GrayScale2DiffInfo
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GrayScale2DiffInfo()
    {
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~GrayScale2DiffInfo()
    {
    }
}

/// <summary>
/// 色反転の設定管理のロジック
/// </summary>
public class ColorReversalInfo
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ColorReversalInfo()
    {
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~ColorReversalInfo()
    {
    }
}
