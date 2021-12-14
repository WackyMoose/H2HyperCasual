using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using TMPro;

namespace HelloWorld
{
    public class TankNetworkManager : MonoBehaviour
    {
        private UNetTransport _transport;

        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private TMP_InputField _portInputField;

        private void Awake()
        {
            _transport = GetComponent<UNetTransport>();
        }

        private void Update()
        {
            _transport.ConnectAddress =  _ipInputField.text;
            _transport.ConnectPort = int.Parse(_portInputField.text);
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
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();

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
