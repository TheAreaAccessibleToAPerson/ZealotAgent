using Butterfly;
using Zealot.client.connection;

namespace Zealot
{
    public sealed class Client : client.ZController
    {
        public const string NAME = "client";

        void Construction()
        {
            Logger.S_I.To(this, "start construction ...");
            {
                obj<TCP>(TCP.NAME);
            }
            Logger.S_I.To(this, "end construction.");
        }

        void Start()
        {
            Logger.S_I.To(this, "starting ...");
            {
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
    }
}