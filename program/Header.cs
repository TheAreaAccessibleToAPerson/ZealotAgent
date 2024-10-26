using Butterfly;

namespace Zealot
{
    /// <summary>
    /// Заголовочный класс в котором происходит настройка 
    /// основных компонентов. В дальнейшем от сюда произойдет запуск
    /// программы.
    /// </summary> <summary>
    public sealed class Header : Controller
    {
        private WritingText _writingText = new();

        void Construction()
        {
            SystemInformation("start construction ...");
            {
                listen_events(Event.SYSTEM, Event.SYSTEM);
                listen_events(Event.SYSTEM_1000_TIME_STEP, Event.SYSTEM, 1000);
                listen_events(Event.LOGGER, Event.LOGGER);
                listen_events(Event.SSL_SEND, Event.SSL_SEND);
                listen_events(Event.TCP_SEND, Event.TCP_SEND);

                input_to(ref Logger.S_I, Event.LOGGER, _writingText.SystemInformation);
                input_to(ref Logger.S_W, Event.LOGGER, _writingText.SystemWarning);
                input_to(ref Logger.S_E, Event.LOGGER, _writingText.SystemError);

                input_to(ref Logger.I, Event.LOGGER, _writingText.Information);
                input_to(ref Logger.W, Event.LOGGER, _writingText.Warning);
                input_to(ref Logger.E, Event.LOGGER, _writingText.Error);

                input_to(ref Logger.CommandStateException, Event.LOGGER, _writingText.StateException);
            }
            SystemInformation("end construction.");
        }

        void Start()
        {
            SystemInformation("starting ...");
            {
                SystemInformation($"wait starting event {Event.LOGGER} ...");
                {
                    invoke_event(() =>
                    {
                        SystemInformation($"Event {Event.LOGGER} start ...");
                        {
                            if (try_fly(() => { obj<Program>(Program.NAME); }))
                            {
                                Logger.S_I.To(this, $"success creating {Program.NAME} object.");
                            }
                            else Logger.S_W.To(this, $"failed to creating {Program.NAME} object.");
                        }
                    },
                    // Ожидаем запуск события отвечающего за логеры.
                    Event.LOGGER);
                }
            }
        }
    }
}