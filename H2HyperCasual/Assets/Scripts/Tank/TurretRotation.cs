using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

namespace TankGame.TankController
{
    public class TurretRotation : NetworkBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Transform _turret;

        private NetworkVariable<float> _networkTurretAngle = new NetworkVariable<float>();

        private void Update()
        {
            if (IsOwner && IsClient)
            {
                HandleInput();
            }

            RotateTurret();
        }
        private void HandleInput()
        {
            var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _turret.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, _turret.transform.forward);
            UpdateTurretRotationServerRpc(rotation.eulerAngles.z);
        }

        private void RotateTurret()
        {
            if (_networkTurretAngle.Value != _turret.rotation.eulerAngles.z)
            {
                var rot = Quaternion.Euler(new Vector3(0,0,_networkTurretAngle.Value + 90));
                _turret.rotation = Quaternion.Slerp(_turret.rotation,rot,_rotateSpeed*Time.deltaTime);
            }
        }

        [ServerRpc]
        public void UpdateTurretRotationServerRpc(float newAngle) 
        {
            _networkTurretAngle.Value = newAngle;
        }
    }
}
