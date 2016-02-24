using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace whereAir
{
    public sealed partial class FacebookLogin : Page
    {
        /// <summary>
        /// Phone SID from AppManifest
        /// </summary>
        const string PhoneSID = "3fbea257-aa36-4e61-8e15-e8ea68394f74";
        /// <summary>
        /// Facebook App ID
        /// </summary>
        const string FacebookAppID = "869980273119279";
        /// <summary>
        /// Array having permissions required by the App
        /// </summary>
        private string[] PermissionList = {
            "public_profile"
        };
        public FacebookLogin()
        {
            this.InitializeComponent();

            User.FBAccount = FBSession.ActiveSession;

            User.FBAccount.FBAppId = FacebookAppID;
            User.FBAccount.WinAppId = PhoneSID;
        }

        private async void FacebookLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetLoading();

                FBPermissions permissions = new FBPermissions(PermissionList);
                FBResult result = await User.FBAccount.LoginAsync(permissions);

                if (result.Succeeded)
                {

                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    UnsetLoading();
                    await new MessageDialog("Unable to Login. Try to Login Again.").ShowAsync();
                }
            }
            catch
            {
                // continue
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetLoading();

                FBPermissions permissions = new FBPermissions(PermissionList);
                FBResult result = await User.FBAccount.LoginAsync(permissions);

                if (result.Succeeded)
                {

                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    UnsetLoading();
                }
            }
            catch
            {
                // continue
            }
        }

        private void SetLoading()
        {
            FacebookLoginButton.Content = "Loading";
            FacebookLoginButton.IsEnabled = false;
            LoadingRing.IsActive = true;
        }

        private void UnsetLoading()
        {
            FacebookLoginButton.Content = "Login";
            FacebookLoginButton.IsEnabled = true;
            LoadingRing.IsActive = false;
        }
    }
}
