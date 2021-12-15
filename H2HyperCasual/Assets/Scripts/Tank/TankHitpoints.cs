using UnityEngine;
using Unity.Netcode;
using TankGame.UI;
using TMPro;

namespace TankGame.TankUtils
{
    public class TankHitpoints : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private NetworkVariable<int> _hitPoints = new NetworkVariable<int>();
        private HitpointsUI _hitPointsUI;

        private void Awake()
        {
            _hitPoints.Value = 100;
            _hpText.text = $"HP: {_hitPoints.Value}";
        }

        //public void TakeDamage(int damage)
        //{
        //    _hitPoints.Value -= damage;
        //    _hpText.text = $"HP: {_hitPoints.Value}";

        //    if (_hitPoints.Value <= 0)
        //    {
        //        Debug.LogError($"We died");
        //    }
        //}

        public void UpdateHPBar(int newHPAmount)
        {
            _hpText.text = $"HP: {newHPAmount}";
        }

        public void DestroyTank() 
        {
        
        }
    }
}
