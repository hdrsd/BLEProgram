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
        BluetoothLEDevice bleDevice;

        GattDeviceServicesResult serviceRes;
        GattDeviceService gattService;

        GattCharacteristicsResult charRes;
        GattCharacteristic gattChar;

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
            DataWriter writer = new DataWriter();
            byte[] sendData = StrToByteArray(reqData);

            writer.WriteBytes(sendData);

            GattCommunicationStatus status = await gattChar.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
            requestList.Items.Add("Send : " + reqData);
        }

        private async void ConnectBluetoothDevice(ulong bluetoothAddr)
        {
            bleDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(bleAddr);

            serviceRes = await bleDevice.GetGattServicesForUuidAsync(serviceUUID);
            gattService = serviceRes.Services[0];

            charRes = await gattService.GetCharacteristicsForUuidAsync(charUUID);
            gattChar = charRes.Characteristics[0];

            requestList.Items.Add("Connected!");
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
