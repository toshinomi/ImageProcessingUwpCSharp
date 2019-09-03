using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageProcessing.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private BitmapImage m_bitmap;
        private string m_strOpenFileName;
        private StorageFile m_storageFile;
        private GrayScale m_grayScale;
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
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");

            m_storageFile = await picker.PickSingleFileAsync();
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
            this.pictureBoxOriginal.Source = null;
            this.pictureBoxAfter.Source = null;
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
                    pictureBoxAfter.Source = await ComFunc.ConvertToSoftwareBitmapSource(m_grayScale.SoftwareBitmap);
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
                m_grayScale.SoftwareBitmap = softwareBitmap;
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
            bool bRst = await Task.Run(() => m_grayScale.GoImgProc(token));
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
    }
}
