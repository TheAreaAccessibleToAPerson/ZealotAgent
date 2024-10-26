using System.Text;
using System.Text.Json;
using Butterfly;
using Butterfly.system.objects.main;
using Zealot.client.connection;
using Zealot.client.ssl.write;

namespace Zealot.client
{
    public sealed class Connection : connection.Controller, SSLShield.IReceive, TCPShield.IReceive
    {
        /// <summary>
        /// Переключает состяние в State.
        /// </summary>
        public IInput I_creatingTCPConnection;

        public Connection(IClient client) : base(client) { }

        // Сюда будет поступать SSL сообщения.
        void SSLShield.IReceive.Receive(int length, byte[] buffer)
        {
            Logger.I.To(_client, $"{NAME}:Receive message length {length}");

            if (length < 7) return;

            int index = 0;
            int currentStep = 0; int maxStepCount = 1000;
            while (true)
            {
                if (currentStep++ > maxStepCount)
                {
                    Logger.W.To(_client, $"{NAME}:Превышено одновеменое количесво SSL сообщений принятых из сети.");

                    return;
                }

                int headerLength = buffer[index + 0];
                if (headerLength == 7)
                {
                    int type = buffer[index + 1] << 8;
                    type += buffer[index + 2];

                    int messageLength = buffer[index + 3] << 24;
                    messageLength += buffer[index + 4] << 16;
                    messageLength += buffer[index + 5] << 8;
                    messageLength += buffer[index + 6];

                    if (index + 7 + messageLength > length)
                    {
                        Logger.S_W.To(_client, $"{NAME}:Привышена длина сообщения.[Length:{messageLength}]");

                        _client.destroy();

                        return;
                    }

                    // Поступил запрос, нужно установить tcp соединение и 
                    // передать поступивший ключ.
                    if (type == ssl.read.Type.REQUEST_TCP_CONNECTOIN)
                    {
                        Logger.I.To(_client, $"{NAME}:Message:RequestTCPConnection");

                        string str = Encoding.UTF8.GetString(buffer, index + 7, messageLength);
                        RequestTCPConnection j = JsonSerializer.Deserialize<RequestTCPConnection>(str);

                        TcpKey = j.Key;

                        Logger.S_I.To(_client, $"{NAME}:CreatingTCPConnection call");

                        I_creatingTCPConnection.To();
                    }
                    else if (type == ssl.read.Type.CONNECTION)
                    {
                        Logger.I.To(_client, $"Соединение с сервером установлено.");

                        _client.SystemInformation("CONNECT", ConsoleColor.Green);
                    }
                    else if (type == ssl.read.Type.DISCONNECTION)
                    {
                        Logger.I.To(_client, $"Сервер разорвал соединение.");

                        _client.SystemInformation("DISCONNECT", ConsoleColor.Red);

                        _client.destroy();
                    }
                    else
                    {
                        Logger.S_E.To(_client, $"{NAME}:Неизвестный тип сообщения.");

                        _client.destroy();

                        return;
                    }

                    index += headerLength + messageLength;
                    if (index >= length) return;
                }
                else
                {
                    Logger.S_W.To(_client, $"{NAME}:Пришло сообщение в котором длина заголовка меньше минимально возможной.");

                    _client.destroy();

                    return;
                }
            }
        }

        // Сюда будут поступать TCP сообщения.
        void TCPShield.IReceive.Receive(int length, byte[] message)
        {
        }

        public interface IClient : IInformation
        {
            public void SystemInformation(string message, ConsoleColor color);
        }
    }
}