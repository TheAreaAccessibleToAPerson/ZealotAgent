using Butterfly;

namespace Zealot.client.connection._SSLShield
{
    public abstract class Controller : Butterfly.Controller
    {
        public const string NAME = "SSLConnection";

        private content.Setting _contentSetting;

        private string RemoveAddress;
        private uint RemovePort;

        /// <summary>
        /// Данный метод запускает клиент для которого создается текущее tcp подключение 
        /// к серверу.
        /// </summary>
        protected void Initialize(Client.IEndInitialize.SSLConnection connection, SSLShield.IReceive receive)
        {
            Logger.S_I.To(this, $"Был получен интерфейс для получения входящих tcp сообщений." +
                $"В ответ ожидает интерфейс для передачи данных на сервер.");

            _contentSetting.ReceiveMessage = receive.Receive;
            _contentSetting.Connection = connection;

            CreatingShield();
        }

        /// <summary>
        /// Соединение с сервером установлено.
        /// </summary> 
        protected void SuccessConnection(SSLShield.IConnection connection)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, "success connection to server");

                SystemInformation("success connection to server");

                if (_contentSetting.Connection != null)
                {
                    Logger.S_I.To(this, $"Оповестим клиента об окончании инициализации tcp подключения.");

                    _contentSetting.Connection.EndInitialize(connection);
                }
                else
                {
                    Logger.S_E.To(this, $"К моменту когда в оболочку придет подтверждение подключения к серверу поле Field.Connection" +
                        "уже должно быть проинициализировано");

                    destroy();

                    return;
                }
            }))
            {
                Logger.S_I.To(this, "SuccessConnection call");
            }
            else Logger.S_I.To(this, "SuccessConnection don't call");
        }

        /// <summary>
        /// Ошибка соединения с сервером.
        /// </summary> 
        protected void ErrorConnection(IImpulsInformation info)
        {
            if (try_fly(() =>
            {
                Logger.S_I.To(this, "ErrorConnection server");

                SystemInformation("ErrorConnection server");

                invoke_event(() =>
                {
                    try_fly(CreatingShield);
                },
                Event.SYSTEM_1000_TIME_STEP);
            }))
            {
                Logger.S_I.To(this, "ErrorConnection call");
            }
            else Logger.S_I.To(this, "ErrorConnection don't call");
        }

        void CreatingShield()
        {
            if (try_fly(() => { obj<Content>(Content.NAME, _contentSetting); }))
            {
                Logger.S_I.To(this, $"creating object {Content.NAME}");
            }
            else Logger.S_W.To(this, $"don't creating object {Content.NAME}");
        }

        protected bool TryGetServerAddress()
        {
            if (Data.TryGetServerSSLAddress(out string addr, out uint port, out string info))
            {
                Logger.S_I.To(this, info);

                SystemInformation(info);

                RemoveAddress = addr;
                RemovePort = port;

                _contentSetting = new content.Setting()
                {
                    Address = addr,
                    Port = port
                };

                return true;
            }
            else
            {
                SystemInformation(info, ConsoleColor.Red);

                Logger.S_W.To(this, info);

                destroy();

                return false;
            }
        }

        private sealed class Content : content.Controller, SSLShield.IConnection
        {
            void Construction()
            {
                Logger.S_I.To(this, "start construction ...");
                {
                    send_message(ref I_toConnection, SSLShield.BUS.Content.Message.SUCCESS_CONNECTION);
                    send_impuls(ref I_toDisconnection, SSLShield.BUS.Content.Impuls.ERROR_CONNECTION);
                }
                Logger.S_I.To(this, "end construction.");
            }

            void Start()
            {
                SystemInformation("start");

                Logger.S_I.To(this, "starting ...");
                {
                    Logger.S_I.To(this, "send message: connection to server.");

                    I_toConnection.To(this);
                }
                Logger.S_I.To(this, "start.");
            }

            void Destruction()
            {
                Logger.S_I.To(this, "start destruction ...");
                {
                    Logger.S_I.To(this, "send impuls: disconnect server.");

                    if (StateInformation.IsCallConstruction)
                    {
                        I_toDisconnection.To();
                    }
                    else Logger.S_I.To(this, "don't call construction. don't send impuls: disconnect server.");
                }
                Logger.S_I.To(this, "end destruction.");
            }

            void Configurate()
            {
                Logger.S_I.To(this, "start configurate ...");

                if (Setting())
                {
                    Logger.S_I.To(this, "setting success");

                    if (Connection())
                    {
                        Logger.S_I.To(this, "connection success");

                        Logger.S_I.To(this, "end configurate.");
                    }
                    else Logger.S_I.To(this, "configurate exception.");
                }
                else Logger.S_I.To(this, "configurate exception.");
            }

            void Destroyed()
            {
                Logger.S_I.To(this, "start destroyed ...");
                {
                    if (Disconnect())
                    {
                        Logger.S_I.To(this, "disconnect success");
                    }
                }
                Logger.S_I.To(this, "end destroyed.");
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