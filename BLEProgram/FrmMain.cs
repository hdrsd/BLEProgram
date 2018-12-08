using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        //uuid들입니다.
        Guid serviceUUID = NrfUuid.RX_SERVICE_UUID;
        Guid charUUID = NrfUuid.RX_CHAR_UUID;
        Guid txUUID = NrfUuid.TX_CHAR_UUID;

        BluetoothLEAdvertisementWatcher bleWatcher = new BluetoothLEAdvertisementWatcher();
        BluetoothLEDevice bleDevice;

        GattDeviceServicesResult serviceRes;
        GattDeviceService gattService;

        GattCharacteristicsResult charRes;
        GattCharacteristic gattChar;

        GattCharacteristicsResult txRes;
        GattCharacteristic txChar;

        ulong bleAddr;

        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            bleWatcher.Received += bleWatcher_Received;
        }

        //bleWatcher가 Advertising중인 ble장치를 발견하면 통신을 시작합니다.
        private void bleWatcher_Received(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            var serviceUUIDs = eventArgs.Advertisement.ServiceUuids;

            int index = -1;

            //이벤트 발생으로 받아온 uuid값들중 index -1에 serviceUUID있는지 확인합니다.
            if(serviceUUIDs.IndexOf(serviceUUID) == index)
            {
                string strAdd = eventArgs.BluetoothAddress.ToString();
                requestList.Items.Add("Target device : " + strAdd);

                //장치를 발견했으니 Scan종료합니다.
                bleWatcher.Stop();

                bleAddr = eventArgs.BluetoothAddress;

                try
                {
                    ConnectBluetoothDevice(eventArgs.BluetoothAddress);
                }catch(Exception ex)
                {
                    MessageBox.Show("연결 실패 : " + ex.ToString());
                }
            }
        }

        private void ExitToolStrip_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("종료하시겠습니까?", "BLEProgram", MessageBoxButtons.YesNo);
            switch (result)
            {
                case DialogResult.Yes:
                    bleWatcher.Stop();
                    Application.Exit();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //FrmMain이 로드되면 bleWatcher의 스캔을 시작합니다.
            bleWatcher.ScanningMode = BluetoothLEScanningMode.Active;

            bleWatcher.Start();

        }

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            await SendData(DataText.Text);
        }

        //연결 이후 메서드입니다.
        private async void ConnectBluetoothDevice(ulong bluetoothAddr)
        {
            //bluetoothAddr장치와 싱크합니다.
            bleDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(bleAddr);

            //Gatt서비스를 싱크합니다.
            serviceRes = await bleDevice.GetGattServicesForUuidAsync(serviceUUID);
            gattService = serviceRes.Services[0];

            //RXChar를 싱크 한 뒤 Value를 가져옵니다. 따로 변경하지 않았으나 charRes는 Rx char입니다.
            charRes = await gattService.GetCharacteristicsForUuidAsync(charUUID);
            gattChar = charRes.Characteristics[0];

            //TxChar를 싱크 한 뒤 Value를 가져옵니다.
            txRes = await gattService.GetCharacteristicsForUuidAsync(txUUID);
            txChar = txRes.Characteristics[0];

            //txChar가 null이 아니므로 Event를 추가합니다.
            txChar.ValueChanged += txChar_ValueChanged;

            requestList.Items.Add("Connected!");
        }

        private async Task SendData(string reqData)
        {
            //입력 데이터를 Byte arr화 합니다.
            DataWriter writer = new DataWriter();
            byte[] sendData = StrToByteArray(reqData);

            writer.WriteBytes(sendData);

            //Byte arr화 한 데이터를 전송합니다.
            GattCommunicationStatus status = await gattChar.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
            requestList.Items.Add("Send : " + reqData);

            //tx에 들어오는 Val대기
            txRes = await gattService.GetCharacteristicsForUuidAsync(txUUID);
            txChar = txRes.Characteristics[0];

            //Notify대기
            GattCommunicationStatus gattStatus = await txChar.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

            //위에서 받아온 status가 success이면 콘솔에 성공을 따로 띄웁니다.(디버그용)
            if (gattStatus == GattCommunicationStatus.Success)
            {
                requestList.Items.Add("ResponseByNotify : " + reqData);
            }
        }

        private void txChar_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs eventArgs)
        {
            //txChar의 값이 변동되면 이 이벤트 실행
            requestList.Items.Add("Response : " + Encoding.ASCII.GetString(eventArgs.CharacteristicValue.ToArray()));
        }

        //String to byte Arr
        private byte[] StrToByteArray(string data)
        {
            List<byte> bl = new List<byte>();
            char[] convertTarget = data.ToCharArray();
            foreach (char c in convertTarget)
            {
                byte vb = Convert.ToByte(c);
                bl.Add(vb);
            }
            return bl.ToArray();
        }
    }
}
