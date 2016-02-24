using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class FinalRoute : Page
    {
        private BasicGeoposition SourcePosition { get; set; } = HomePage.Current.SourcePosition.Value;
        private BasicGeoposition DestinationPosition { get; set; } = HomePage.Current.DestinationPosition.Value;
        private MapIcon SourceMapIcon { get; set; } = HomePage.Current.SourceMapIcon;
        private MapIcon DestinationMapIcon { get; set; } = HomePage.Current.DestinationMapIcon;

        private const string WebURL = "http://128.199.133.85:8000/get_hawa/get/";
        /// <summary>
        /// List of all points to be used in making Route
        /// </summary>
        private List<Geopoint> AllPoints { get; set; } = null;

        public FinalRoute()
        {
            this.InitializeComponent();

            AllPoints = new List<Geopoint>();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Debug.WriteLine("BackRequested");
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };

        }      

        private void AddJsonPoints(string json)
        {
            try
            {
                Regex reg = new Regex(@"\[(?<latitude>[\.\d]+), (?<longitude>[\.\d]+)\]");

                foreach (Match m in reg.Matches(json))
                {
                    double regLatitude = double.Parse(m.Groups["latitude"].Value);
                    double regLongitude = double.Parse(m.Groups["longitude"].Value);
                    Debug.WriteLine("Latitude : " + m.Groups["latitude"].Value);
                    Debug.WriteLine("Longitude : " + m.Groups["longitude"].Value);

                    AllPoints.Add(new Geopoint(new BasicGeoposition() { Latitude = regLatitude, Longitude = regLongitude }));
                }
            }
            catch
            {
                // continue
            }
        }

        private async Task GetAllPoints()
        {
            try
            {
                AllPoints.Add(new Geopoint(SourcePosition));

                try
                {
                    string URL = WebURL + string.Format("?source_lat={0}&source_long={1}&dest_lat={2}&dest_long={3}", SourcePosition.Latitude.ToString(), SourcePosition.Longitude.ToString(), DestinationPosition.Latitude.ToString(), DestinationPosition.Longitude.ToString());

                    string json;
                    //string URL = "http://www.google.com";
                    using (var client = new HttpClient())
                    {
                        json = await client.GetStringAsync(URL);
                    }

                    //string json = "{\"1\": [25.2628193, 82.9897208], \"2\": [25.263333, 82.9896565], \"3\": [25.2635136, 82.99269079999999], \"4\": [25.27001, 82.9990535], \"5\": [25.2777705, 83.00220929999999], \"6\": [25.2819148, 83.0030073], \"7\": [25.2880692, 82.9991783], \"8\": [25.2907264, 82.99535639999999], \"9\": [25.2913254, 82.9921354], \"10\": [25.2901792, 82.9899698], \"11\": [25.2898349, 82.98303279999999], \"12\": [25.2892961, 82.96987209999999], \"13\": [25.3019357, 82.96845359999999], \"14\": [25.3003501, 82.9511617], \"15\": [25.2946962, 82.9503714]}";

                    AddJsonPoints(json);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Nothing Found");
                }


                //AllPoints.Add(new Geopoint(new BasicGeoposition() { Latitude = 25.2628193, Longitude = 82.9897208 }));
                //AllPoints.Add(new Geopoint(new BasicGeoposition() { Latitude = 25.2946962, Longitude = 82.9503714 }));

                AllPoints.Add(new Geopoint(DestinationPosition));
            }
            catch
            {
                // continue
            }
        }

        private async Task PlotAllPoints()
        {
            try
            {
                MapControl1.MapElements.Add(SourceMapIcon);
                MapControl1.MapElements.Add(DestinationMapIcon);

                MapRouteFinderResult routeResult =
                    await MapRouteFinder.GetDrivingRouteFromWaypointsAsync(AllPoints);

                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);

                viewOfRoute.RouteColor = Colors.Blue;
                viewOfRoute.OutlineColor = Colors.Black;

                MapControl1.Routes.Add(viewOfRoute);

                ShowTimeBlock.Text = "Time : " + routeResult.Route.EstimatedDuration.ToString();
                ShowPathBlock.Text = "Distance : " + routeResult.Route.LengthInMeters + "m";

                await MapControl1.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    MapAnimationKind.Default);

                double angle = angleFromCoordinate(SourcePosition.Latitude, SourcePosition.Longitude,
                    DestinationPosition.Latitude, DestinationPosition.Longitude);

                Debug.WriteLine(angle);
                await MapControl1.TryRotateAsync(-1.0 * angle);

                await MapControl1.TryTiltAsync(55);
            }
            catch
            {
                // continue
            }
        }

        private double angleFromCoordinate(double lat1, double long1, double lat2, double long2)
        {

            double dLon = (long2 - long1);

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
                    * Math.Cos(lat2) * Math.Cos(dLon);

            double brng = Math.Atan2(y, x);

            brng = brng * 180.0 / Math.PI;
            brng = (brng + 360.0) % 360.0;
            brng = 360.0 - brng;

            return brng;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetAllPoints();

            await PlotAllPoints();
        }
    }
}
