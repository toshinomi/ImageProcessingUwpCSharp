using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImageProcessing.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private BitmapImage m_bitmap;
        private SoftwareBitmap m_softwareBitmap;
        private string m_strOpenFileName;
        private StorageFile m_storageFile;
        private object m_imgProc;
        private CancellationTokenSource m_tokenSource;

        public MainPage()
        {
            InitializeComponent();

            InitMemberVariables();
        }

        ~MainPage()
        {
        }

        public void InitMemberVariables()
        {
            m_bitmap = null;
            m_softwareBitmap = null;
            m_strOpenFileName = "";
            m_storageFile = null;
            m_imgProc = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack;
            base.OnNavigatedTo(e);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public async void OnClickBtnFileSelect(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");

            m_storageFile = await openPicker.PickSingleFileAsync();
            if (m_storageFile != null)
            {
                pictureBoxOriginal.Source = null;
                pictureBoxAfter.Source = null;
                m_strOpenFileName = m_storageFile.Path;

                bool bLoadImageResult = await LoadImage();
                if (!bLoadImageResult)
                {
                    await new Windows.UI.Popups.MessageDialog("Open File Error").ShowAsync();
                }
            }
            return;
        }

        public void OnClickBtnAllClear(object sender, RoutedEventArgs e)
        {
            pictureBoxOriginal.Source = null;
            pictureBoxAfter.Source = null;
            m_imgProc = null;
            m_bitmap = null;
            m_softwareBitmap = null;

            return;
        }

        public async void OnClickBtnStart(object sender, RoutedEventArgs e)
        {
            pictureBoxAfter.Source = null;

            bool bLoadImageResult = await LoadImage();
            if (bLoadImageResult)
            {
                bool bTaskResult = await TaskWorkImageProcessing();
                if (bTaskResult)
                {
                    string strCurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
                    pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(SelectGetBitmap(strCurImgName));
                }
            }
            return;
        }

        public async Task<bool> LoadImage()
        {
            bool bRst = true;

            try
            {
                var openFile = await m_storageFile.OpenReadAsync();
                m_bitmap = new BitmapImage();
                m_bitmap.SetSource(openFile);
                pictureBoxOriginal.Source = m_bitmap;

                m_softwareBitmap = await ComFunc.CreateSoftwareBitmap(m_storageFile, m_bitmap);
                string strCurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
                SelectLoadImage(strCurImgName);
            }
            catch (Exception)
            {
                bRst = false;
                return bRst;
            }

            return bRst;
        }

        public async Task<bool> TaskWorkImageProcessing()
        {
            m_tokenSource = new CancellationTokenSource();
            CancellationToken token = m_tokenSource.Token;
            ComImgInfo imgInfo = new ComImgInfo();
            ComBinarizationInfo binarizationInfo = new ComBinarizationInfo();
            binarizationInfo.Thresh = (byte)sliderThresh.Value;
            imgInfo.CurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
            imgInfo.BinarizationInfo = binarizationInfo;
            bool bRst = await Task.Run(() => SelectGoImgProc(imgInfo, token));
            return bRst;
        }

        public void OnClickBtnStop(object sender, RoutedEventArgs e)
        {
            if (m_tokenSource != null)
            {
                m_tokenSource.Cancel();
            }

            return;
        }

        public async void OnClickBtnSaveImage(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("png file", new List<string>() {".png"});
            savePicker.SuggestedFileName = "Default";

            var storageFile = await savePicker.PickSaveFileAsync();
            if (storageFile != null)
            {
                var size = pictureBoxAfter.RenderSize;
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(pictureBoxAfter, (int)size.Width, (int)size.Height);

                var displayInformation = DisplayInformation.GetForCurrentView();

                using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData
                    (
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        displayInformation.LogicalDpi,
                        displayInformation.LogicalDpi,
                        (await renderTargetBitmap.GetPixelsAsync()).ToArray()
                    );
                    await encoder.FlushAsync();
                }
            }

            return;
        }

        public async void OnClickBtnShowHistgram(object sender, RoutedEventArgs e)
        {
            if (m_bitmap == null)
            {
                return;
            }

            var navigateHistgramData = new ComNavigateHistgramData();
            if (m_bitmap != null)
            {
                navigateHistgramData.SoftwareBitmapOriginal = await ComFunc.CreateSoftwareBitmap(m_storageFile, m_bitmap);
            }
            string strCurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
            if (SelectGetStatus(strCurImgName) == (int)ComInfo.ImageProcStatus.Implemented)
            {
                navigateHistgramData.SoftwareBitmapAfter = SelectGetBitmap(strCurImgName);
            }
            if (m_bitmap != null)
            {
                Frame.Navigate(typeof(HistgramLiveChartsPage), navigateHistgramData);
            }
        }

        public SoftwareBitmap SelectGetBitmap(string _strImgName)
        {
            SoftwareBitmap softwareBitmap = null;

            switch (_strImgName)
            {
                case ComInfo.IMG_NAME_EDGE_DETECTION:
                    var edge = (EdgeDetection)m_imgProc;
                    softwareBitmap = edge.SoftwareBitmap;
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE:
                    var gray = (GrayScale)m_imgProc;
                    softwareBitmap = gray.SoftwareBitmap;
                    break;
                case ComInfo.IMG_NAME_BINARIZATION:
                    var binarization = (Binarization)m_imgProc;
                    softwareBitmap = binarization.SoftwareBitmap;
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE_2DIFF:
                    var gray2Diff = (GrayScale2Diff)m_imgProc;
                    softwareBitmap = gray2Diff.SoftwareBitmap;
                    break;
                case ComInfo.IMG_NAME_COLOR_REVERSAL:
                    var colorReversal = (ColorReversal)m_imgProc;
                    softwareBitmap = colorReversal.SoftwareBitmap;
                    break;
                default:
                    break;

            }

            return softwareBitmap;
        }

        public bool SelectGoImgProc(ComImgInfo _comImgInfo, CancellationToken _token)
        {
            bool bRst = true;

            switch (_comImgInfo.CurImgName)
            {
                case ComInfo.IMG_NAME_EDGE_DETECTION:
                    var edge = (EdgeDetection)m_imgProc;
                    bRst = edge.GoImgProc(_token);
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE:
                    var gray = (GrayScale)m_imgProc;
                    bRst = gray.GoImgProc(_token);
                    break;
                case ComInfo.IMG_NAME_BINARIZATION:
                    var binarization = (Binarization)m_imgProc;
                    binarization.Thresh = _comImgInfo.BinarizationInfo.Thresh;
                    bRst = binarization.GoImgProc(_token);
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE_2DIFF:
                    var gray2Diff = (GrayScale2Diff)m_imgProc;
                    bRst = gray2Diff.GoImgProc(_token);
                    break;
                case ComInfo.IMG_NAME_COLOR_REVERSAL:
                    var colorReversal = (ColorReversal)m_imgProc;
                    bRst = colorReversal.GoImgProc(_token);
                    break;
                default:
                    break;
            }

            return bRst;
        }

        public int SelectGetStatus(string _strImgName)
        {
            int status = -1;

            switch (_strImgName)
            {
                case ComInfo.IMG_NAME_EDGE_DETECTION:
                    var edge = (EdgeDetection)m_imgProc;
                    status = edge.Status;
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE:
                    var gray = (GrayScale)m_imgProc;
                    status = gray.Status;
                    break;
                case ComInfo.IMG_NAME_BINARIZATION:
                    var binarization = (Binarization)m_imgProc;
                    status = binarization.Status;
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE_2DIFF:
                    var gray2Diff = (GrayScale2Diff)m_imgProc;
                    status = gray2Diff.Status;
                    break;
                case ComInfo.IMG_NAME_COLOR_REVERSAL:
                    var colorReversal = (ColorReversal)m_imgProc;
                    status = colorReversal.Status;
                    break;
                default:
                    break;

            }

            return status;
        }

        public bool SelectLoadImage(string _strImgName)
        {
            bool bRst = true;

            if (m_imgProc != null)
            {
                m_imgProc = null;
            }

            switch (_strImgName)
            {
                case ComInfo.IMG_NAME_EDGE_DETECTION:
                    m_imgProc = new EdgeDetection(m_softwareBitmap);
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE:
                    m_imgProc = new GrayScale(m_softwareBitmap);
                    break;
                case ComInfo.IMG_NAME_BINARIZATION:
                    m_imgProc = new Binarization(m_softwareBitmap);
                    break;
                case ComInfo.IMG_NAME_GRAY_SCALE_2DIFF:
                    m_imgProc = new GrayScale2Diff(m_softwareBitmap);
                    break;
                case ComInfo.IMG_NAME_COLOR_REVERSAL:
                    m_imgProc = new ColorReversal(m_softwareBitmap);
                    break;
                default:
                    break;
            }

            return bRst;
        }

        public void OnSliderPreviewKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            string strCurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
            if (strCurImgName != ComInfo.IMG_NAME_BINARIZATION)
            {
                return;
            }
            if (pictureBoxAfter.Source != null)
            {
                OnClickBtnStart(sender, e);
            }
        }


        private void OnSliderKeyUp(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            string strCurImgName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
            if (strCurImgName != ComInfo.IMG_NAME_BINARIZATION)
            {
                return;
            }
            if (pictureBoxAfter.Source != null)
            {
                OnClickBtnStart(sender, e);
            }
        }
    }
}
