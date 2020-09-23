using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalContent : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
        GameManager.RegisterSpawnedObject(gameObject);
    }
}
