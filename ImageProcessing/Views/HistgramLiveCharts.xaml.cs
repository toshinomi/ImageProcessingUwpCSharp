using LiveCharts;
using LiveCharts.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace ImageProcessing.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class HistgramLiveCharts : Page
    {
        private int[,] m_nHistgram = new int[(int)ComInfo.PictureType.MAX, ComInfo.RGB_MAX];
        private SeriesCollection m_seriesCollection = new SeriesCollection();
        private SoftwareBitmap m_softwareBitmapOriginal;
        private SoftwareBitmap m_softwareBitmapAfter;

        public HistgramLiveCharts()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ComNavigateHistgramData param = (ComNavigateHistgramData)e.Parameter;
            m_softwareBitmapOriginal = param.SoftwareBitmapOriginal;
            m_softwareBitmapAfter = param.SoftwareBitmapAfter;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack;
            base.OnNavigatedTo(e);

            DrawHistgram();
        }

        public void GoBack(object obj, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                if (e != null)
                {
                    e.Handled = true;
                }
            }
        }

        public void DrawHistgram()
        {
            InitHistgram();

            if (m_softwareBitmapOriginal != null)
            {
                CalHistgram(m_softwareBitmapOriginal, (int)ComInfo.PictureType.Original);
            }
            if (m_softwareBitmapAfter != null)
            {
                CalHistgram(m_softwareBitmapAfter, (int)ComInfo.PictureType.After);
            }

            var chartValueOriginal = new ChartValues<int>();
            var chartValueAfter = new ChartValues<int>();
            for (int nType = 0; nType < (int)ComInfo.PictureType.MAX; nType++)
            {
                for (int nIdx = 0; nIdx < (m_nHistgram.Length >> 1); nIdx++)
                {
                    if (nType == 0)
                    {
                        chartValueOriginal.Add(m_nHistgram[nType, nIdx]);
                    }
                    else if (nType == 1)
                    {
                        chartValueAfter.Add(m_nHistgram[nType, nIdx]);
                    }
                }
            }

            var seriesCollection = new SeriesCollection();

            var lineSeriesChartOriginal = new LineSeries()
            {
                Values = chartValueOriginal,
                Title = "Histgram Original"
            };
            var lineSeriesChartAfter = new LineSeries()
            {
                Values = chartValueAfter,
                Title = "Histgram After"
            };
            seriesCollection.Add(lineSeriesChartOriginal);
            seriesCollection.Add(lineSeriesChartAfter);

            m_seriesCollection = seriesCollection;
            LiveChartsGraph.Series = m_seriesCollection;
        }

        public void CalHistgram(SoftwareBitmap _softwareBitmap, int _nIndex)
        {
            int nIdxWidth;
            int nIdxHeight;
            unsafe
            {
                using (var buffer = _softwareBitmap.LockBuffer(BitmapBufferAccessMode.ReadWrite))
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
                            for (nIdxWidth = 0; nIdxWidth < desc.Width; nIdxWidth++)
                            {
                                var nPixel = desc.StartIndex + desc.Stride * nIdxHeight + 4 * nIdxWidth;

                                byte nPixelB = pData[nPixel + (int)ComInfo.Pixel.B];
                                byte nPixelG = pData[nPixel + (int)ComInfo.Pixel.G];
                                byte nPixelR = pData[nPixel + (int)ComInfo.Pixel.R];

                                byte nGrayScale = (byte)((nPixelB + nPixelG + nPixelR) / 3);

                                m_nHistgram[_nIndex, nGrayScale] += 1;
                            }
                        }
                    }
                }
            }
        }

        public void InitHistgram()
        {
            for (int nIdx = 0; nIdx < (m_nHistgram.Length >> 1); nIdx++)
            {
                m_nHistgram[(int)ComInfo.PictureType.Original, nIdx] = 0;
                m_nHistgram[(int)ComInfo.PictureType.After, nIdx] = 0;
            }
        }
    }
}
