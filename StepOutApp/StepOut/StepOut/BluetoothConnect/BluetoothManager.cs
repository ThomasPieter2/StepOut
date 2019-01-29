using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;

namespace StepOut.BluetoothConnect
{
    public class BluetoothManager
    {
        bool devicesFound = false;

        public async Task<List<IDevice>> Scan(IAdapter adapter)
        {
            List<IDevice> deviceList = new List<IDevice>();
            adapter.ScanTimeout = 5000;
            try
            {
                adapter.DeviceDiscovered += (o, a) =>
                {
                    if (a.Device.Name != null)
                    {
                        deviceList.Add(a.Device);
                    }
                };
                await adapter.StartScanningForDevicesAsync();
            }
            catch (DeviceConnectionException ex)
            {
                throw ex;
            }
            //adapter.ScanTimeoutElapsed += delegate
            //{
            //    devicesFound = true;
            //};

            if (deviceList != null) devicesFound = true;

            deviceList.Concat(adapter.GetSystemConnectedOrPairedDevices());
            if (devicesFound) return deviceList;
            
            else return null;
        }

        public async Task<bool> Connect(IDevice device, IAdapter adapter)
        {
            await adapter.StopScanningForDevicesAsync();
            try
            {
                await adapter.ConnectToDeviceAsync(device);
                return true;
            }
            catch (DeviceConnectionException ex)
            {
                Debug.WriteLine(ex);
                return false;
                //throw ex;
            }
        }

        public async Task Disconnect(IDevice device, IAdapter adapter)
        {
            await adapter.DisconnectDeviceAsync(device);
            Debug.WriteLine("Succesfull");
        }

        public async Task<ICharacteristic> GetCharasteric(IDevice device)
        {
            try
            {
                var services = await device.GetServicesAsync();
                Guid guid = new Guid();
                foreach (var ser in services)
                {
                    if (ser.Name == "Heart Rate") guid = ser.Id; Debug.WriteLine(guid);
                    Debug.WriteLine(ser);
                }
                var service = await device.GetServiceAsync(guid);

                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("00002a37-0000-1000-8000-00805f9b34fb"));
                await characteristic.StartUpdatesAsync();
                return characteristic;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> ReadHeartRate(ICharacteristic characteristic)
        {
            try
            {
                var bytes = characteristic.Value;
                int hr = bytes[1];
                Debug.WriteLine("{0}: {1} bpm", DateTime.Now, bytes[1]);
                return hr;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
