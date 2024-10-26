namespace Zealot
{
    public struct Event
    {
        private const string NAME = "Event" + _.s;

        /// <summary>
        /// Системное событие.
        /// Изначально оно служит создание и уничтожение обьектов системы.
        /// Но ему можно делигировать выполнение и других задач.
        /// </summary> <summary>
        public const string SYSTEM = NAME + "System";

        /// <summary>
        /// Системное событие с отложеным вызовом минимум на 1 секунду.
        /// Изначально оно служит создание и уничтожение обьектов системы.
        /// Но ему можно делигировать выполнение и других задач.
        /// </summary> <summary>
        public const string SYSTEM_1000_TIME_STEP = SYSTEM + _.s + "1000 time step";

        /// <summary>
        /// Событие обрабатывающее логер.
        /// </summary> <summary>
        public const string LOGGER = NAME + _.s + "Logger";

        public const string SSL_SEND = NAME + _.s + "SSLSend";
        public const string SSL_RECEIVE = NAME + _.s + "SSLReceive";

        public const string TCP_SEND = NAME + _.s + "TCPSend";
        public const string TCP_RECEIVE = NAME + _.s + "TCPReceive";
    }
}