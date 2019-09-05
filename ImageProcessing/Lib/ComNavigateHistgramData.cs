using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

class ComNavigateHistgramData
{
    private SoftwareBitmap m_bitmapOrigianl;
    private SoftwareBitmap m_bitmapAfter;

    public SoftwareBitmap SoftwareBitmapOriginal
    {
        set { m_bitmapOrigianl = value; }
        get { return m_bitmapOrigianl; }
    }

    public SoftwareBitmap SoftwareBitmapAfter
    {
        set { m_bitmapAfter = value; }
        get { return m_bitmapAfter; }
    }

    public ComNavigateHistgramData()
    {
        m_bitmapOrigianl = null;
        m_bitmapAfter = null;
    }

    ~ComNavigateHistgramData()
    {
    }
}
