using Butterfly;

namespace Zealot
{
    public struct _ 
    {
        public const string s = ":";
    }

    public sealed class Program : Controller, ReadLine.IInformation
    {
        public const string NAME = "program";

        void Construction()
        {
            Logger.S_I.To(this, "start construction ...");
            {
            }
            Logger.S_I.To(this, "end construction.");
        }

        void Start()
        {
            SystemInformation("start");

            Logger.S_I.To(this, "starting ...");
            {
                ReadLine.Start(this);

                obj<Client>(Client.NAME);
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
                if (StateInformation.IsCallStart)
                {
                    ReadLine.Stop(this);
                }
            }
            Logger.S_I.To(this, "stop");
        }

        public void Command(string command)
        {
            if (command == "exit")
            {
                destroy();
            }
            else ReadLine.Input();
        }
    }
}