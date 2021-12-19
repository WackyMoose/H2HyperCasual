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

    private NetworkVariable<FixedString32Bytes> _displayName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        StartCoroutine(SpawnWait());
    }

    IEnumerator SpawnWait() 
    {
        yield return new WaitForSeconds(1);

        if (IsServer == true)
        {
            PlayerData? player = TankNetworkManager.GetPlayer(OwnerClientId);

            if (player.HasValue)
            {
                Debug.Log($"We have a name {player.Value.PlayerName}");
                _displayName.Value = player.Value.PlayerName;
            }

            _nameTest = "ServerFart";
        }
        else if (IsOwner == true && IsClient == true)
        {
            UpdatePlayerNameServerRpc(_nameTest, OwnerClientId);
        }
    }

    private void OnEnable()
    {
        _displayName.OnValueChanged += HandleDisplayNameChanged;
    }

    private void OnDisable()
    {
        _displayName.OnValueChanged -= HandleDisplayNameChanged;
    }

    private void HandleDisplayNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        Debug.Log(previousValue + " " + newValue);

        _textName.text = _displayName.Value.ToString();
    }

    public void UpdateName() 
    {
        _textName.text = _displayName.Value.ToString();
    }

    [ServerRpc]
    private void UpdatePlayerNameServerRpc(string playerName, ulong clientId) 
    {
        foreach (var player in NetworkManager.Singleton.ConnectedClients)
        {
            foreach (var obj in player.Value.OwnedObjects)
            {
                Debug.Log($"{clientId} owns: {obj.name}");
            }
        }
    }
}
