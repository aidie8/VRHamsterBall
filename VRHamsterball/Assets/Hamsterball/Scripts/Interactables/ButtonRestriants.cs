using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRestriants : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _localX = 0;
    private float _localY = 0;
    private float _localZ = 0;
    public bool _freezeAlongX = false;
    public bool _freezeAlongY = false;
    public bool _freezeAlongZ = false;
    // Use this for initialization
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();

    }

    void Update()
    {
        _localX = transform.localPosition.x;
        _localY = transform.localPosition.y;
        _localZ = transform.localPosition.z;

        if (_freezeAlongX) _localX = 0;
        if (_freezeAlongY) _localY = 0;
        if (_freezeAlongZ) _localZ = 0;
        gameObject.transform.localPosition = new Vector3(_localX, _localY, _localZ);
    }
}
