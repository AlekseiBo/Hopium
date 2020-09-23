using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProperties : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] bool beyondPortal;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    void Start()
    {
        transform.SetLocalScale(Random.Range(minSize, maxSize));

        int index = Random.Range(0, prefabs.Length - 1);
        var go = Instantiate(prefabs[index], transform, false);

        if (beyondPortal)
        {
            go.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Beyond");

            var shadow = GetComponent<StencilShadow>();
            if (shadow != null)
            {
                shadow.DisableShadow();
            }
        }
        
    }
}
