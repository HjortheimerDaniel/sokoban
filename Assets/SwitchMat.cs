using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMat : MonoBehaviour
{
    [SerializeField] Material[] _mats;
    Renderer _r;
    int _i;

    void Start()
    {
        _r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            _r.material = _mats[_i % _mats.Length];
            _i++;
        }
    }
}
