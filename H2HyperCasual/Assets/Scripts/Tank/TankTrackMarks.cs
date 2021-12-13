using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TankTrackMarks : MonoBehaviour
{
    [SerializeField] private Transform _trackPrefab;
    [SerializeField] private int _trackAmount;
    private ObjectPool<Transform> _tracksPool;

    private void Awake()
    {
        _tracksPool = new ObjectPool<Transform>(() => { return Instantiate(_trackPrefab); },
                                                            mark => { mark.gameObject.SetActive(true); },
                                                            mark => { mark.gameObject.SetActive(false); },
                                                            mark => { Destroy(mark.gameObject); },
                                                            false,
                                                            _trackAmount, 200);
    }
}
