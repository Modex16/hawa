using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace whereAir
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            if (COPDCheckBox.IsChecked == true)
            {
                localSettings.Values["COPDCheckBox"] = true;
                flag = 1;
            }
            if (AsthmaCheckBox.IsChecked == true)
            {
                localSettings.Values["AsthmaCheckBox"] = true;
                flag = 1;
            }
            if (OtherLungDiseasesCheckBox.IsChecked == true)
            {
                localSettings.Values["OtherLungDiseasesCheckBox"] = true;
                flag = 1;
            }
            if (NoneCheckBox.IsChecked == true)
            {
                if (flag == 1)
                {
                    MessageDialog dialog = new MessageDialog("Invalid Selection");
                    await dialog.ShowAsync();
                    flag = -1;
                }
                else
                {
                    localSettings.Values["NoneCheckBox"] = true;
                }
            }
            if (flag != -1)
                if (composite == null)
                {
                    composite["COPDCheckBox"] = localSettings.Values["COPDCheckBox"];
                    composite["AsthmaCheckBox"] = localSettings.Values["AsthmaCheckBox"];
                    composite["OtherLungDiseasesCheckBox"] = localSettings.Values["OtherLungDiseasesCheckBox"];
                    composite["NoneCheckBox"] = localSettings.Values["NoneCheckBox"];
                    this.Frame.Navigate(typeof(MainPage));
                }
                else if (flag == 1)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    MessageDialog dialog = new MessageDialog("Select Valid Selection!");
                    await dialog.ShowAsync();
                }
        }
    }
}
