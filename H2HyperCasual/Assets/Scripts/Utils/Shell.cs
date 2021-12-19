using TankGame.TankController;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TankGame.TankUtils {
    public class Shell : NetworkBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private int _shellDamage = 10;
        private float _lifeTime;
        private Tank _ownerTank;
        private ulong _ownerID;

        //TODO fix the lag problem!
        public void Setup(Tank owner, float lifeTime, ulong ownerID) 
        {
            _ownerTank = owner;
            _lifeTime = lifeTime;
            _ownerID = ownerID;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            if (IsServer)
            {
                //Need some way to despawn it!
                StartCoroutine(DestroyBulletAfterLifeTime(_lifeTime));
            }
        }

        public override void OnNetworkDespawn()
        {
            //Create a explosion particles/animation

            var exp = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(exp, 1);

            StopAllCoroutines();
        }

        IEnumerator DestroyBulletAfterLifeTime(float time)
        {
            if (!NetworkObject.IsSpawned)
            {
                yield return null;
            }

            yield return new WaitForSeconds(time);

            NetworkObject.Despawn(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collisionObject = collision.gameObject;

            if (NetworkManager.Singleton.IsServer == false || NetworkObject.IsSpawned == false)
            {
                return;
            }

            if (collisionObject.CompareTag("Obstacle"))
            {
                StartCoroutine(DestroyBulletAfterLifeTime(0));

            }

            if (collisionObject.TryGetComponent<Tank>(out var tank))
            {
                Debug.Log("We hit someone");
                if (tank != _ownerTank)
                {
                    tank.TakeDamage(_shellDamage,_ownerTank);
                    StartCoroutine(DestroyBulletAfterLifeTime(0));
                }
            }
        }
    }
}
