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
        private string m_strOpenFileName;
        private StorageFile m_storageFile;
        private GrayScale m_grayScale;
        private ColorReversal m_colorReversal;
        private Binarization m_binaraization;
        private GrayScale2Diff m_grayScale2Diff;
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
            m_strOpenFileName = "";
            m_storageFile = null;
            m_grayScale = new GrayScale();
            m_colorReversal = new ColorReversal();
            m_binaraization = new Binarization();
            m_grayScale2Diff = new GrayScale2Diff();
            m_tokenSource = new CancellationTokenSource();
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

            //m_grayScale.SoftwareBitmap = null;
            //m_grayScale.Status = (int)ComInfo.ImageProcStatus.NotImplemented;
            //m_colorReversal.SoftwareBitmap = null;
            //m_colorReversal.Status = (int)ComInfo.ImageProcStatus.NotImplemented;
            //m_binaraization.SoftwareBitmap = null;
            //m_binaraization.Status = (int)ComInfo.ImageProcStatus.NotImplemented;
            m_grayScale2Diff.SoftwareBitmap = null;
            m_grayScale2Diff.Status = (int)ComInfo.ImageProcStatus.NotImplemented;
            m_bitmap = null;

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
                    //pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(m_grayScale.SoftwareBitmap);
                    //pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(m_colorReversal.SoftwareBitmap);
                    //pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(m_binaraization.SoftwareBitmap);
                    pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(m_grayScale2Diff.SoftwareBitmap);
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

                var softwareBitmap = await ComFunc.CreateSoftwareBitmap(m_storageFile, m_bitmap);
                //m_grayScale.SoftwareBitmap = softwareBitmap;
                //m_colorReversal.SoftwareBitmap = softwareBitmap;
                //m_binaraization.SoftwareBitmap = softwareBitmap;
                m_grayScale2Diff.SoftwareBitmap = softwareBitmap;
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
            CancellationToken token = m_tokenSource.Token;
            //bool bRst = await Task.Run(() => m_grayScale.GoImgProc(token));
            //bool bRst = await Task.Run(() => m_colorReversal.GoImgProc(token));
            //bool bRst = await Task.Run(() => m_binaraization.GoImgProc(token));
            bool bRst = await Task.Run(() => m_grayScale2Diff.GoImgProc(token));
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
            var navigateHistgramData = new ComNavigateHistgramData();
            if (m_bitmap != null)
            {
                navigateHistgramData.SoftwareBitmapOriginal = await ComFunc.CreateSoftwareBitmap(m_storageFile, m_bitmap);
            }
            //if (m_grayScale.SoftwareBitmap != null)
            //if (m_colorReversal.SoftwareBitmap != null)
            //if (m_colorReversal.Status == (int)ComInfo.ImageProcStatus.Implemented)
            //if (m_binaraization.Status == (int)ComInfo.ImageProcStatus.Implemented)
            if (m_grayScale2Diff.Status == (int)ComInfo.ImageProcStatus.Implemented)
            {
                //navigateHistgramData.SoftwareBitmapAfter = m_grayScale.SoftwareBitmap;
                //navigateHistgramData.SoftwareBitmapAfter = m_colorReversal.SoftwareBitmap;
                //navigateHistgramData.SoftwareBitmapAfter = m_binaraization.SoftwareBitmap;
                navigateHistgramData.SoftwareBitmapAfter = m_grayScale2Diff.SoftwareBitmap;
            }
            if (m_bitmap != null)
            {
                Frame.Navigate(typeof(HistgramLiveCharts), navigateHistgramData);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            base.OnNavigatedTo(e);
        }
    }
}
