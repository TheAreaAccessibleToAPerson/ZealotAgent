using Butterfly;
using Butterfly.system.objects.main;
using Zealot.client.connection;

namespace Zealot.client
{
    public sealed class State
    {
        public const string NAME = "State";

        public IInput<Client.IEndInitialize.SSLConnection, SSLShield.IReceive> I_initializeSSL;
        public IInput<Client.IEndInitialize.TCPConnection, TCPShield.IReceive> I_initializeTCP;
        public IInput I_sendTCPKey;

        public enum Type
        {
            None,

            /// <summary>
            /// Загружаем необходимые данные.
            /// </summary>
            LoadingData,

            /// <summary>
            /// Устанавливаем SSL соединение.
            /// </summary>
            ConnectionSSL,

            /// <summary>
            /// Осущесвляет авторизацию на сервере.
            /// </summary>
            Authorization,

            /// <summary>
            /// Устанавливаем TCP соединение.
            /// </summary>
            ConnectionTCP,

            /// <summary>
            /// После того как было устрановлено tcp соединение по нему первым делом нужно передать
            /// ключ, полученый ранее от сервера.
            /// </summary> <summary>
            SendTCPKey,
        }

        private readonly IClient _client;

        private readonly Client.IEndInitialize.SSLConnection _sslConnection;
        private readonly Client.IEndInitialize.TCPConnection _tcpConnection;

        /// <summary>
        /// Место куда будут поступать ssl сообщения.
        /// </summary>
        private readonly SSLShield.IReceive _sslReceive;

        /// <summary>
        /// Место куда будут поступать tcp сообщения.
        /// </summary>
        private readonly TCPShield.IReceive _tcpReceive;

        public State(IClient client, Client.IEndInitialize.SSLConnection sslConnection,
            Client.IEndInitialize.TCPConnection tcpConnection, SSLShield.IReceive sslReceive,
            TCPShield.IReceive tcpReceive)
        {
            _client = client;
            _sslConnection = sslConnection;
            _tcpConnection = tcpConnection;
            _sslReceive = sslReceive;
            _tcpReceive = tcpReceive;
        }

        public Type CurrentState { private set; get; } = Type.None;

        public void Change()
        {
            Logger.S_I.To(_client, $"{NAME}:change state. current state {CurrentState}");

            if (CurrentState == Type.None)
            {
                LoadingData();
            }
            else if (CurrentState == Type.LoadingData)
            {
                ConnectionSSL();
            }
            else if (CurrentState == Type.ConnectionSSL)
            {
                Authorization();
            }
            else if (CurrentState == Type.Authorization)
            {
                ConnectionTCP();
            }
            else if (CurrentState == Type.ConnectionTCP)
            {
                SendTcpKey();
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:extra call");

                _client.destroy();

                return;
            }
        }

        private void LoadingData()
        {
            if (CurrentState == Type.None)
            {
                Logger.S_I.To(_client, $"{NAME}:change of state {Type.None} -> {Type.LoadingData}");

                CurrentState = Type.LoadingData;

                _client.LoadingData();

                ConnectionSSL();
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:error changing state to {Type.LoadingData}. current state was expected {Type.None}");

                _client.destroy();

                return;
            }
        }

        /// <summary>
        /// Устанавлиет SSL соединение с сервером.
        /// </summary> <summary>
        private void ConnectionSSL()
        {
            if (CurrentState == Type.LoadingData)
            {
                Logger.S_I.To(_client, $"{NAME}:change of state {Type.LoadingData} -> {Type.ConnectionSSL}");

                CurrentState = Type.ConnectionSSL;

                I_initializeSSL.To(_sslConnection, _sslReceive);
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:error changing state to {Type.ConnectionSSL}. current state was expected {Type.LoadingData}");

                _client.destroy();

                return;
            }
        }

        /// <summary>
        /// Оправляет данные для авторизации на сервер.
        /// </summary> <summary>
        private void Authorization()
        {
            if (CurrentState == Type.ConnectionSSL)
            {
                Logger.S_I.To(_client, $"{NAME}:change of state {Type.ConnectionSSL} -> {Type.Authorization}");

                CurrentState = Type.Authorization;

                _client.SendAuthorizationData();
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:error changing state to {Type.Authorization}. current state was expected {Type.ConnectionSSL}");

                _client.destroy();

                return;
            }
        }

        private void ConnectionTCP()
        {
            if (CurrentState == Type.Authorization)
            {
                Logger.S_I.To(_client, $"{NAME}:change of state {Type.Authorization} -> {Type.ConnectionTCP}");

                CurrentState = Type.ConnectionTCP;

                if (I_initializeTCP != null)
                {
                    Logger.S_I.To(_client, $"{NAME}:initialize tcp");

                    I_initializeTCP.To(_tcpConnection, _tcpReceive);
                }
                else 
                {
                    Logger.S_E.To(_client, $"I_initializeTCP is null");

                    _client.destroy();

                    return;
                }
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:error changing state to {Type.ConnectionTCP}. current state was expected {Type.Authorization}");

                _client.destroy();

                return;
            }
        }

        /// <summary>
        /// Отправляем ключ, по tcp полученый ранее.
        /// </summary> <summary>
        private void SendTcpKey()
        {
            if (CurrentState == Type.ConnectionTCP)
            {
                Logger.S_I.To(_client, $"{NAME}:change of state {Type.ConnectionTCP} -> {Type.SendTCPKey}");

                CurrentState = Type.SendTCPKey;

                I_sendTCPKey.To();
            }
            else 
            {
                Logger.S_E.To(_client, $"{NAME}:error changing state to {Type.SendTCPKey}. current state was expected {Type.ConnectionTCP}");

                _client.destroy();

                return;
            }
        }

        public interface IClient : IInformation
        {
            public void LoadingData();

            /// <summary>
            /// Передать данные для авторизации на сервер.
            /// </summary>
            public void SendAuthorizationData();
        }
    }
}