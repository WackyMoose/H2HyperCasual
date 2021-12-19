using System.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using TankGame.Managers;

public class TankPlayerName : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TMP_Text _textName;

    [SerializeField] private string _nameTest;

    private NetworkVariable<FixedString32Bytes> _displayName = new NetworkVariable<FixedString32Bytes>(new FixedString32Bytes(""));

    private void Awake()
    {
        _displayName.OnValueChanged += HandleDisplayNameChanged;
    }

    private void Start()
    {
        if (IsOwner == true)
        {
            SetPlayerNameServerRpc("Henzi");
        }
    }

    //public override void OnNetworkSpawn()
    //{
    //    Debug.Log("We spawned some tanks");

    //    StartCoroutine(SpawnWait());
    //}

    [Unity.Netcode.ServerRpc(RequireOwnership = true)]
    private void SetPlayerNameServerRpc(string name)
    {
        this._displayName.Value = name;
    }

    //private void OnEnable()
    //{
    //    _displayName.OnValueChanged += HandleDisplayNameChanged;
    //}

    //private void OnDisable()
    //{
    //    _displayName.OnValueChanged -= HandleDisplayNameChanged;
    //}

    private void HandleDisplayNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        Debug.Log(previousValue + " " + newValue);

        _textName.text = newValue.Value;
    }

    //public void UpdateName() 
    //{
    //    _textName.text = _displayName.Value.ToString();
    //}

    //[ServerRpc]
    //private void UpdatePlayerNameServerRpc(string playerName, ulong clientId) 
    //{
    //    foreach (var player in NetworkManager.Singleton.ConnectedClients)
    //    {
    //        foreach (var obj in player.Value.OwnedObjects)
    //        {
    //            Debug.Log($"{clientId} owns: {obj.name}");
    //        }
    //    }
    //}
}
