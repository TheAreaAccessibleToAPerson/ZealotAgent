using System.ComponentModel;

namespace Zealot
{
    public static class Component
    {
        public const string LOG_PATH = "log";

        public static void Initialize()
        {
            if (!Directory.Exists(LOG_PATH))
            {
                // Создаем папку под логер.
                Directory.CreateDirectory(LOG_PATH);
            }
        }
    }
}