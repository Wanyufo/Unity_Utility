using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    /// <summary>
    /// Abstract class for Network Connections. Methods return booleans success and data as out parameters.
    /// </summary>
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
            if (IsOpen)
            {
                return false;
            }

            // template Method Pattern
            bool internalSuccess = ConcreteInitialze();
            IsOpen = internalSuccess;
            return internalSuccess;
        }

        public bool Send(byte[] data)
        {
            if (!IsOpen)
            {
                return false;
            }

            return ConcreteSend(data);
        }

        public bool Receive(out byte[] data)
        {
            if (!IsOpen)
            {
                data = null;
                return false;
            }

            return ConcreteReceive(out data);
        }

        public bool Close()
        {
            if (!IsOpen)
            {
                return false;
            }

            IsOpen = false;
            return ConcreteClose();
        }


        protected abstract bool ConcreteInitialze();
        protected abstract bool ConcreteSend(byte[] data);
        protected abstract bool ConcreteReceive(out byte[] data);
        protected abstract bool ConcreteClose();

        // QoL Methods

        /// <summary>
        /// Send String encoded in UTF8
        /// </summary>
        /// <param name="data">String to send</param>
        /// <returns>bool success</returns>
        public bool SendString(string data)
        {
            return Send(System.Text.Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Receive String encoded in UTF8
        /// </summary>
        /// <param name="data">received data as string</param>
        /// <returns>bool success</returns>
        public bool ReceiveString(out string data)
        {
            bool success = Receive(out byte[] dataBytes);
            data = System.Text.Encoding.UTF8.GetString(dataBytes);
            return success;
        }

        public abstract bool GetIPFromHostName(string hostName, out IPAddress[] ipAddresses);
    }
}