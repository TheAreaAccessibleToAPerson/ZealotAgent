namespace Zealot.client.ssl.write
{
    public class RequestTCPConnection
    {
        /// <summary>
        /// Ключ который должен придти первым сообщением по TCP соединению от клиента.
        /// </summary>
        public string Key { set; get; }
    }
}