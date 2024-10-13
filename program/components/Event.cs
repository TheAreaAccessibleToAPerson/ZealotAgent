namespace Zealot
{
    public struct Event
    {
        private const string NAME = "Event" + ":";

        /// <summary>
        /// Системное событие.
        /// Изначально оно служит создание и уничтожение обьектов системы.
        /// Но ему можно делигировать выполнение и других задач.
        /// </summary> <summary>
        public const string SYSTEM = NAME + "System";

        /// <summary>
        /// Событие обрабатывающее логер.
        /// </summary> <summary>
        public const string LOGGER = NAME + "Logger";
    }
}