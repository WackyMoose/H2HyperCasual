using TankGame.TankController;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

namespace TankGame.TankUtils {
    public class Shell : NetworkBehaviour
    {
        [SerializeField] private List<GameObject> _shellGraphics = new List<GameObject>();
        [SerializeField] private int _shellDamage = 10;
        private float _lifeTime;
        private Tank _ownerTank;

        //TODO fix the lag problem!
        public void Setup(Tank owner, float lifeTime) 
        {
            _ownerTank = owner;
            _lifeTime = lifeTime;

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (IsServer)
            {
                //Need some way to despawn it!
                StartCoroutine(DestroyBullet());
            }
        }

        public override void OnNetworkDespawn()
        {
            //Create a explosion particles/animation
            StopAllCoroutines();
        }

        

        IEnumerator DestroyBullet()
        {
            if (!NetworkObject.IsSpawned)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_lifeTime);

            NetworkObject.Despawn(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collisionObject = collision.gameObject;

            if (NetworkManager.Singleton.IsServer == false ||NetworkObject.IsSpawned == false)
            {
                return;
            }

            if (collisionObject.CompareTag("Obstacle"))
            {
                DestroyBullet();
            }

            if (collisionObject.TryGetComponent<Tank>(out var tank))
            {
                Debug.LogError("We hit someone");
                if (tank != _ownerTank)
                {
                    tank.TakeDamage(_shellDamage,_ownerTank);
                    DestroyBullet();
                }
            }
        }
    }
}
