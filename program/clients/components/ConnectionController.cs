using System.Net.Security;
using System.Text;
using Butterfly;

namespace Zealot.client.connection
{
    public abstract class Controller
    {
        public const string NAME = "ClientConnection";

        /// <summary>
        /// Ключ который необходим для установления tcp соединения.
        /// </summary> <summary>
        protected string TcpKey { set; get; }

        protected readonly Connection.IClient _client;

        private SSLShield.IConnection _sslConnection;
        private TCPShield.IConnection _tcpConnection;

        public Controller(Connection.IClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Передает данные для авторизации. 
        /// </summary>
        public void SendSSLAuthorizationData(string login, string password)
        {
            Logger.S_I.To(_client, $"send authorization data");

            if (_sslConnection != null)
            {
                _sslConnection.Send(GetHeader(ssl.write.Type.AUTHORIZATION,
                    System.Text.Json.JsonSerializer.Serialize(new ssl.write.ClientAuthorization()
                    {
                        Login = login,
                        Password = password
                    }
                )));
            }
            else
            {
                Logger.S_E.To(_client, $"Неудалось передать данные авторизации на сервер. Ссылка на _sslConnection равна null.");

                _client.destroy();

                return;
            }
        }

        /// <summary>
        /// Передаем ключ что бы сервер распознал tcp подключение.
        /// </summary>
        public void SendTCPKey()
        {
            if (TcpKey == null)
            {
                Logger.S_E.To(_client, $"Неудалось передать ключ по tcp так как он небыл присвоен/получен.");

                _client.destroy();

                return;
            }
            else
            {
                Logger.S_I.To(_client, $"send tcp_key");

                _tcpConnection.Send(GetHeader(tcp.write.Type.KEY,
                    System.Text.Json.JsonSerializer.Serialize(new tcp.write.Key()
                    {
                        Value = TcpKey
                    }
                )));
            }
        }


        private byte[] GetHeader(int type, string str)
        {
            byte[] buffer = new byte[MessageHeader.LENGHT + str.Length];

            int length = Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, MessageHeader.LENGHT);
            {
                buffer[MessageHeader.LENGTH_BYTE_INDEX] = MessageHeader.LENGHT;
                buffer[MessageHeader.TYPE_1BYTE_INDEX] = (byte)(type >> 8);
                buffer[MessageHeader.TYPE_2BYTE_INDEX] = (byte)type;
                buffer[MessageHeader.MESSAGE_LENGTH_1BYTE_INDEX] = (byte)(length >> 24);
                buffer[MessageHeader.MESSAGE_LENGTH_2BYTE_INDEX] = (byte)(length >> 16);
                buffer[MessageHeader.MESSAGE_LENGTH_3BYTE_INDEX] = (byte)(length >> 8);
                buffer[MessageHeader.MESSAGE_LENGTH_4BYTE_INDEX] = (byte)length;
            }

            return buffer;
        }

        public void SetSSLConnection(SSLShield.IConnection value)
        {
            if (_sslConnection != null)
            {
                Logger.S_I.To(_client, $"Переназначена ссылка на SSL соединение.");
            }
            else Logger.S_I.To(_client, $"Получена ссылка на SSL соединение.");

            _sslConnection = value;
        }

        public void SetTCPConnection(TCPShield.IConnection value)
        {
            if (_sslConnection != null)
            {
                Logger.S_I.To(_client, $"Переназначена ссылка на TCP соединение.");
            }
            else Logger.S_I.To(_client, $"Получена ссылка на TCP соединение.");

            _tcpConnection = value;
        }
    }
}