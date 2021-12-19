using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;
using TankGame.Utils;
using UnityEngine.UI;

namespace TankGame.Managers
{
    public class TankNetworkManager : NetworkManager
    {
        public event Action<GameState> OnGameStateChanged;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _leaveButton;

        [SerializeField] private GameObject _lobbyUI;
        [SerializeField] private GameObject _leaveUI;

        private GameState _gameState = GameState.Lobby;

        private UNetTransport _transport;
        private PlayerSpawner _playerSpawner;

        private static Dictionary<ulong, PlayerData> clientData;

        [SerializeField] private int kills;

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();

            _hostButton.onClick.AddListener(Host);
            _clientButton.onClick.AddListener(Client);
            _leaveButton.onClick.AddListener(Leave);

            _leaveUI.SetActive(false);

            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }

        private void HandleServerStarted() 
        {
            // Temporary workaround to treat host as client
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.ServerClientId);
            }
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer == true)
            {
                clientData.Remove(clientId);
            }

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _leaveUI.SetActive(false);
                _lobbyUI.SetActive(true);
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                _lobbyUI.SetActive(false);
                _leaveUI.SetActive(true);
            }
        }

        public static PlayerData? GetPlayer(ulong clientId) 
        {
            if (clientData.TryGetValue(clientId, out PlayerData player))
            {
                return player;
            }

            return null;
        }

        public void Host() 
        {
            clientData = new Dictionary<ulong, PlayerData>();
            clientData[NetworkManager.Singleton.LocalClientId] = new PlayerData("Hans");
            
            var payload = JsonUtility.ToJson(new ConnectionPayload() { playerName = clientData[NetworkManager.Singleton.LocalClientId].PlayerName });

            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);

            // Set password ready to send to the server to validate
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
            NetworkManager.Singleton.StartHost();
            ChangeGameState(GameState.Ongoing);
        }

        public void Client() 
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload() { playerName = "Henzi" });

            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);

            // Set password ready to send to the server to validate
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
            NetworkManager.Singleton.StartClient();
        }

        public void Leave() 
        {
            NetworkManager.Singleton.Shutdown();

            if (NetworkManager.Singleton.IsServer == true)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            }

            _leaveUI.SetActive(false);
            _lobbyUI.SetActive(true);
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            string payload = Encoding.ASCII.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            bool approveConnection = true;

            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (approveConnection == true)
            {
                spawnPos = _playerSpawner.GetNextSpawnPosition();
                clientData[clientId] = new PlayerData(connectionPayload.playerName);
            }

            callback(true, null, approveConnection, spawnPos, spawnRot);
        }

        [ServerRpc]
        public void UpdateMatchDataServerRpc(ulong killId, ulong dyingId) 
        {
            Debug.Log($"{NetworkManager.Singleton.ConnectedClients[killId].ClientId} killed {NetworkManager.Singleton.ConnectedClients[dyingId].ClientId}");
            kills++;

            if (kills > 3)
            {
                ChangeGameState(GameState.Finished);
            }
        }

        public void ChangeGameState(GameState newState) 
        {
            _gameState = newState;
            OnGameStateChanged?.Invoke(_gameState);
        }


        //TODO Use the playername from player model to populate all tank text name component...
    }

    public enum GameState
    {
        Lobby,
        Ongoing,
        Finished,
        Stopped,
    }
}
