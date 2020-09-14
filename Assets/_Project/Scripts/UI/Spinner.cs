using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] bool active = true;
    [SerializeField] float speed = 1;
    [SerializeField] Vector3 axis;

    void FixedUpdate()
    {
        if (active)
            transform.Rotate(axis, speed);
    }
}
