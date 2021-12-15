using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

namespace TankGame.TankController
{
    public class TurretRotation : NetworkBehaviour
    {
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Transform _turret;

        public Transform Turret { get { return _turret; } private set { _turret = value; } }

        public void RotateTurret(float angle)
        {
            if (angle != _turret.rotation.eulerAngles.z)
            {
                var rot = Quaternion.Euler(new Vector3(0,0, angle + 90));
                _turret.rotation = Quaternion.Slerp(_turret.rotation,rot,_rotateSpeed*Time.deltaTime);
            }
        }
    }
}
