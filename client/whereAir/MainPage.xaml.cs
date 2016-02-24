using Facebook.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace whereAir
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ScenarioFrame.Navigate(typeof(HomePage));

            if (User.FBAccount.LoggedIn)
            {
                // Get current user
                FBUser user = User.FBAccount.User;
                // Set profile pic
                ProfilePic.UserId = user.Id;
                // Set User Name
                UserName.Text = user.Name;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyout m = new MenuFlyout();
            MenuFlyoutItem mn = new MenuFlyoutItem();
            mn.Text = "Team Hawa";
            m.Items.Add(mn);
            m.ShowAt((FrameworkElement)sender);
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyout m = new MenuFlyout();
            MenuFlyoutItem mn1 = new MenuFlyoutItem();
            mn1.Text = User.Name;
            m.Items.Add(mn1);
            /*MenuFlyoutItem mn2 = new MenuFlyoutItem();
            mn2.Text = "Email :" + User.Email;
            m.Items.Add(mn2);*/
            MenuFlyoutItem mn3 = new MenuFlyoutItem();
            mn3.Text = "Connected Using Facebook";
            m.Items.Add(mn3);
            m.ShowAt((FrameworkElement)sender);
        }
    }
}
