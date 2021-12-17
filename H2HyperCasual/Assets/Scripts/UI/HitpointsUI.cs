using UnityEngine;
using Unity.Netcode;
using TMPro;

namespace TankGame.UI
{
    public class HitpointsUI : NetworkBehaviour
    {
        [SerializeField] private RectTransform _hpBar;
        [SerializeField] private RectTransform _hpBarwhite;

        private bool _isOwner;

        private void Awake()
        {
            if (IsOwner && IsClient)
            {
                _isOwner = true;
            }
        }

        public void UpdateHPBar(float newBarScale)
        {
            if (_isOwner == false)
            {
                return;
            }

            _hpBar.localScale = new Vector3(newBarScale,1,1);
        }
    }
}
