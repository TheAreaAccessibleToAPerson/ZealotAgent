namespace Zealot.client.tcp.write
{
    public struct Type 
    {
        /// <summary>
        /// Данное сообщение отправляет ключ.
        /// По которому на сервере определится для кокова клиента
        /// установлено данное соединение.
        /// </summary> <summary>
        public const int KEY = 0;
    }
}