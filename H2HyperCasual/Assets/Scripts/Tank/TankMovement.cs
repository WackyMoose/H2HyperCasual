using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.TankController
{
    public class TankMovement : MonoBehaviour
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
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");


        }

        private void FixedUpdate()
        {
            _rigidBody2D.MovePosition(_rigidBody2D.position * _horizontalInput * _driveSpeed);
            _rigidBody2D.MoveRotation(_rigidBody2D.rotation + _verticalInput * _rotateSpeed);
        }
    }
}
