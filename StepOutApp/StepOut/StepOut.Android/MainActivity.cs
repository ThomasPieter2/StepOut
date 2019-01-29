using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Content;
using Android;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Plugin.Permissions;

namespace StepOut.Droid
{
    [Activity(Label = "Step Out", Icon = "@drawable/stepoutapp", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            if (adapter == null || adapter.IsEnabled == false)
            {
                int REQUEST_ENABLE_BT = 1;
                //Kijken of BT aanstaat, zoniet vragen om aan te zetten
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }

            const int locationPermissionsRequestCode = 1000;

            var locationPermissions = new[]
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
            };

            // check if the app has permission to access coarse location
            var coarseLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);

            // check if the app has permission to access fine location
            var fineLocationPermissionGranted = ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);

            // if either is denied permission, request permission from the user
            if (coarseLocationPermissionGranted == Permission.Denied ||
                fineLocationPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, locationPermissions, locationPermissionsRequestCode);
            }

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}