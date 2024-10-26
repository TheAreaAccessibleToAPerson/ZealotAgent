using System.Net.Sockets;
using System.Text;
using Butterfly;

namespace Zealot.client.connection._SSLShield.content
{
    public class Setting
    {
        public string Address { set; get; }
        public uint Port { set; get; }

        /// <summary>
        /// Все входящие сообщения нужно перенаправить в данный метод.
        /// </summary>
        public Action<int, byte[]> ReceiveMessage { set; get; }

        /// <summary>
        /// Все входящие сообщения нужно перенаправить в данный метод.
        /// </summary>
        public Client.IEndInitialize.SSLConnection Connection { set; get; }
    }

    public abstract class Controller : Butterfly.Controller.Board.LocalField<Setting>
    {
        public const string NAME = "Shield" + SSLShield.NAME;

        protected Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        protected bool IsRunning = true;

        protected IInput<SSLShield.IConnection> I_toConnection;
        protected IInput I_toDisconnection;

        protected IInput<byte[]> I_sendBytes;
        protected IInput<string> I_sendString;

        private Action<int, byte[]> _clientReceive;

        public void Send(string message)
        {
            if (IsRunning) I_sendString.To(message);
        }

        public void Send(byte[] message)
        {
            if (IsRunning) I_sendBytes.To(message);
        }

        public void Sending(string message)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message);

                Socket.Send(bytes);
            }
            catch (SocketException socketExcetion)
            {
                Logger.I.To(this, socketExcetion.ToString());

                destroy();

                return;
            }
            catch (Exception ex)
            {
                Logger.S_I.To(this, ex.ToString());

                destroy();

                return;
            }
        }

        public void Sending(byte[] message)
        {
            try
            {
                Logger.I.To(this, $"send message length:{message.Length}");

                Socket.Send(message);
            }
            catch (SocketException socketExcetion)
            {
                Logger.I.To(this, socketExcetion.ToString());

                destroy();

                return;
            }
            catch (Exception ex)
            {
                Logger.S_I.To(this, ex.ToString());

                destroy();

                return;
            }
        }

        protected void ReceiveMessage()
        {
            if (IsRunning)
            {
                try
                {
                    if (Socket.Available > 0)
                    {
                        byte[] buffer = new byte[65536];
                        int length = Socket.Receive(buffer);

                        Logger.I.To(this, $"Поступило новое сообщение длиной {length}");

                        _clientReceive(length, buffer);
                    }
                }
                catch (SocketException socketException)
                {
                    Logger.S_I.To(this, socketException.ToString());

                    destroy();

                    return;
                }
                catch (Exception ex)
                {
                    Logger.S_I.To(this, ex.ToString());

                    destroy();

                    return;
                }
            }
        }

        protected bool Setting()
        {
            Logger.S_I.To(this, "start setting ...");
            {
                if (Field.ReceiveMessage != null)
                {
                    Logger.S_I.To(this, $"client receive message initialize.");

                    _clientReceive = Field.ReceiveMessage;
                }
                else
                {
                    Logger.S_E.To(this, $"client receive message don't initialize(field value is null)");

                    destroy();

                    return false;
                }
            }
            Logger.S_I.To(this, "end setting.");

            return true;
        }

        protected bool Connection()
        {
            try
            {
                Logger.S_I.To(this, $"connection ...");
                SystemInformation("connection ...");
                Socket.Connect(Field.Address, (int)Field.Port);
                Logger.S_I.To(this, $"connect.");
                SystemInformation("connect.");
            }
            catch (Exception ex)
            {
                Logger.S_W.To(this, ex.ToString());
                SystemInformation($"don't connection to server {Field.Address}:{Field.Port}", ConsoleColor.Red);

                destroy();

                return false;
            }

            return true;
        }

        protected bool Disconnect()
        {
            Logger.S_I.To(this, $"Приступаем к разрыву соединения с сервером");

            Logger.S_I.To(this, $"Отключаем обьекту возможность передачи и приема через него данных.");
            IsRunning = false;

            try
            {
                Logger.S_I.To(this, $"Отключаемся от сервера.");
                Socket.Disconnect(true);
            }
            catch (Exception ex)
            {
                Logger.S_I.To(this, $"Неудалось произвести отключение от сервера - {ex}");

                return false;
            }

            try
            {
                Logger.S_I.To(this, $"Отключаем сокету возможность передачи и приема через него данных.");
                Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Logger.S_I.To(this, $"Неудалось отключить сокету возможность передачи и приема через него данных - {ex}");

                return false;
            }

            try
            {
                Logger.S_I.To(this, $"Отчищаем сокет.");
                Socket.Close();
            }
            catch (Exception ex)
            {
                Logger.S_I.To(this, $"Неудалось произвести отчистку сокета - {ex}");

                return false;
            }

            Logger.S_I.To(this, $"Отчищаем ссылку на сокет.");
            Socket = null;

            Logger.S_I.To(this, $"Соединение с сервером разорвано.");
            return true;
        }
    }
}