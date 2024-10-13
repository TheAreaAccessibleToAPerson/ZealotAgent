using System.Net.Sockets;
using Butterfly;

namespace Zealot.client.connection
{
    public sealed class TCP : Controller
    {
        public const string NAME = "TCPConnection";

        // Текущее количесво попыток переподключения.
        private uint _currentNumberOfAttemptsReconnection = 0;

        private readonly Setting _setting = new Setting();

        IConnection _connection { set; get; } = null;

        void Construction()
        {
            Logger.S_I.To(this, "start construction ...");
            {
                listen_echo<IConnection>(BUS.Echo.GET_CONNECTION)
                    .output_to((@return) => { @return.To(_connection); },
                        Event.SYSTEM);

                listen_message<IConnection>(BUS.Message.SUCCESS_CONNECTION)
                    .output_to((connection) =>
                    {
                        _connection = connection;

                        _currentNumberOfAttemptsReconnection = 0;

                        SystemInformation("success connection to server.");
                        Logger.I.To(this, "success connection to server.");
                    },
                    Event.SYSTEM);

                listen_impuls(BUS.Message.ERROR_CONNECTION)
                    .output_to((info) =>
                    {
                        SystemInformation("error connection to server.", ConsoleColor.Red);
                        Logger.I.To(this, "error connection to server.");

                        if (--_currentNumberOfAttemptsReconnection > 0)
                        {
                            SystemInformation($"current number of attempts reconnection:{_currentNumberOfAttemptsReconnection}", ConsoleColor.Yellow);

                            if (StateInformation.IsDestroy == false) Start();
                        }
                        else
                        {
                            SystemInformation($"current number of attempts reconnection:{_currentNumberOfAttemptsReconnection}", ConsoleColor.Red);

                            destroy();

                            return;
                        }
                    },
                    Event.SYSTEM);
            }
            Logger.S_I.To(this, "end construction.");
        }

        void Start()
        {
            if (try_fly(() => { obj<Shield>(Shield.NAME, _setting); }))
            {
                Logger.S_I.To(this, $"creating object {Shield.NAME}");
            }
            else Logger.S_W.To(this, $"don't creating object {Shield.NAME}");
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
                if (Data.TryGetServerTCPAddress(out string addr, out uint port, out string info))
                {
                    SystemInformation(info);

                    Logger.S_I.To(this, info);

                    _setting.Address = addr;
                    _setting.Port = port;

                    _setting.NumberOfAttemptsReconnection = 1000000;
                    _setting.ReconnectionTimeStep = 1000;

                    Logger.S_I.To(this, "end configurate.");
                }
                else
                {
                    SystemInformation(info, ConsoleColor.Red);

                    Logger.S_W.To(this, info);

                    destroy();

                    return;
                }
            }
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

        public class Setting
        {
            // Адрес сервера.
            public string Address { set; get; }

            // Порт сервера.
            public uint Port { set; get; }

            // Количесво попыток переподключения.
            public int NumberOfAttemptsReconnection { set; get; }

            // Промежуток времени(милисекунды) между переподключениями.
            public int ReconnectionTimeStep { set; get; }
        }

        public struct BUS
        {
            private const string _NAME = _.s + NAME;

            public struct Message
            {
                /// <summary>
                /// Сообщает что подключение прошло успешно. 
                /// </summary>
                public const string SUCCESS_CONNECTION = _NAME + "SuccessConnection";

                /// <summary>
                /// Сообщает что подключение не удалось.
                /// </summary>
                public const string ERROR_CONNECTION = _NAME + "ErrorConnection";
            }

            public struct Echo
            {
                /// <summary>
                /// Получаем tcp соединение. 
                /// </summary>
                public const string GET_CONNECTION = _NAME + "GetConnection";
            }
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

        private sealed class Shield : Board.LocalField<Setting>, IConnection
        {
            public const string NAME = "Shield" + TCP.NAME;

            private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            public void Send(string message)
            {
                if (isRunning)
                {
                }
            }

            public void Send(byte[] message)
            {
                if (isRunning)
                {
                    _socket.Send(message);
                }
            }

            public void Send<T>(T message)
            {
                if (isRunning)
                {

                }
            }

            private bool isRunning = false;

            IInput<IConnection> _toConnection;
            IInput _toDisconnection;

            void Construction()
            {
                Logger.S_I.To(this, "start construction ...");
                {
                    send_message(ref _toConnection, BUS.Message.SUCCESS_CONNECTION);
                    send_impuls(ref _toDisconnection, BUS.Message.ERROR_CONNECTION);
                }
                Logger.S_I.To(this, "end construction.");
            }

            void Start()
            {
                SystemInformation("start");

                Logger.S_I.To(this, "starting ...");
                {
                    isRunning = true;

                    _toConnection.To(this);
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
                    try
                    {
                        _socket.Connect(Field.Address, (int)Field.Port);

                        Logger.S_I.To(this, "end configurate.");
                    }
                    catch (Exception ex)
                    {
                        SystemInformation($"don't connection to server {Field.Address}:{Field.Port}", ConsoleColor.Red);

                        Logger.S_W.To(this, ex.ToString());

                        destroy();

                        return;
                    }
                }
            }

            void Destroyed()
            {
                {
                    try 
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        _socket = null;
                    }
                    catch {}

                    isRunning = false;

                    _toDisconnection.To();
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
}

