using Unity.Netcode;
using UnityEngine;

namespace TankGame.TankController
{
    public class TankMovement : NetworkBehaviour
    {
        [SerializeField] private float _driveSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotateSpeed;
    
        private Rigidbody2D _rigidBody2D;
        [SerializeField]
        private NetworkVariable<float> _networkHorizontalInput = new NetworkVariable<float>();
        [SerializeField]
        private NetworkVariable<float> _networkVerticalInput = new NetworkVariable<float>();

        private float _horizontalInput;
        private float _verticalInput;

        private float _oldHorizontalInput;
        private float _oldVeritcalInput;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsOwner && IsClient)
            {
                HandleInput();
            }
        }

        private void FixedUpdate()
        {
            MoveTank();
            RotateTank();
        }

        private void HandleInput() 
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");

            if (_oldHorizontalInput != _horizontalInput || _oldVeritcalInput != _verticalInput)
            {
                _oldHorizontalInput = _horizontalInput;
                _oldVeritcalInput = _verticalInput;
                UpdateClientPositionAndRotationServerRpc(_horizontalInput, _verticalInput);
            }
        }

        private void MoveTank() 
        {
            if (_networkVerticalInput.Value != 0)
            {
                if (_rigidBody2D.velocity.sqrMagnitude < _maxSpeed)
                {
                    _rigidBody2D.AddForce(transform.up * _driveSpeed * _networkVerticalInput.Value);
                }
            }
        }

        private void RotateTank() 
        {
            if (_networkHorizontalInput.Value != 0)
            {
                _rigidBody2D.MoveRotation(_rigidBody2D.rotation + -(_networkHorizontalInput.Value * _rotateSpeed));
            }
        }

        [ServerRpc]
        public void UpdateClientPositionAndRotationServerRpc(float newHorizontalInput, float newVerticalInput) 
        {
            _networkHorizontalInput.Value = newHorizontalInput;
            _networkVerticalInput.Value = newVerticalInput;
        }
    }
}
