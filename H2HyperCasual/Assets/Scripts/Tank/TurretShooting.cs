using UnityEngine;
using Unity.Netcode;
using TankGame.Utils;

namespace TankGame.TankController {
    public class TurretShooting : NetworkBehaviour
    {
        [SerializeField] private NetworkObjectPool _networkObjectPool;
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private int _rateOfFire = 60;
        [SerializeField] private GameObject _shellPrefab;

        private float _currShootTime;

        private void Awake()
        {
            _networkObjectPool = FindObjectOfType<NetworkObjectPool>();
        }

        [ServerRpc]
        public void ShootServerRPC() 
        {
            if (_currShootTime < Time.time)
            {
                _currShootTime = Time.time + (60 / (float)_rateOfFire);
                //Shoot();
            }
        }

        //public void Shoot()
        //{
        //    var shell = _networkObjectPool.GetNetworkObject(_shellPrefab).gameObject;
        //    shell.transform.position = _shootingPoint.position + _shootingPoint.up;
        //    shell.transform.eulerAngles = _shootingPoint.eulerAngles;
        //    var shellRb = shell.GetComponent<Rigidbody2D>();

        //    var velocity = GetComponent<Rigidbody2D>().velocity;
        //    velocity += (Vector2)(shell.transform.up) * 10;
        //    shellRb.velocity = velocity;

        //    TankColor tankColor = (TankColor)Random.Range(0, 5);
            
        //    shell.GetComponent<Shell>().Setup(GetComponent<Tank>(), 15,tankColor);
        //    shell.GetComponent<NetworkObject>().Spawn(true);
        //}
    }
}