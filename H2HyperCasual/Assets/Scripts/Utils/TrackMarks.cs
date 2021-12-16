using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class TrackMarks : NetworkBehaviour
{
    private float _lifeTime;

    public void Setup(float lifeTime) 
    {
        _lifeTime = lifeTime;

        if (IsServer)
        {
            StartCoroutine(DestroyTrackMarks());
        }
    }

    public override void OnNetworkDespawn()
    {
        StopAllCoroutines();
    }

    IEnumerator DestroyTrackMarks()
    {
        if (!NetworkObject.IsSpawned)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_lifeTime);

        NetworkObject.Despawn(true);
    }
}
