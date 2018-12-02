using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Windows;
using Windows.Foundation;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

using BLEProgram;

namespace BLEProgram
{
    public partial class FrmMain : Form
    {
        Guid serviceUUID = NrfUuid.RX_SERVICE_UUID;
        Guid charUUID = NrfUuid.RX_CHAR_UUID;

        BluetoothLEAdvertisementWatcher bleWatcher = new BluetoothLEAdvertisementWatcher();

        ulong bleAddr;

        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            bleWatcher.Received += bleWatcher_Received;
        }

        private  void bleWatcher_Received(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            var serviceUUIDs = eventArgs.Advertisement.ServiceUuids;
            int index = -1;
            if(serviceUUIDs.IndexOf(serviceUUID) == index)
            {
                string strAdd = eventArgs.BluetoothAddress.ToString();

                requestList.Items.Add("Target device : " + strAdd);

                bleWatcher.Stop();

                bleAddr = eventArgs.BluetoothAddress;

                ConnectBluetoothDevice(eventArgs.BluetoothAddress);
            }
        }

        private async Task SendData(string reqData)
        {
            var leDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(bleAddr);

            var serviceRes = await leDevice.GetGattServicesForUuidAsync(serviceUUID);
            var service = serviceRes.Services[0];

            var charRes = await service.GetCharacteristicsForUuidAsync(charUUID);
            var chars = charRes.Characteristics[0];

            var writer = new DataWriter();
            var sendData = StrToByteArray(reqData);

            writer.WriteBytes(sendData);

            var stat = await chars.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
            requestList.Items.Add("Send : " + reqData);
        }

        private async void ConnectBluetoothDevice(ulong bluetoothAddr)
        {
            var leDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddr);
            Console.WriteLine("Waiting...");

            GattDeviceServicesResult result = await leDevice.GetGattServicesAsync();
            if (result.Status == GattCommunicationStatus.Success)
            {
                var services = result.Services;
                Console.WriteLine("test conn");
            }

            /*var serviceRes = await leDevice.GetGattServicesForUuidAsync(serviceUUID);

            Console.WriteLine("ServiceParamSending...");

            var service = serviceRes.Services[0];

            var charRes = await service.GetCharacteristicsForUuidAsync(charUUID);
            var chars = charRes.Characteristics[0];

            requestList.Items.Add("Connected!");*/
        }

        private byte[] StrToByteArray(string data)
        {
            List<byte> bl = new List<byte>();
            char[] convertTarget = data.ToCharArray();
            foreach(char c in convertTarget)
            {
                byte vb = Convert.ToByte(c);
                bl.Add(vb);
            }
            return bl.ToArray();
        }

        private void ExitToolStrip_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you exit now?", "BLEProgram", MessageBoxButtons.YesNo);
            switch (result)
            {
                case DialogResult.Yes:
                    Application.Exit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            bleWatcher.ScanningMode = BluetoothLEScanningMode.Active;

            bleWatcher.Start();

        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            await SendData(DataText.Text);
        }
    }
}
