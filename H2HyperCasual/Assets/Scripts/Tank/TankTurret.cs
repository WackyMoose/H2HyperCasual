using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.TankController
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Transform _turret;
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private GameObject _muzzleFlash;

        private float _turretAngle;
    }
}
