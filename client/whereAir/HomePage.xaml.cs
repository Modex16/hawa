using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace whereAir
{
    public sealed partial class HomePage : Page
    {
        public static HomePage Current { get; private set; }
        /// <summary>
        /// UIElement for "Find your Location" Above Map.
        /// </summary>
        public UIElement FindLocation { get; set; } = null;
        /// <summary>
        /// Enum for ToggleClick
        /// </summary>
        private enum ToggleClick
        {
            None,
            Source,
            Destination
        }
        /// <summary>
        /// Currently selected ToggleOption
        /// </summary>
        private ToggleClick Selected { get; set; } = ToggleClick.None;
        /// <summary>
        /// MapIcon of Source Point
        /// </summary>
        public MapIcon SourceMapIcon { get; private set; } = new MapIcon();
        /// <summary>
        /// Basic GeoPosition of SourcePoint
        /// </summary>
        public BasicGeoposition? SourcePosition { get; private set; } = null;
        /// <summary>
        /// MapIcon of Destination Point
        /// </summary>
        public MapIcon DestinationMapIcon { get; private set; } = new MapIcon();
        /// <summary>
        /// Basic GeoPosition of Destination Point
        /// </summary>
        public BasicGeoposition? DestinationPosition { get; private set; } = null;

        public HomePage()
        {
            Current = this;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.InitializeComponent();
            SourceMapIcon.Title = "Initial Location";
            DestinationMapIcon.Title = "Final Location";

            SetMapPosition();
        }

        private async void SetMapPosition()
        {
            FindLocation = MainStackPanel.Children[0];
            MainStackPanel.Children.RemoveAt(0);

            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // Show "Finding your Location"
                    MainStackPanel.Children.Insert(0, FindLocation);
                    //BasicGeoposition snPosition = new BasicGeoposition() { Latitude = 25.367, Longitude = 82.996 };

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;
                    //Geopoint myLocation = new Geopoint(snPosition);

                    MainStackPanel.Children.RemoveAt(0);

                    await MapControl1.TrySetViewAsync(myLocation, 14.5);

                    MapIcon mapIcon1 = new MapIcon() { Location = myLocation, Title = "Your Location" };
                    MapControl1.MapElements.Add(mapIcon1);

                    SourceMapIcon.Location = myLocation;
                    SourcePosition = myLocation.Position;
                    MapControl1.MapElements.Add(SourceMapIcon);

                    MapFindProgressBar.IsActive = false;
                    MapFindTextBlock.Visibility = Visibility.Collapsed;

                    break;

                case GeolocationAccessStatus.Denied:
                    // Handle the case  if access to location is denied.
                    await new MessageDialog("Seems like you didn't allowed us for using Locations Services or your Location " +
                        "Setting is turned off. This App will not work without Locations Services. Thanks!!").ShowAsync();
                    Application.Current.Exit();
                    break;

                case GeolocationAccessStatus.Unspecified:
                    // Handle the case if  an unspecified error occurs.
                    await new MessageDialog("Seems like you didn't allowed us for using Locations Services or your Location " +
                        "Setting is turned off. This App will not work without Locations Services. Thanks!!").ShowAsync();
                    Application.Current.Exit();
                    break;
            }

        }

        private void MapControl1_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            var tappedGeoPosition = args.Location.Position;
            switch (Selected)
            {
                case ToggleClick.None:
                    break;
                case ToggleClick.Source:
                    SourcePosition = tappedGeoPosition;

                    DestinationButton.IsEnabled = true;
                    SourceButton.IsEnabled = true;
                    DestinationButton.IsChecked = false;
                    SourceButton.IsChecked = false;

                    MapControl1.MapElements.Remove(SourceMapIcon);
                    SourceMapIcon.Location = new Geopoint(SourcePosition.Value);
                    MapControl1.MapElements.Add(SourceMapIcon);

                    Selected = ToggleClick.None;
                    break;
                case ToggleClick.Destination:
                    DestinationPosition = tappedGeoPosition;

                    SourceButton.IsEnabled = true;
                    DestinationButton.IsEnabled = true;
                    DestinationButton.IsChecked = false;
                    SourceButton.IsChecked = false;

                    MapControl1.MapElements.Remove(DestinationMapIcon);
                    DestinationMapIcon.Location = new Geopoint(DestinationPosition.Value);
                    MapControl1.MapElements.Add(DestinationMapIcon);

                    Selected = ToggleClick.None;
                    break;
                default:
                    break;
            }
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourcePosition == null && DestinationPosition == null)
            {
                await new MessageDialog("Please provide Source as well as Destination Positions.").ShowAsync();
            }
            else if (SourcePosition == null)
            {
                await new MessageDialog("Please provide Source Position.").ShowAsync();
            }
            else if (DestinationPosition == null)
            {
                await new MessageDialog("Please provide Destination Position.").ShowAsync();
            }
            else
            {
                MainStackPanel.Children.Insert(0, FindLocation);
                MapFindTextBlock.Text = "Finding your Route";
                this.Frame.Navigate(typeof(FinalRoute));
            }
        }

        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (SourceButton.IsChecked.Value)
            {
                Selected = ToggleClick.Source;
                DestinationButton.IsEnabled = false;
            }
            else
            {
                Selected = ToggleClick.None;
                DestinationButton.IsEnabled = true;
            }

        }

        private void DestinationButton_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationButton.IsChecked.Value)
            {
                Selected = ToggleClick.Destination;
                SourceButton.IsEnabled = false;
            }
            else
            {
                Selected = ToggleClick.None;
                SourceButton.IsEnabled = true;
            }
        }
    }
}
