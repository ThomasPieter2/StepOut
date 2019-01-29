using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using StepOut.BluetoothConnect;
using StepOut.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StepOut.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectBluetoothPage : ContentPage
    { 
        #region Variables
        IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        IBluetoothLE ble = CrossBluetoothLE.Current;
        IDevice device;
        List<IDevice> deviceList = new List<IDevice>();
        ICharacteristic characteristic;
        List<int> heartRates = new List<int>();
        bool connected2Device = false;

        BluetoothManager btManager = new BluetoothManager();
        #endregion

        public ConnectBluetoothPage()
        {
            InitializeComponent();
            StartUp();
        }

        private async Task StartUp()
        {
            try
            {
                lvwItems.ItemsSource = null;
                actLoading.IsEnabled = true;
                actLoading.IsVisible = true;

                deviceList = await btManager.Scan(adapter);
                actLoading.IsEnabled = false;
                actLoading.IsVisible = false;

                if (deviceList != null)
                {
                    lvwItems.ItemsSource = deviceList;
                }
                else
                {
                    await DisplayAlert("Waarschuwing", "Er zijn geen bluetooth hartslagsensoren gevonden", "Ok");
                }
                btnScan.IsEnabled = true;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het scannen voor bluetooth devices, als dit blijft voorkomen gelieve contact op te nemen met de suport.", "Ok");
            }
        }

        #region Scan & Connect

        private async void BtnScan_Clicked(object sender, EventArgs e)
        {
            StartUp();
        }

        private async void LvwItems_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (lvwItems.SelectedItem != null)
                {
                    device = lvwItems.SelectedItem as IDevice;
                    connected2Device = await btManager.Connect(device, adapter);

                if (connected2Device)
                {
                    CacheProvidor.Set<IDevice>("device", device, DateTime.Now.AddDays(1));
                    characteristic = await btManager.GetCharasteric(device);
                    CacheProvidor.Set<ICharacteristic>("characteristic", characteristic, DateTime.Now.AddDays(1));

                        await Navigation.PushModalAsync(new NavigationPage(new MasterPage()));
                    }
                    else
                    {
                        await DisplayAlert("Foutmelding", "Verbinding maken is mislukt. We scannen opnieuw voor u.", "Ok");
                        StartUp();
                    }
                }
                lvwItems.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await StepOutManager.Writelog(ex);
                await DisplayAlert("Fout", "Er is iets misgelopen bij het connecteren met een bluetooth device, als dit blijft voorkomen gelieve contact op te nemen met de suport.", "Ok");
            }
        }

        #endregion
    }
}