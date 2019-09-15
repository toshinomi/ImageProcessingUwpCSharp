using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class SettingImageProcessingPage : Page
    {
        public SettingImageProcessingPage()
        {
            InitializeComponent();

            LoadParam();
        }

        ~SettingImageProcessingPage()
        {
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

        public void LoadParam()
        {
            var settings = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView(ComInfo.RESOURCE_IMG_PROC_TYPE);
            if (settings != null)
            {
                List<ComImageProcessingType> items = new List<ComImageProcessingType>();
                items.Add(new ComImageProcessingType(int.Parse(settings.GetString(ComInfo.IMG_TYPE_EDGE_ID)), settings.GetString(ComInfo.IMG_TYPE_EDGE_NAME)));
                items.Add(new ComImageProcessingType(int.Parse(settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_ID)), settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_NAME)));
                items.Add(new ComImageProcessingType(int.Parse(settings.GetString(ComInfo.IMG_TYPE_BINARIZATION_ID)), settings.GetString(ComInfo.IMG_TYPE_BINARIZATION_NAME)));
                items.Add(new ComImageProcessingType(int.Parse(settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_2DIFF_ID)), settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_2DIFF_NAME)));
                items.Add(new ComImageProcessingType(int.Parse(settings.GetString(ComInfo.IMG_TYPE_COLOR_REVERSAL_ID)), settings.GetString(ComInfo.IMG_TYPE_COLOR_REVERSAL_NAME)));
                cmbBoxImageProcessingType.Items.Add(settings.GetString(ComInfo.IMG_TYPE_EDGE_NAME));
                cmbBoxImageProcessingType.Items.Add(settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_NAME));
                cmbBoxImageProcessingType.Items.Add(settings.GetString(ComInfo.IMG_TYPE_BINARIZATION_NAME));
                cmbBoxImageProcessingType.Items.Add(settings.GetString(ComInfo.IMG_TYPE_GRAY_SCALE_2DIFF_NAME));
                cmbBoxImageProcessingType.Items.Add(settings.GetString(ComInfo.IMG_TYPE_COLOR_REVERSAL_NAME));

                string strImgTypeSelectName = ComFunc.GetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME);
                if (!string.IsNullOrWhiteSpace(strImgTypeSelectName))
                {
                    cmbBoxImageProcessingType.SelectedIndex = (int)items.Find(x => x.Name == strImgTypeSelectName)?.Id - 1;
                }
                if (cmbBoxImageProcessingType.SelectedIndex == -1)
                {
                    cmbBoxImageProcessingType.SelectedIndex = 0;
                }
            }

            return;
        }

        public void SaveParam()
        {
            string strProcTypeName = (string)cmbBoxImageProcessingType.SelectedItem;
            ComFunc.SetStringApplicationDataContainer(ComInfo.IMG_TYPE_SELECT_NAME, strProcTypeName);

            return;
        }

        public void OnSelectionChangedCmbBoxImageProcessingType(object sender, SelectionChangedEventArgs e)
        {
            SaveParam();
        }
    }
}
