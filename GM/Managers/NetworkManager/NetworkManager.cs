using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Unity_Utilities.GM.Managers.NetworkManager
{
    public class NetworkManager : MonoBehaviour
    {
        // create Hashmap of NetworkConnections with string identifier
        private readonly Dictionary<string, NetworkConnection> _networkConnections = new Dictionary<string, NetworkConnection>();

        /// <summary>
        /// Add a networkConnection to the NetworkManager
        /// </summary>
        /// <param name="identifier">The key with which the NetworkConnection is to be Identified</param>
        /// <param name="networkConnection">The NetworkConnection to add</param>
        /// <returns>true if the element is added to the NetworkManager successfully; false if the identifier is already in use.</returns>
        public bool AddNetworkConnection(string identifier, NetworkConnection networkConnection)
        {
            return _networkConnections.TryAdd(identifier, networkConnection);
        }

        /// <summary>
        /// Try to remove a NetworkConnection from the NetworkManager
        /// </summary>
        /// <param name="identifier">The key of the NetworkConnection to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if key is not found in the Dictionary</returns>
        public bool RemoveNetworkConnection(string identifier)
        {
            return _networkConnections.Remove(identifier);
        }

        /// <summary>
        /// Try to get a NetworkConnection from the NetworkManager.
        /// </summary>
        /// <param name="identifier">The key of the NetworkConnection to get.</param>
        /// <param name="networkConnection">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter</param>
        /// <returns>true if the element is successfully found and returned; otherwise, false. This method returns false if key is not found in the Dictionary</returns>
        public bool TryGetNetworkConnection(string identifier, out NetworkConnection networkConnection)
        {
            return _networkConnections.TryGetValue(identifier, out networkConnection);
        }
    }
}