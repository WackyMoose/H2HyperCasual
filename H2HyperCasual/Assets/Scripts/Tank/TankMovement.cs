using System.Collections;
using System.Collections.Generic;
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

        private float _horizontalInput;
        private float _verticalInput;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsOwner)
            {
                _horizontalInput = Input.GetAxis("Horizontal");
                _verticalInput = Input.GetAxis("Vertical");
            }
        }

        private void FixedUpdate()
        {
            if (_rigidBody2D.velocity.sqrMagnitude < _maxSpeed)
            {
                _rigidBody2D.AddForce(transform.up * _driveSpeed * _verticalInput);
            }
            _rigidBody2D.MoveRotation(_rigidBody2D.rotation + -(_horizontalInput * _rotateSpeed));
        }
    }
}
