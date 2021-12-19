using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TankGame.Utils;
using TankGame.Models;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        private PlayerManager _playerManager;

        private MatchData _matchData = new MatchData();

        private void Start()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            _playerManager = FindObjectOfType<PlayerManager>();

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

        public void Host() 
        {
            var player = _playerManager.GetPlayerData().player;
            byte[] playerData = PlayerObjectToByteArray(player);

            // Set password ready to send to the server to validate
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = playerData;
            NetworkManager.Singleton.StartHost();
            ChangeGameState(GameState.Ongoing);
        }

        public void Client() 
        {
            var player = _playerManager.GetPlayerData().player;
            byte[] playerData = PlayerObjectToByteArray(player);

            // Set password ready to send to the server to validate
            NetworkManager.Singleton.NetworkConfig.ConnectionData = playerData;
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
            var playerData = ByteArrayToPlayerObject(connectionData);

            _matchData.AddPlayer(clientId, playerData);

            bool approveConnection = true;

            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            spawnPos = _playerSpawner.GetNextSpawnPosition();

            callback(true, null, approveConnection, spawnPos, spawnRot);
        }

        [ServerRpc]
        public void UpdateMatchDataServerRpc(ulong killId, ulong dyingId) 
        {
            Debug.Log($"{NetworkManager.Singleton.ConnectedClients[killId].ClientId} killed {NetworkManager.Singleton.ConnectedClients[dyingId].ClientId}");
        }

        [ServerRpc]
        public void AddPlayerToMatchServerRpc(ulong clientId,Player playerToAdd) 
        {
            _matchData.AddPlayer(clientId,playerToAdd);
        }
        
        public void ChangeGameState(GameState newState) 
        {
            _gameState = newState;
            OnGameStateChanged?.Invoke(_gameState);
        }

        byte[] PlayerObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private Player ByteArrayToPlayerObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Player obj = (Player)binForm.Deserialize(memStream);

            return obj;
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
