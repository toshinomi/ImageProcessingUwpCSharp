using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 画像処理設定のロジック
/// </summary>
public class ComImageProcessingType : ICloneable
{
    private int m_nId;
    private string m_strName;

    /// <summary>
    /// ID
    /// </summary>
    public int Id
    {
        set { m_nId = value; }
        get { return m_nId; }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        set { m_strName = value; }
        get { return m_strName; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ComImageProcessingType()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="_nId">ID</param>
    /// <param name="_strNmae">名称</param>
    public ComImageProcessingType(int _nId, string _strNmae)
    {
        m_nId = _nId;
        m_strName = _strNmae;
    }

    /// <summary>
    /// デスクトラクタ
    /// </summary>
    ~ComImageProcessingType()
    {
    }

    /// <summary>
    /// クローン
    /// </summary>
    /// <returns>画像処理設定のクローン</returns>
    public object Clone()
    {
        return (ComImageProcessingType)MemberwiseClone();
    }
}
