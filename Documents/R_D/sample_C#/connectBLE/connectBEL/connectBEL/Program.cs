using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace connectBEL
{
    class Program
    {
        static void Main(string[] args)
        {

            Regex regexPortName = new Regex(@"(COM\d+)");
            ManagementObjectSearcher searchSerial = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");  // デバイスマネージャーから情報を取得するためのオブジェクト

            // デバイスマネージャーの情報を列挙する
            foreach (ManagementObject obj in searchSerial.Get())
            {
                string name = obj["Name"] as string; // デバイスマネージャーに表示されている機器名
                string classGuid = obj["ClassGuid"] as string; // GUID
                string devicePass = obj["DeviceID"] as string; // デバイスインスタンスパス

                if (classGuid != null && devicePass != null)
                {
                    // デバイスインスタンスパスからBluetooth接続機器のみを抽出
                    // {4d36e978-e325-11ce-bfc1-08002be10318}はBluetooth接続機器を示す固定値
                    if (String.Equals(classGuid, "{4d36e978-e325-11ce-bfc1-08002be10318}",
              StringComparison.InvariantCulture))
                    {
                        // デバイスインスタンスパスからデバイスIDを2段階で抜き出す
                        string[] tokens = devicePass.Split('&');
                        string[] addressToken = tokens[4].Split('_');

                        string bluetoothAddress = addressToken[0];
                        Match m = regexPortName.Match(name);
                        string comPortNumber = "";
                        if (m.Success)
                        {
                            // COM番号を抜き出す
                            comPortNumber = m.Groups[1].ToString();
                        }

                        if (Convert.ToUInt64(bluetoothAddress, 16) > 0)
                        {
                            string bluetoothName = GetBluetoothRegistryName(bluetoothAddress);
                        }
                        // bluetoothNameが接続機器名
                        // comPortNumberが接続機器名のCOM番号
                    }
                }
            }

        }
    }
}
