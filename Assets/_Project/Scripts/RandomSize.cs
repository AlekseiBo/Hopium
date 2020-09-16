using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSize : MonoBehaviour
{
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    void Start()
    {
        transform.SetLocalScale(Random.Range(minSize, maxSize));
    }
}
