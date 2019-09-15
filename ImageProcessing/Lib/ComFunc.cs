using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

public class ComFunc
{
    public ComFunc()
    {
    }

    ~ComFunc()
    {
    }

    static public byte DoubleToByte(double _dValue)
    {
        byte nCnvValue = 0;

        if (_dValue > 255.0)
        {
            nCnvValue = 255;
        }
        else if (_dValue < 0)
        {
            nCnvValue = 0;
        }
        else
        {
            nCnvValue = (byte)_dValue;
        }

        return nCnvValue;
    }

    static public byte LongToByte(long _nValue)
    {
        byte nCnvValue = 0;

        if (_nValue > 255)
        {
            nCnvValue = 255;
        }
        else if (_nValue < 0)
        {
            nCnvValue = 0;
        }
        else
        {
            nCnvValue = (byte)_nValue;
        }

        return nCnvValue;
    }

    static public async Task<SoftwareBitmap> CreateSoftwareBitmap(StorageFile _file, BitmapImage _bitmap)
    {
        IRandomAccessStream random = await RandomAccessStreamReference.CreateFromFile(_file).OpenReadAsync();
        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(random);

        var softwareBitmap = new SoftwareBitmap(BitmapPixelFormat.Rgba8, _bitmap.PixelWidth, _bitmap.PixelHeight);
        return softwareBitmap = await decoder.GetSoftwareBitmapAsync();
    }

    static public async Task<SoftwareBitmapSource> ConvertToSoftwareBitmapSource(SoftwareBitmap _bitmap)
    {
        SoftwareBitmap displayableImage = SoftwareBitmap.Convert(_bitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
        await bitmapSource.SetBitmapAsync(displayableImage);

        return bitmapSource;
    }

    static public string GetStringApplicationDataContainer(string _strKey)
    {
        string strValue = "";

        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        if (container.Values.ContainsKey(_strKey))
        {
            strValue = container.Values[_strKey].ToString();
        }

        return strValue;
    }

    static public void SetStringApplicationDataContainer(string _strKey, string _strValue)
    {
        ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
        container.Values[_strKey] = _strValue;
    }
}
