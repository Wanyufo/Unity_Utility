using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    public abstract class NetworkConnection
    {
        private IPEndPoint _endPoint;
        public bool IsOpen { get; protected set; }


        public NetworkConnection(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }


        public bool Initialize()
        {
            // doing this is a version of the template Method Pattern
            bool internalSuccess = ConcreteInitialze();
            IsOpen = internalSuccess;
            return internalSuccess;
        }

        protected abstract bool ConcreteInitialze();

        // todo use template method pattern here as well
        public abstract void Send(byte[] data);
        public abstract byte[] Receive();

        public void Close()
        {
            IsOpen = false;
            ConcreteClose();
        }

        protected abstract void ConcreteClose();
        // TODO Add any other needed methods (if any)
        // Add QoL methods like SendString, ReceiveString, etc.
    }
}

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    public class UDP_UWP_NetworkConnection : NetworkConnection
    {
        public UDP_UWP_NetworkConnection(IPEndPoint endPoint) : base(endPoint)
        {
        }

        protected override bool ConcreteInitialze()
        {
            throw new System.NotImplementedException();
        }

        public override void Send(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] Receive()
        {
            throw new System.NotImplementedException();
        }

        protected override void ConcreteClose()
        {
            throw new System.NotImplementedException();
        }
    }
}