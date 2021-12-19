using TMPro;
using UnityEngine;
using Unity.Netcode;
using TankGame.TankUtils;
using TankGame.Utils;
using Unity.Collections;

namespace TankGame.TankController {
    public class Tank : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private float _tankDriveSpeed;
        [SerializeField] private float _maxTankDriveSpeed;
        [SerializeField] private float _tankRotateSpeed;
        [SerializeField] private float _turretRotateSpeed;
        [SerializeField] private int _rateOfFire = 60;
        [SerializeField] private float _shellVelocity;
        [SerializeField] private float _shellLifeTime;
        [SerializeField] private float _distanceBetweenTracks;
        [SerializeField] private float _trackMarksLifeTime;

        [SerializeField] private GameObject _graphics;
        [SerializeField] private Rigidbody2D _rigidBody2D;
        [SerializeField] private Transform _tankBodyTransform;
        [SerializeField] private Transform _turretTransform;
        [SerializeField] private NetworkObjectPool _networkObjectPool;
        [SerializeField] private RectTransform _hpBar;
        [SerializeField] private RectTransform _hpBarWhite;

        [SerializeField] private NetworkVariable<float> _networkHorizontalInput = new NetworkVariable<float>();
        [SerializeField] private NetworkVariable<float> _networkVerticalInput = new NetworkVariable<float>();
        [SerializeField] private NetworkVariable<float> _networkTurretAngle = new NetworkVariable<float>();
        [SerializeField] private NetworkVariable<int> _networkHitPoints = new NetworkVariable<int>(100);
        [SerializeField] private NetworkVariable<float> _networkHitPointsScale = new NetworkVariable<float>(1);

        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private GameObject _shellPrefab;
        [SerializeField] private GameObject _trackMarkPrefab;

        [SerializeField] private PlayerSpawner _playerSpawner;
        
        private float _currShootTime;

        private float _horizontalInput;
        private float _verticalInput;

        private Vector2 _lastPosition = Vector2.zero;
        #endregion

        #region Updates and all that jazz

        public override void OnNetworkSpawn()
        {
            if (IsServer == false)
                return;

            _networkObjectPool = FindObjectOfType<NetworkObjectPool>();
            _hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<RectTransform>();

            _graphics.SetActive(true);
        }

        private void OnEnable()
        {
            _networkHitPointsScale.OnValueChanged += UpdateHpBar;
        }

        private void OnDisable()
        {
            _networkHitPointsScale.OnValueChanged -= UpdateHpBar;
        }

        private void Update()
        {
            if (IsOwner && IsClient)
            {
                HandleInput();
                CreateTrackMarks();
            }

            RotateTurret();
        }

        private void FixedUpdate()
        {
            DriveTank();
            RotateTank();
        }
        #endregion

        #region Private Methods

        private void HandleInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");

            var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _turretTransform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, _turretTransform.transform.forward);

            if (Input.GetMouseButtonDown(0)) 
            {
                Shoot();
            }

            UpdateTurretRotationServerRpc(rotation.eulerAngles.z);
            UpdateClientPositionAndRotationServerRpc(_horizontalInput, _verticalInput);
        }
        private void RotateTurret()
        {
            if (_networkTurretAngle.Value != _turretTransform.rotation.eulerAngles.z)
            {
                var rot = Quaternion.Euler(new Vector3(0, 0, _networkTurretAngle.Value + 90));
                _turretTransform.rotation = Quaternion.Slerp(_turretTransform.rotation, rot, _turretRotateSpeed * Time.deltaTime);
            }
        }
        private void DriveTank() 
        {
            if (_rigidBody2D.velocity.sqrMagnitude < _maxTankDriveSpeed)
            {
                _rigidBody2D.AddForce(transform.up * _tankDriveSpeed * _networkVerticalInput.Value);
            }
        }
        private void RotateTank() 
        {
            _rigidBody2D.MoveRotation(_rigidBody2D.rotation + -(_networkHorizontalInput.Value * _tankRotateSpeed));
        }
        private void Shoot() 
        {
            ShootServerRpc();
        }
        private void CreateTrackMarks() 
        {
            var distanceDriven = Vector2.Distance(transform.position, _lastPosition);
            if (distanceDriven >= _distanceBetweenTracks)
            {
                _lastPosition = transform.position;
                CreateTrackMarksServerRpc();
            }
        }
        private void UpdateHpBar(float oldValue, float newValue) 
        {
            if (IsOwner && IsClient)
            {
                _hpBar.localScale = new Vector3(_networkHitPointsScale.Value, 1, 1);
            }
        }
        #endregion

        #region RPC
        [ServerRpc]
        private void UpdateTurretRotationServerRpc(float z)
        {
            _networkTurretAngle.Value = z;
        }

        [ServerRpc]
        private void UpdateClientPositionAndRotationServerRpc(float horizontalInput, float verticalInput)
        {
            _networkHorizontalInput.Value = horizontalInput;
            _networkVerticalInput.Value = verticalInput;
        }

        [ServerRpc]
        public void ShootServerRpc()
        {
            if (_currShootTime < Time.time)
            {
                _currShootTime = Time.time + (60 / (float)_rateOfFire);
                
                var shell = _networkObjectPool.GetNetworkObject(_shellPrefab).gameObject;
                
                shell.transform.position = _shootingPoint.position + _shootingPoint.up;
                shell.transform.eulerAngles = _shootingPoint.eulerAngles;
                
                var shellRb = shell.GetComponent<Rigidbody2D>();
                var velocity = GetComponent<Rigidbody2D>().velocity;
                velocity += (Vector2)(shell.transform.up) * _shellVelocity;
                shellRb.velocity = velocity;

                shell.GetComponent<Shell>().Setup(GetComponent<Tank>(), _shellLifeTime, OwnerClientId);
                shell.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            }
        }

        [ServerRpc]
        public void CreateTrackMarksServerRpc() 
        {
            var track = _networkObjectPool.GetNetworkObject(_trackMarkPrefab).gameObject;
            
            track.transform.position = _tankBodyTransform.position;
            track.transform.rotation = _tankBodyTransform.rotation;
            
            track.GetComponent<TrackMarks>().Setup(_trackMarksLifeTime);
            track.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        }

        #endregion

        #region Public Methods
        public void TakeDamage(int damage, Tank damagingTank) 
        {
            //Debug.Log($"{damagingTank.name} dealth {damage} to us!");
            _networkHitPoints.Value = _networkHitPoints.Value - damage;

            _networkHitPointsScale.Value = (float)_networkHitPoints.Value / 100;

            //TODO Generate smoke when are low on HP...

            if (_networkHitPoints.Value <= 0)
            {
                _networkHitPoints.Value = 0;

                //TODO Blow up, or something like that

                _networkHitPoints.Value = 100;

                transform.position = _playerSpawner.GetNextSpawnPosition();

                _rigidBody2D.velocity = Vector2.zero;
                _rigidBody2D.angularVelocity = 0;
            }
        }
        #endregion
    }
    public enum TankColor 
    {
        Dark,Red,Green,Blue,Sand
    }
}
