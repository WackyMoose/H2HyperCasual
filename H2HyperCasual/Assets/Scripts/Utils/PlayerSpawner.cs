using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TankGame.Utils
{
    public class PlayerSpawner : MonoBehaviour
    {
        NetworkManager _NetworkManager;

        int _RoundRobinIndex = 0;

        [SerializeField]
        List<Transform> _SpawnPositions = new List<Transform>();

        /// <summary>
        /// Get a spawn position for a spawned object based on the spawn method.
        /// </summary>
        /// <returns>?The spawn position.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Vector3 GetNextSpawnPosition()
        {
            _RoundRobinIndex = (_RoundRobinIndex + 1) % _SpawnPositions.Count;
            return _SpawnPositions[_RoundRobinIndex].position;
        }

        private void Awake()
        {
            _NetworkManager = FindObjectOfType<NetworkManager>();
            _NetworkManager.ConnectionApprovalCallback += ConnectionApprovalWithRandomSpawnPos;
        }

        void ConnectionApprovalWithRandomSpawnPos(byte[] payload, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            callback(true, null, true, GetNextSpawnPosition(), Quaternion.identity);
        }
    }
}