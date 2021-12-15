using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TankGame.UI;
using TMPro;

namespace TankGame.TankUtils
{
    public class TankHitpoints : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private int _hitPoints;
        private HitpointsUI _hitPointsUI;

        private void Awake()
        {
            _hitPoints = 100;
            _hpText.text = $"HP: {_hitPoints}";
        }

        public void TakeDamage(int damage)
        {
            _hitPoints -= damage;
            _hpText.text = $"HP: {_hitPoints}";
            
            if (_hitPoints <= 0)
            {
                Debug.LogError($"We died");
            }
        }

        //public void UpdateHPBar(int newHPAmount) 
        //{
        //    _hpText.text = $"HP: {newHPAmount}";
        //}

        public void DestroyTank() 
        {
        
        }
    }
}
