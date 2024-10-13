using Butterfly.system.objects.main;
using Zealot.client.connection;

namespace Zealot.client
{
    public interface IClient : IInformation
    {
        public void LoadingData();
        public void Destroy();
    }
}