using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using IXRE.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    // Use this to get IDE Support for Non-Editor Code. 
    // DISABLE THIS FOR BUILD
    // #undef UNITY_EDITOR
    public class NetworkManager_OLD : MonoBehaviour, IManager
    {
       


        //###############################################################
        // TODO Rewrite this with the new NetworkConnection class. and make all of it as generic as possible.
        //###############################################################

        private string _hostName = "comma1.adm.ds.fhnw.ch";
        private int _port = 1001;
        [NonSerialized] public Vector3 ColorLeft;
        [NonSerialized] public Vector3 ColorRight;

        private UDPConnection _udpConnection;

        private int _lastIndex = 0;

        private long _averageDeliveryTime;
        private double _deliveredCOunt;


#if !UNITY_EDITOR
        private void Start()
        {
            _udpConnection = FindObjectOfType<UDPConnection>();
            Debug.Log("_udpConnection: " + _udpConnection.name);
        }

#endif

#if UNITY_EDITOR
        private void Start()
        {
            InitializeUDP();
        }
#endif


        public void UpdatePorts(string newHostName, int newPort)
        {
            _port = newPort;
            _hostName = newHostName;
#if UNITY_EDITOR
            InitializeUDP();
#else
            Destroy(_udpConnection.gameObject);
            _udpConnection = Instantiate(new GameObject("UDPConnection")).AddComponent<UDPConnection>();
            _udpConnection.port = _port;
            _udpConnection.gameObject.name = "UDPConnection";
            Debug.Log("Created new UDPConnection: " + _udpConnection.name);

#endif
            ColorLeft = new Vector3(0, 0, 0);
            ColorRight = new Vector3(0, 0, 0);
        }


        // TODO REMOVE this is debug stuff
        [SerializeField] private TMP_Text text;

        [FormerlySerializedAs("interpolationTimeFrame")]
        public float interpolationFactor = 0.1f;

#if !UNITY_EDITOR
        private void Update()
        {
            string message = _udpConnection.GetLatestUDPPacket();
            if (message != "") DecypherMessage(message);
        }
#endif


#if UNITY_EDITOR
        private UdpClient _udpClientSender;
        private UdpClient _udpClientReceiver;
        private IPEndPoint _remoteEndpoint;
        private Thread _receivingThread;
        private string _latestMessage;

        public void SendUDPString(string message)
        {
            Debug.Log("Sending message: " + message + " to " + _hostName + " / " + _remoteEndpoint.ToString());

            byte[] data = Encoding.UTF8.GetBytes(message);
            _udpClientSender.Send(data, data.Length, _remoteEndpoint);
        }

        private void InitializeUDP()
        {
            // cleanup
            if (_udpClientSender != null)
            {
                _udpClientSender.Close();
                _udpClientSender = null;
            }

            if (_udpClientReceiver != null)
            {
                _udpClientReceiver.Close();
                _udpClientReceiver = null;
            }

            if (_receivingThread != null)
            {
                _receivingThread.Abort();
                _receivingThread = null;
            }


            Debug.Log("Initializing UDP" + " " + _hostName + " " + _port);
            // Send
            _udpClientSender = new UdpClient();
            // get first Address, assuming no ambiguity
            IPAddress[] addresses = System.Net.Dns.GetHostAddresses(_hostName);
            if (addresses.Length == 0) return;
            _remoteEndpoint = new IPEndPoint(addresses[0], _port);

            // Receive
            _udpClientReceiver = new UdpClient(_port);
            _receivingThread = new Thread(() => HandleReceiveData(new IPEndPoint(IPAddress.Any, _port)))
                {IsBackground = true};
            _receivingThread.Start();
            Debug.Log("Initialized UDP successfully ended");
        }
        
#endif

#if UNITY_EDITOR
        private void HandleReceiveData(IPEndPoint anyIP)
        {
            while (true)
            {
                try
                {
                    byte[] data = _udpClientReceiver.Receive(ref anyIP);

                    // Debug.Log("Received data from " + anyIP.ToString());
                    string text = Encoding.UTF8.GetString(data);

                    _latestMessage = text;
                }
                catch (Exception err)
                {
                    Debug.Log(err.ToString());
                }
            }
        }

        private void OnDestroy()
        {
            _receivingThread.Abort();
        }
#endif
    }
}