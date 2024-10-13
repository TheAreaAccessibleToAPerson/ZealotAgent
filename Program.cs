namespace Butterfly
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            Butterfly.fly<Zealot.Header>(new Butterfly.Settings()
            {
                Name = "Program",

                SystemEvent = new EventSetting(Zealot.Event.SYSTEM, 5),

                EventsSetting = new EventSetting[]
                {
                    new EventSetting(Zealot.Event.LOGGER, 5),
                }
            });

            /*
            Butterfly.fly<Zealot.Header>(new Butterfly.Settings()
            {
                
                Name = "Program",

                SystemEvent = new EventSetting(Zealot.Header.Events.SYSTEM,
            Zealot.Header.Events.SYSTEM_TIME_DELAY),

                EventsSetting = new EventSetting[]
        {
                    new EventSetting(Zealot.Header.Events.LOGGER,
                        Zealot.Header.Events.LOGGER_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.MONGO_DB,
                        Zealot.Header.Events.MONGO_DB_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.WORK_DEVICE,
                        Zealot.Header.Events.WORK_DEVICE_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.CLIENT_WORK,
                        Zealot.Header.Events.LISTEN_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.SEND_MESSAGE_TO_CLIENT,
                        Zealot.Header.Events.SEND_MESSAGE_TO_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.RECEIVE_MESSAGE_FROM_CLIENT,
                        Zealot.Header.Events.RECEIVE_MESSAGE_FROM_CLIENT_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.SCAN_DEVICES,
                        Zealot.Header.Events.SCAN_DEVICES_TIME_DELAY, 20000),

                    new EventSetting(Zealot.Header.Events.REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.REQUEST_DEVICES_INFORMATION_TIME_DELAY),

                    new EventSetting(Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION,
                        Zealot.Header.Events.EXTRACT_FROM_RESULT_REQUEST_DEVICES_INFORMATION_TIME_DELAY)
        }
            });

        */
        }
    }

}