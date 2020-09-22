using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StencilShadow : MonoBehaviour
{
    [SerializeField] GameObject shadowObject;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    GameObject body;
    private Bounds bounds;
    bool initialized;
    int layerMask;

    void Awake()
    {
        if (!initialized)
            Initialize();
    }

    private void Start()
    {
        var session = GameObject.FindGameObjectWithTag("ARSession");
        raycastManager = session.GetComponent<ARRaycastManager>();
        planeManager = session.GetComponent<ARPlaneManager>();
    }

    public void Update()
    {
        var rayPoint = transform.position - Vector3.up * bounds.extents.y;
        Ray rayDown = new Ray(rayPoint, Vector3.down);

        if (Application.isEditor)
        {
            if (Physics.Raycast(rayDown, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                shadowObject.transform.position = hit.point;
                shadowObject.transform.localScale = transform.localScale / hit.distance;
                shadowObject.SetActive(true);
            }
            else
            {
                shadowObject.SetActive(false);
            }
        }
        else
        {
            if (raycastManager.Raycast(rayDown, hits, TrackableType.PlaneWithinInfinity))
            {
                var trackable = planeManager.GetPlane(hits[0].trackableId);
                if (trackable.alignment == PlaneAlignment.HorizontalUp)
                {
                    shadowObject.transform.position = hits[0].pose.position;
                    shadowObject.transform.localScale = transform.localScale / hits[0].distance;
                    shadowObject.SetActive(true);
                }
                else
                {
                    shadowObject.SetActive(false);
                }
            }
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
