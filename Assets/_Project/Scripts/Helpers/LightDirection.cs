using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDirection : MonoBehaviour
{
    [SerializeField] Transform camTrans;

    private void Update()
    {
        var dir = camTrans.forward.Flat().normalized;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
