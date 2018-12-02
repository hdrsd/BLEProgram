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
        Guid serviceUUID = BluetoothUuidHelper.FromShortId(0x180D);
        Guid charUUID = BluetoothUuidHelper.FromShortId(0x2A29);

        BluetoothLEAdvertisementWatcher bleWatcher = new BluetoothLEAdvertisementWatcher();

        ulong bleAddr;

        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            bleWatcher.Received += bleWatcher_Received;
        }

        private async void bleWatcher_Received(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // BluetoothDevice btDevice;
            if (bleWatcher.Status != BluetoothLEAdvertisementWatcherStatus.Started) return; //Can't run this is already stopped.

            bleWatcher.Stop();

            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);

            if (device.DeviceInformation.Pairing.IsPaired == false)
            {

                var handlerPairingRequested = new TypedEventHandler<DeviceInformationCustomPairing, DevicePairingRequestedEventArgs>(handlerPairingReq);
                device.DeviceInformation.Pairing.Custom.PairingRequested += handlerPairingRequested;
                var prslt = await device.DeviceInformation.Pairing.Custom.PairAsync(DevicePairingKinds.ProvidePin, DevicePairingProtectionLevel.None);
                device.DeviceInformation.Pairing.Custom.PairingRequested -= handlerPairingRequested; //Don't need it anymore once paired.
                System.Threading.Thread.Sleep(5000); //try 3 second delay.
                device.Dispose();
                device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
            }
            //Now Paired.  Lets get all Chars       


            var GatService = device.GetGattService(Guid.Parse("9804c436-45ed-50e2-b577-c43ccb17079d"));
            var GattChars = GatService.GetAllCharacteristics();
            var c1 = GattChars[0];
            var d1 = c1.GetAllDescriptors()[0];
            var c2 = GattChars[1];
            var d2 = c2.GetAllDescriptors()[0];
            byte[] datarslt1, datarslt2;

            Console.WriteLine("d1 total Descs[0] :" + c1.GetAllDescriptors().Count);

            Console.WriteLine("d2 total Descs[0] :" + c2.GetAllDescriptors().Count);

            //online examples state I should be able to write to the "Notify" Characterisitic which is C2.
            //I happen to know that C2 is the one I want so below I will be refering to it only.

            if (c2.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify))
            {
                //we have isolated the Notify Char.
                Console.WriteLine("Notify - Uuid: " + c2.Uuid);

                IAsyncOperation<GattReadClientCharacteristicConfigurationDescriptorResult> cs2Char;
                GattCommunicationStatus cs2;
                try
                {

                    cs2Char = c2.ReadClientCharacteristicConfigurationDescriptorAsync();
                    Console.WriteLine("Read Client Chars: " + cs2Char.GetResults().ClientCharacteristicConfigurationDescriptor.ToString());

                    cs2 = await c2.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                }
                catch (Exception eee)
                { Console.WriteLine("WriteClientChar Error:" + eee.Message); }

                //c2.ValueChanged += stData_ValueChanged; //will alert us when data changes.
                                                        //set the notify enable flag per examples


                GattCommunicationStatus WriteRslt = 0;
                DataWriter writer = new DataWriter();
                writer.WriteBytes(new byte[] { 0x01 });
                var buf = writer.DetachBuffer();
                try
                {
                    Console.WriteLine("Writting Bytes: ");
                    WriteRslt = await d2.WriteValueAsync(buf);
                    Console.WriteLine("Result(good): " + WriteRslt.ToString());
                }
                catch (Exception ee)
                { Console.WriteLine("Result(bad): " + WriteRslt.ToString() + "   Error:" + ee.Message); }
            }

            /*var serviceUUIDs = eventArgs.Advertisement.ServiceUuids;
            int index = -1;
            if(serviceUUIDs.IndexOf(serviceUUID) == index)
            {
                string strAdd = eventArgs.BluetoothAddress.ToString();

                requestList.Items.Add("Target device : " + strAdd);

                bleWatcher.Stop();

                bleAddr = eventArgs.BluetoothAddress;

                ConnectBluetoothDevice(bleAddr);
            }*/
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

            var serviceRes = await leDevice.GetGattServicesForUuidAsync(serviceUUID);
            var service = serviceRes.Services[0];

            var charRes = await service.GetCharacteristicsForUuidAsync(charUUID);
            var chars = charRes.Characteristics[0];

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
