using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using TMPro;
using TankGame.TankController;

namespace TankGame.Managers
{
    public class TankNetworkManager : NetworkBehaviour
    {
        private UNetTransport _transport;

        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private TMP_InputField _portInputField;

        private void Start()
        {
            _transport = GetComponent<UNetTransport>();

            NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += Singleton_OnClientDisconnectCallback;

            _ipInputField.onValueChanged.AddListener(delegate { SetupIpAndPort(); });
            //_portInputField.onValueChanged.AddListener(delegate { SetupIpAndPort(); });
        }

        private void SetupIpAndPort() 
        {
            _transport.ConnectAddress = _ipInputField.text;
            //_transport.ConnectPort = int.Parse(_portInputField.text);
        }

        private void Singleton_OnServerStarted() 
        {
            if (NetworkManager.Singleton.IsServer == false)
                return;
            Debug.Log($"Server started!");
        }

        private void Singleton_OnClientDisconnectCallback(ulong obj)
        {
            if (obj == NetworkManager.Singleton.LocalClientId)
                return;
        }

        private void Singleton_OnClientConnectedCallback(ulong obj)
        {
            if (NetworkManager.Singleton.IsServer == false)
                return;

            NetworkManager.Singleton.ConnectedClients[obj].PlayerObject.GetComponent<Tank>().SetPlayerName($"HansHenrik: {NetworkManager.Singleton.ConnectedClients.Count}");
            Debug.LogError($"{obj} connected, making the player count {NetworkManager.Singleton.ConnectedClients.Count}");
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            if (NetworkManager.Singleton.IsServer)
            {
                GUILayout.Label($"IP: {_transport.ConnectAddress} on port: {_transport.ConnectPort}");
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            
            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }
    }
}
