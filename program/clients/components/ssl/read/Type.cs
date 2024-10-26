namespace Zealot.client.ssl.read
{
    public struct Type
    {
        /// <summary>
        /// Запрашивает у клиента TCP соединение.
        /// </summary>
        public const int REQUEST_TCP_CONNECTOIN = 0;

        /// <summary>
        /// Соединение установлено.
        /// </summary>
        public const int CONNECTION = 1;

        /// <summary>
        /// Соединение разорвано.
        /// </summary>
        public const int DISCONNECTION = 2;
    }
}