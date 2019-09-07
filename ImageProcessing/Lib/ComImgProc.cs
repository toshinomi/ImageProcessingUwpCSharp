using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

abstract public class ComImgProc
{
    protected SoftwareBitmap m_softwareBitmap;
    protected int m_nStatus;

    public ComImgProc(SoftwareBitmap _softwareBitmap)
    {
        m_softwareBitmap = _softwareBitmap;
        m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
    }

    public ComImgProc()
    {
        m_softwareBitmap = null;
        m_nStatus = (int)ComInfo.ImageProcStatus.NotImplemented;
    }

    ~ComImgProc()
    {
    }

    public SoftwareBitmap SoftwareBitmap
    {
        set { m_softwareBitmap = value; }
        get { return m_softwareBitmap; }
    }

    public int Status
    {
        set { m_nStatus = value; }
        get { return m_nStatus; }
    }

    abstract public bool GoImgProc(CancellationToken _token);
}
