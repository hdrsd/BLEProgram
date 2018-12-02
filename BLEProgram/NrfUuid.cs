using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLEProgram
{
    public class NrfUuid
    {
        public static Guid TX_POWER_UUID = Guid.Parse("00001804-0000-1000-8000-00805f9b34fb");
        public static Guid TX_POWER_LEVEL_UUID = Guid.Parse("00002a07-0000-1000-8000-00805f9b34fb");
        public static Guid CCCD = Guid.Parse("00002902-0000-1000-8000-00805f9b34fb");
        public static Guid FIRMWARE_REVISON_UUID = Guid.Parse("00002a26-0000-1000-8000-00805f9b34fb");
        public static Guid DIS_UUID = Guid.Parse("0000180a-0000-1000-8000-00805f9b34fb");
        public static Guid RX_SERVICE_UUID = Guid.Parse("6e400001-b5a3-f393-e0a9-e50e24dcca9e");
        public static Guid RX_CHAR_UUID = Guid.Parse("6e400002-b5a3-f393-e0a9-e50e24dcca9e");
        public static Guid TX_CHAR_UUID = Guid.Parse("6e400003-b5a3-f393-e0a9-e50e24dcca9e");
    }
}
