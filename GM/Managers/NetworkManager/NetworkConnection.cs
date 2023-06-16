using System.Net;

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    public abstract class NetworkConnection
    {
        private IPEndPoint _endPoint;
        private ConnectionType _connectionType;


        public NetworkConnection(IPEndPoint endPoint, ConnectionType connectionType)
        {
            _endPoint = endPoint;
            _connectionType = connectionType;
        }
        
        public abstract bool Initialize();
        public abstract void Send(byte[] data);
        public abstract byte[] Receive();
        public abstract void Close();
        // TODO Add any other needed methods (if any)
        
    }

    public enum ConnectionType
    {
        UDP,
        TCP
    }
}