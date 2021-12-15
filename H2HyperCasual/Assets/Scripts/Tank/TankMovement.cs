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

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        public void MoveTank(float input) 
        {
            if (input != 0)
            {
                if (_rigidBody2D.velocity.sqrMagnitude < _maxSpeed)
                {
                    _rigidBody2D.AddForce(transform.up * _driveSpeed * input);
                }
            }
        }

        public void RotateTank(float input) 
        {
            if (input != 0)
            {
                _rigidBody2D.MoveRotation(_rigidBody2D.rotation + -(input * _rotateSpeed));
            }
        }
    }
}
