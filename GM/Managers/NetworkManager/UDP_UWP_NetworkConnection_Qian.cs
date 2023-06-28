
// This script is based on the UDPConnection.cs script from the HoloLensCamCalib project.
/*
*  UDPKeyboardInput.cs
*  HoloLensCamCalib
*
*  This file is a part of HoloLensCamCalib.
*
*  HoloLensCamCalib is free software: you can redistribute it and/or modify
*  it under the terms of the GNU Lesser General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  HoloLensCamCalib is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with HoloLensCamCalib.  If not, see <http://www.gnu.org/licenses/>.
*
*  Copyright 2020 Long Qian
*
*  Author: Long Qian
*  Contact: lqian8@jhu.edu
*
*/

using System.Net;
using Unity.VisualScripting;
using UnityEngine;

#if !UNITY_EDITOR && Unity_WSA
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    
    
    // TODO Test this Socket type. If successful, implement the missing Methods.
    // TODO Write UDP_NetworkConnection.cs for non-UWP platforms. See NetworkManager_Old.cs for reference.
    public class UDP_UWP_NetworkConnection_Qian : NetworkConnection
    {
#if !UNITY_EDITOR && Unity_WSA
    private readonly static Queue<string> receivedUDPPacketQueue = new Queue<string>();

    DatagramSocket socket;

#endif
        public UDP_UWP_NetworkConnection_Qian(IPEndPoint endPoint) : base(endPoint)
        {
        }

        protected override bool ConcreteInitialze()
        {
#if !UNITY_EDITOR && Unity_WSA
        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;
        HostName IP = null;
        try { 

            // bind socket using _endPoint
            IP = new HostName(_endPoint.Address.ToString());
            socket.BindEndpointAsync(IP, _endPoint.Port.ToString());
}

#endif
            throw new System.NotImplementedException();
        }

        protected override bool ConcreteSend(byte[] data)
        {
            throw new System.NotImplementedException();
        }

        protected override bool ConcreteReceive(out byte[] data)
        {
#if !UNITY_EDITOR && Unity_WSA
                string returnedLastUDPPacket = "";
                if (receivedUDPPacketQueue.Count > 0)
                {
                    returnedLastUDPPacket = receivedUDPPacketQueue.Dequeue();
                    data = System.Text.Encoding.UTF8.GetBytes(returnedLastUDPPacket);
                    return true;
                } else 
                {
                    data = null;
                    return false;
                }

#else
            data = null;
            return false;
#endif
        }

        protected override bool ConcreteClose()
        {
#if !UNITY_EDITOR && Unity_WSA
            if (socket != null) {
                socket.MessageReceived -= Socket_MessageReceived;
                socket.Dispose();
                Debug.Log("Socket disposed");
                return true;
            } else {
                return false;
            }
#else
            return false;
#endif
        }

        public override bool GetIPFromHostName(string hostName, out IPAddress[] ipAddresses)
        {
            try
            {
                ipAddresses = System.Net.Dns.GetHostAddresses(hostName);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
                ipAddresses = null;
                return false;
            }
        }

#if !UNITY_EDITOR && Unity_WSA
        private async void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
            Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
        {
            Stream streamIn = args.GetDataStream().AsStreamForRead();
            StreamReader reader = new StreamReader(streamIn);
            string message = await reader.ReadLineAsync();

            receivedUDPPacketQueue.Enqueue(message);

        }
#endif
        ~UDP_UWP_NetworkConnection_Qian()
        {
            Close();
        }
    }

#if !UNITY_EDITOR && Unity_WSA
    async void Start() {
        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;
        HostName IP = null;
        try
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            IP = Windows.Networking.Connectivity.NetworkInformation.GetHostNames()
            .SingleOrDefault(
                hn =>
                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                    == icp.NetworkAdapter.NetworkAdapterId);

            await socket.BindEndpointAsync(IP, port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }
        Debug.Log("DatagramSocket setup done...");
    }
    
#endif
}