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