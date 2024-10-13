using Zealot.client.connection;

namespace Zealot.client
{
    public abstract class Controller : Butterfly.Controller, IClient, 
        Client.IEndInitialize.SSLConnection,
        Client.IEndInitialize.TCPConnection
    {
        public readonly State State;
        private readonly Connection _connection;

        public Controller() : base()
        {
            _connection = new(this);
            State = new(this, this, this, _connection, _connection);
        }


        void Client.IEndInitialize.SSLConnection.EndInitialize(SSLShield.IConnection value)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, $"ssl shield end initilize");

                _connection.SetSSLConnection(value);

                invoke_event(State.Change, Event.SYSTEM);
            }))
            {
                Logger.S_I.To(this, $"SSLShield.IReceive.EndInitialize call");
            }
            else Logger.S_I.To(this, $"SSLShield.IReceive.EndInitialize don't call");
        }

        void Client.IEndInitialize.TCPConnection.EndInitialize(TCPShield.IConnection value)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, $"tcp shield end initilize");

                _connection.SetTCPConnection(value);

                invoke_event(State.Change, Event.SYSTEM);
            }))
            {
                Logger.S_I.To(this, $"TCPShield.IReceive.EndInitialize call");
            }
            else Logger.S_I.To(this, $"TCPShield.IReceive.EndInitialize don't call");
        }

        void IClient.LoadingData()
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, "loading data ...");
            }))
            {
                Logger.S_I.To(this, "LoadingData call");
            }
            else Logger.S_I.To(this, "LoadingData don't call");

        }

        void IClient.Destroy()
        {
            destroy();
        }
    }
}