using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ComImgInfo
{
    private string m_strCurImgName;
    private ComEdgeDetectionInfo m_edgeDetectoinInfo;
    private ComGrayScaleInfo m_grayScaleInfo;
    private ComBinarizationInfo m_binarizationInfo;
    private ComGrayScale2DiffInfo m_grayScale2DiffInfo;
    private ComColorReversalInfo m_colorReversalInfo;

    public string CurImgName
    {
        set { m_strCurImgName = value; }
        get { return m_strCurImgName; }
    }

    public ComEdgeDetectionInfo EdgeDetectoinInfo
    {
        set { m_edgeDetectoinInfo = value; }
        get { return m_edgeDetectoinInfo; }
    }

    public ComGrayScaleInfo GrayScaleInfo
    {
        set { m_grayScaleInfo = value; }
        get { return m_grayScaleInfo; }
    }

    public ComBinarizationInfo BinarizationInfo
    {
        set { m_binarizationInfo = value; }
        get { return m_binarizationInfo; }
    }

    public ComGrayScale2DiffInfo GrayScale2DiffInfo
    {
        set { m_grayScale2DiffInfo = value; }
        get { return m_grayScale2DiffInfo; }
    }

    public ComColorReversalInfo ColorReversalInfo
    {
        set { m_colorReversalInfo = value; }
        get { return m_colorReversalInfo; }
    }

    public ComImgInfo()
    {
        m_edgeDetectoinInfo = new ComEdgeDetectionInfo();
        m_grayScaleInfo = new ComGrayScaleInfo();
        m_binarizationInfo = new ComBinarizationInfo();
        m_grayScale2DiffInfo = new ComGrayScale2DiffInfo();
        m_colorReversalInfo = new ComColorReversalInfo();
    }

    ~ComImgInfo()
    {
    }

    public void Init()
    {
        m_strCurImgName = "";
        m_edgeDetectoinInfo = null;
        m_grayScaleInfo = null;
        m_binarizationInfo = null;
        m_grayScale2DiffInfo = null;
        m_colorReversalInfo = null;
    }
}

public class ComEdgeDetectionInfo
{
    public ComEdgeDetectionInfo()
    {
    }

    ~ComEdgeDetectionInfo()
    {
    }
}

public class ComGrayScaleInfo
{
    public ComGrayScaleInfo()
    {
    }

    ~ComGrayScaleInfo()
    {
    }
}

public class ComBinarizationInfo
{
    private byte m_nThresh;
    public byte Thresh
    {
        set { m_nThresh = value; }
        get { return m_nThresh; }
    }

    public ComBinarizationInfo()
    {
    }

    ~ComBinarizationInfo()
    {
    }

    public void Init()
    {
        m_nThresh = 0;
    }
}

public class ComGrayScale2DiffInfo
{
    public ComGrayScale2DiffInfo()
    {
    }

    ~ComGrayScale2DiffInfo()
    {
    }
}

public class ComColorReversalInfo
{
    public ComColorReversalInfo()
    {
    }

    ~ComColorReversalInfo()
    {
    }
}
