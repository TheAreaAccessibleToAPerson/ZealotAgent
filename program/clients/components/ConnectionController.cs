using System.Net.Security;
using System.Text;

namespace Zealot.client.connection
{
    public abstract class Controller
    {
        public const string NAME = "ClientConnection";

        protected readonly IClient _client;

        private SSLShield.IConnection _sslConnection;
        private TCPShield.IConnection _tcpConnection;

        public Controller(IClient client)
        {
            _client = client;
        }

        public void SetSSLConnection(SSLShield.IConnection value)
        {
            Logger.S_I.To(_client, $"initialize sslConnection");

            _sslConnection = value;

            _sslConnection.Send(GetBuffer(System.Text.Json.JsonSerializer.Serialize(new ssl.write.ClientAuthorization()
            {
                Login = "login",
                Password = "password"
            }
            )));
        }

        private byte[] GetBuffer(string str)
        {
            byte[] buffer = new byte[MessageHeader.LENGHT + str.Length];

            int length = Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, MessageHeader.LENGHT);
            {
                buffer[MessageHeader.LENGTH_BYTE_INDEX] = MessageHeader.LENGHT;
                buffer[MessageHeader.TYPE_1BYTE_INDEX] = ssl.write.Type.AUTHORIZATION >> 8;
                buffer[MessageHeader.TYPE_2BYTE_INDEX] = ssl.write.Type.AUTHORIZATION;
                buffer[MessageHeader.MESSAGE_LENGTH_1BYTE_INDEX] = (byte)(length >> 24);
                buffer[MessageHeader.MESSAGE_LENGTH_2BYTE_INDEX] = (byte)(length >> 16);
                buffer[MessageHeader.MESSAGE_LENGTH_3BYTE_INDEX] = (byte)(length >> 8);
                buffer[MessageHeader.MESSAGE_LENGTH_4BYTE_INDEX] = (byte)length;
            }

            return buffer;
        }

        public void SetTCPConnection(TCPShield.IConnection value)
        {
            if (_tcpConnection != null)
            {
                Logger.S_I.To(_client, $"initialize tcpConnection");

                _tcpConnection = value;
            }
            else
            {
                Logger.S_E.To(_client, $"{NAME}:Попытка повторно присвоить значение tcpConnection");

                _client.Destroy();

                return;
            }
        }
    }
}