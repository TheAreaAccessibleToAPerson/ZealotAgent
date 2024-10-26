using System.Text;

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
                    new EventSetting(Zealot.Event.SSL_SEND, 5),
                    new EventSetting(Zealot.Event.SSL_RECEIVE, 5),
                    new EventSetting(Zealot.Event.TCP_SEND, 5),
                    new EventSetting(Zealot.Event.TCP_RECEIVE, 5),
                }
            });
        }
    }

}