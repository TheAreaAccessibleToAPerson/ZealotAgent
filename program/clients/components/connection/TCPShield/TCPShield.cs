namespace Zealot.client.connection
{
    public sealed class TCPShield : _TCPShield.Controller
    {
        void Construction()
        {
            Logger.S_I.To(this, "start construction ...");
            {
                listen_message<Client.IEndInitialize.TCPConnection, IReceive>(BUS.Client.Message.INITIALIZE)
                    .output_to(Initialize);

                listen_message<IConnection>(BUS.Content.Message.SUCCESS_CONNECTION)
                    .output_to(Connection);

                listen_impuls(BUS.Content.Impuls.DISCONNECT_SERVER)
                    .output_to(Disconnection);
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

            if (TryGetServerAddress())
            {
                Logger.S_I.To(this, "end configurate.");
            }
            else Logger.S_W.To(this, "configurate exception.");
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
                }
            }
            Logger.S_I.To(this, "stop");
        }


        public struct BUS
        {
            private const string _NAME = _.s + NAME;

            public struct Content
            {
                public struct Message
                {
                    /// <summary>
                    /// Сообщает что подключение прошло успешно. 
                    /// </summary>
                    public const string SUCCESS_CONNECTION = _NAME + "SuccessConnection";
                }

                public struct Impuls
                {
                    /// <summary>
                    /// Сообщает что соединение с сервером разорвано.
                    /// </summary>
                    public const string DISCONNECT_SERVER = _NAME + "ErrorConnection";
                }

                public struct Echo { }
            }

            public struct Client
            {
                public struct Message
                {
                    /// <summary>
                    /// Сообщение от клиента в котором он передает реализацию 
                    /// TCPShield.IReceive интерфейса и сообщает о начале создания
                    /// соединения c сервером. После того как произойдет соединение,
                    /// ожидается что через переданый клиентом интерфейс его оповестят
                    /// об окончания данной процедуры, а так же вернут ему способ 
                    /// передачи данных на сервер.
                    /// </summary>
                    public const string INITIALIZE = _NAME + "Initialize";
                }

                public struct Echo { }
            }
        }

        public interface IReceive
        {
            public void Receive(int length, byte[] message);
        }

        public interface IConnection
        {
            /// <summary>
            /// Принимает входящие сообщение типа строка. 
            /// </summary>
            public void Send(string message);

            /// <summary>
            /// Принимает входящие сообщение типа массив байтов.
            /// </summary>
            public void Send(byte[] message);

            /// <summary>
            /// Принимает входящие сообщение типа обьект json.
            /// </summary>
            public void Send<JsonType>(JsonType message);
        }

    }
}

