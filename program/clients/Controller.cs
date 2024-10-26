using Zealot.client.connection;

namespace Zealot.client
{
    public abstract class Controller : Butterfly.Controller, 
        State.IClient, Connection.IClient,
        Client.IEndInitialize.SSLConnection, Client.IEndInitialize.TCPConnection
    {
        public readonly State State;
        protected readonly Connection Connection;
        private readonly Data _data = new();

        public Controller() : base()
        {
            Connection = new(this);
            State = new(this, this, this, Connection, Connection);
        }

        /// <summary>
        /// SSL Соединение с сервером устрановлено.
        /// value - способ передачи данных на сервер.
        /// </summary>
        void Client.IEndInitialize.SSLConnection.EndInitialize(SSLShield.IConnection value)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, $"ssl shield end initilize");

                Connection.SetSSLConnection(value);

                invoke_event(State.Change, Event.SYSTEM);
            }))
            {
                Logger.S_I.To(this, $"SSLShield.IReceive.EndInitialize call");
            }
            else Logger.S_I.To(this, $"SSLShield.IReceive.EndInitialize don't call");
        }

        void State.IClient.SendAuthorizationData()
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, $"Передаем данные для авторизации на сервер.");

                if (_data.TryGetLogin(out string login) && _data.TryGetPassword(out string password))
                {
                    Connection.SendSSLAuthorizationData(login, password);
                }
                else 
                {
                    Logger.S_I.To(this, $"Неудалось получить логин или пароль.");

                    destroy();

                    return;
                }
            }))
            {
                Logger.S_I.To(this, $"State.IClient call");
            }
            else Logger.S_I.To(this, $"State.IClient don't call");
        }

        /// <summary>
        /// TCP Соединение с сервером устрановлено.
        /// value - способ передачи данных на сервер.
        /// </summary>
        void Client.IEndInitialize.TCPConnection.EndInitialize(TCPShield.IConnection value)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, $"Соединение с сервереом в TCPShield установлено.");

                Connection.SetTCPConnection(value);

                invoke_event(State.Change, Event.SYSTEM);
            }))
            {
                Logger.S_I.To(this, $"TCPShield.IReceive.EndInitialize call");
            }
            else Logger.S_I.To(this, $"TCPShield.IReceive.EndInitialize don't call");
        }

        void State.IClient.LoadingData()
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, "loading data ...");

                if (_data.TryLoading(out string info))
                {
                    Logger.S_I.To(this, info);
                }
                else 
                {
                    Logger.S_I.To(this, info);

                    destroy();

                    return;
                }
            }))
            {
                Logger.S_I.To(this, "LoadingData call");
            }
            else Logger.S_I.To(this, "LoadingData don't call");
        }
    }
}