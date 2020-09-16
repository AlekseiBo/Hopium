using UnityEngine;
using System.Collections.Generic;

public class StencilShadow : MonoBehaviour
{
    [SerializeField] GameObject shadowObject;

    GameObject body;
    private Bounds bounds;
    bool initialized;
    int layerMask;

    void Awake()
    {
        if (!initialized)
            Initialize();
    }

    public void Update()
    {
        var rayPoint = transform.position - Vector3.up * bounds.extents.y;
        Ray rayDown = new Ray(rayPoint, Vector3.down);

        if (Physics.Raycast(rayDown, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            shadowObject.transform.position = hit.point;
            shadowObject.SetActive(true);
        }
        else
        {
            shadowObject.SetActive(false);
        }
    }

    private void Initialize()
    {
        layerMask = LayerMask.GetMask("Plane");

        body = GetComponentInParent<Rigidbody>().gameObject;

        bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Collider next in body.GetComponentsInChildren<Collider>(true))
        {
            if (!next.isTrigger)
                bounds.Encapsulate(next.bounds);
        }

        shadowObject.SetActive(false);
        initialized = true;
    }
}
