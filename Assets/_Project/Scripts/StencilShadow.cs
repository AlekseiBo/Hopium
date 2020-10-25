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
    private Rigidbody body;
    private Quaternion shadowRotation;
    private Bounds bounds;
    private bool initialized;
    private int layerMask;

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
        var rayPoint = bounds.center;
        Ray rayDown = new Ray(rayPoint, Vector3.down);

        if (Application.isEditor)
        {
            if (Physics.Raycast(rayDown, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                shadowObject.transform.position = hit.point;
                shadowObject.transform.localScale = Vector3.Min(Vector3.one, Vector3.one / hit.distance);
                shadowObject.transform.rotation = shadowRotation;
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
                    shadowObject.transform.localScale = Vector3.Min(Vector3.one, Vector3.one / hits[0].distance);
                    shadowObject.transform.rotation = shadowRotation;
                    shadowObject.SetActive(true);
                }
                else
                {
                    shadowObject.SetActive(false);
                }
            }
        }
    }

    public void DisableShadow()
    {
        if (shadowObject != null)
            Destroy(shadowObject);

        Destroy(this);
    }

    private void Initialize()
    {
        shadowRotation = shadowObject.transform.rotation;

        layerMask = LayerMask.GetMask("Plane");

        body = GetComponentInParent<Rigidbody>();

        bounds = new Bounds(body.transform.position, Vector3.zero);
        foreach (Collider next in body.GetComponentsInChildren<Collider>(true))
        {
            if (!next.isTrigger)
                bounds.Encapsulate(next.bounds);
        }

        shadowObject.SetActive(false);
        initialized = true;
    }
}
