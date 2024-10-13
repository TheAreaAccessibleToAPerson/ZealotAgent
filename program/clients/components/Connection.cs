using Zealot.client.connection;

namespace Zealot.client
{
    public sealed class Connection : connection.Controller, SSLShield.IReceive , TCPShield.IReceive
    {
        public Connection(IClient client) : base(client) {}

        void SSLShield.IReceive.Receive(int length, byte[] message)
        {
        }

        void TCPShield.IReceive.Receive(int length, byte[] message)
        {
        }
    }
}