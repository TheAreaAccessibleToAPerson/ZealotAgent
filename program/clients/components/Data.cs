namespace Zealot.client
{
    public class Data 
    {
        public const string NAME = Client.NAME + _.s + "Data";

        private string _login;
        private string _password;

        public bool TryGetLogin(out string login)
        {
            login = _login;

            return _login != null;
        }

        public bool TryGetPassword(out string password)
        {
            password = _password;

            return _password != null;
        }

        /// <summary>
        /// Считать данные.
        /// </summary>
        public bool TryLoading(out string info)
        {
            info = $"{NAME}:Загрузка логина и пороля прошла успешно!";

            _login = "login";
            _password = "password";

            return true;
        }
    }
}