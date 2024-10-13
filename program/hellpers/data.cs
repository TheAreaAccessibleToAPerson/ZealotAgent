using System.Net;
using MongoDB.Driver.Linq;

namespace Zealot
{
    public static class Data
    {
        public static bool TryGetServerSSLAddress(out string address, out uint port, out string info)
        {
            address = ""; port = 0; info = "";

            string path = "data/SSLServerData.txt";

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string text = reader.ReadToEnd();
                    string[] lines = text.Split('\n');

                    if (lines.Length >= 2)
                    {
                        address = lines[0].Replace(" ", "");

                        if (IPAddress.TryParse(address, out IPAddress addr))
                        {
                            string p = lines[1].Replace(" ", "");

                            if (UInt32.TryParse(p, out port) && port <= 65536)
                            {
                                info = $"Из файла {path} был получен адрес и порт удаленого сервера для tcp соединения {address}:{port}";

                                return true;
                            }
                            else info = $"Во второй строке {path} не верный формат порта для tcp соединения $[{address}]";
                        }
                        else info = $"В первой строке {path} не верный формат адреса для tcp соединения $[{address}]";
                    }
                    else info = $"В {path} должно быть 2 строки в первой адрес севрера для tcp соединения, во второй порт.";
                }
            }
            catch (Exception ex)
            {
                info = ex.ToString();
            }

            return false;
        }

        public static bool TryGetServerTCPAddress(out string address, out uint port, out string info)
        {
            address = ""; port = 0; info = "";

            string path = "data/TCPServerData.txt";

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string text = reader.ReadToEnd();
                    string[] lines = text.Split('\n');

                    if (lines.Length >= 2)
                    {
                        address = lines[0].Replace(" ", "");

                        if (IPAddress.TryParse(address, out IPAddress addr))
                        {
                            string p = lines[1].Replace(" ", "");

                            if (UInt32.TryParse(p, out port) && port <= 65536)
                            {
                                info = $"Из файла {path} был получен адрес и порт удаленого сервера для tcp соединения {address}:{port}";

                                return true;
                            }
                            else info = $"Во второй строке {path} не верный формат порта для tcp соединения $[{address}]";
                        }
                        else info = $"В первой строке {path} не верный формат адреса для tcp соединения $[{address}]";
                    }
                    else info = $"В {path} должно быть 2 строки в первой адрес севрера для tcp соединения, во второй порт.";
                }
            }
            catch (Exception ex)
            {
                info = ex.ToString();
            }

            return false;
        }
    }
}