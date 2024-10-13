using Butterfly;
using Zealot.client.connection;

namespace Zealot
{
    public sealed class Client : client.Controller
    {
        public const string NAME = "client";

        void Construction()
        {
            Logger.S_I.To(this, "start construction ...");
            {
                obj<SSLShield>(SSLShield.NAME);
                obj<TCPShield>(TCPShield.NAME);
            }
            Logger.S_I.To(this, "end construction.");
        }

        void Start()
        {
            Logger.S_I.To(this, "starting ...");
            {
                send_message(ref State.I_initializeSSL, SSLShield.BUS.Client.Message.INITIALIZE);
                send_message(ref State.I_initializeTCP, TCPShield.BUS.Client.Message.INITIALIZE);

                invoke_event(State.Change, Event.SYSTEM);
            }
            Logger.S_I.To(this, "start.");
        }

        void Destruction()
        {
            Logger.S_I.To(this, "start destruction ...");
            {
            }
            Logger.S_I.To(this, "end destruction.");
        }

        void Configurate()
        {
            Logger.S_I.To(this, "start configurate ...");
            {
            }
            Logger.S_I.To(this, "end configurate.");
        }

        void Destroyed()
        {
            {
            }
            Logger.S_I.To(this, "destroyed.");
        }

        void Stop()
        {
            Logger.S_I.To(this, "stopping ...");
            {
            }
            Logger.S_I.To(this, "stop");
        }

        public interface IEndInitialize 
        {
            public interface SSLConnection
            {
                public void EndInitialize(SSLShield.IConnection value);
            }

            public interface TCPConnection 
            {
                public void EndInitialize(TCPShield.IConnection value);
            }
        }
    }
}