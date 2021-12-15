using UnityEngine;
using Unity.Netcode;
using TankGame.TankUtils;

namespace TankGame.TankController {
    public class TurretShooting : NetworkBehaviour
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private int _rateOfFire = 60;

        private float _currShootTime;


        private void Update()
        {
            if (IsOwner)
            {
                if (Input.GetMouseButtonDown(0) && _currShootTime < Time.time)
                {
                    _currShootTime = Time.time + (60 / (float)_rateOfFire);
                    Shoot();
                }
            }
        }

        private void Shoot()
        {
            Debug.DrawRay(_shootingPoint.position, -_shootingPoint.up * 5, Color.red, 0.5f);
            RaycastHit2D hit = Physics2D.Raycast(_shootingPoint.position, -_shootingPoint.up, Mathf.Infinity);
            if (hit.collider != null)
            {
                DealDamageServerRPC(10, hit.collider.GetComponent<NetworkObject>().OwnerClientId);
            }
        }


        [ServerRpc]
        public void DealDamageServerRPC(int damage, ulong idToDamage)
        {
            var tankToDamage = NetworkManager.Singleton.ConnectedClients[idToDamage].PlayerObject.GetComponent<TankHitpoints>();
            if (tankToDamage == null)
                return;

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { idToDamage }
                }
            };

            tankToDamage.GetComponent<TankHitpoints>().TakeDamage(damage);
            TakeDamageClientRpc(damage, clientRpcParams);
            //Debug.LogError("We send this");
        }

        [ClientRpc]
        public void TakeDamageClientRpc(int damage, ClientRpcParams clientRpcParams = default)
        {
            if (IsOwner)
                return;

            Debug.LogError($"We took {damage}");
            GetComponent<TankHitpoints>().TakeDamage(damage);
        }
    }
}