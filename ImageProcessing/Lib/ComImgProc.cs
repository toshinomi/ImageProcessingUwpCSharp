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

    public ComImgProc(SoftwareBitmap _softwareBitmap)
    {
        m_softwareBitmap = _softwareBitmap;
    }

    public ComImgProc()
    {
    }

    ~ComImgProc()
    {
    }

    public SoftwareBitmap SoftwareBitmap
    {
        set { m_softwareBitmap = value; }
        get { return m_softwareBitmap; }
    }

    abstract public bool GoImgProc(CancellationToken _token);
}
