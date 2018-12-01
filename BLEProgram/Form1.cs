﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Windows;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace BLEProgram
{
    public partial class Form1 : Form
    {
        Guid serviceUUID = BluetoothUuidHelper.FromShortId(0xffe5);
        Guid charUUID = BluetoothUuidHelper.FromShortId(0xffe9);
        BluetoothLEAdvertisementWatcher bleWatcher = new BluetoothLEAdvertisementWatcher();

        public Form1()
        {
            InitializeComponent();
            bleWatcher.ScanningMode = BluetoothLEScanningMode.Active;
            bleWatcher.Received += bleWatcher_Received;

            bleWatcher.Start();

        }

        private void bleWatcher_Received(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            var serviceUUIDs = eventArgs.Advertisement.ServiceUuids;
            int index = -1;
            if(serviceUUIDs.IndexOf(serviceUUID) == index)
            {
                string strAdd = eventArgs.BluetoothAddress.ToString();

                requestList.Items.Add("Target device : " + strAdd);

                bleWatcher.Stop();

                ConnectBluetoothDevice(eventArgs.BluetoothAddress);
            }
        }

        private async Task SendData(GattCharacteristic gattChar, string reqData)
        {
            var writer = new DataWriter();
            var sendData = StrToByteArray(reqData);

            writer.WriteBytes(sendData);

            var stat = await gattChar.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
            requestList.Items.Add("Send : " + reqData);
        }

        private async void ConnectBluetoothDevice(ulong bluetoothAddr)
        {
            var leDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddr);
            var serviceRes = await leDevice.GetGattServicesForUuidAsync(serviceUUID);
            var service = serviceRes.Services[0];
            var charRes = await service.GetCharacteristicsForUuidAsync(charUUID);
            var chars = charRes.Characteristics[0];

            await SendData(chars, dataText.Text);
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

        private void exitToolStrip_Click(object sender, EventArgs e)
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
    }
}
