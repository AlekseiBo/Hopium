using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float spawnDistance;
    [SerializeField] float spawnHeight;

    public static bool portalActivated;

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private Camera cam;
    private Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0.5f);

    static GameManager instance;
    static List<GameObject> spawnedObjects = new List<GameObject>();
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static List<ARRaycastHit> groundHits = new List<ARRaycastHit>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        cam = Camera.main;

        planeManager.planesChanged += PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs obj)
    {
        Spawn();
        planeManager.planesChanged -= PlanesChanged;
    }

    public void ClearScene()
    {
        spawnedObjects.DestroyContent();
    }

    public static void RegisterSpawnedObject(GameObject obj) => spawnedObjects.Add(obj);

    public static void Spawn(GameObject obj) => instance.SpawnObject(obj);

    public static void Spawn() => instance.SpawnObject(instance.prefab);

    public void SpawnObject(GameObject obj)
    {
        var spawnPose = GetSpawnLocation();
        var spawnRotation = spawnPose.rotation;
        var spawnPosition = spawnPose.position;

        if (obj != null)
        {
            spawnedObject = Instantiate(obj, spawnPosition, spawnRotation);
            spawnedObjects.Add(spawnedObject);
            portalActivated = true;
            planeManager.requestedDetectionMode = PlaneDetectionMode.None;
        }
    }

    private Pose GetSpawnLocation()
    {
        Ray ray = cam.ViewportPointToRay(screenCenter);

        Pose spawnPose = new Pose();
        spawnPose.rotation = Quaternion.LookRotation(cam.transform.forward.Flat() * -1f, Vector3.up);
        spawnPose.position = cam.transform.position + cam.transform.forward.FlatNormilized() * spawnDistance - new Vector3(0f, spawnHeight, 0f);

        if (Application.isEditor)
        {
            int layerMask = LayerMask.GetMask("Plane");
            if (Physics.Raycast(ray, out RaycastHit hit, spawnDistance, layerMask))
            {
                if (hit.collider.name == "Vertical")
                {
                    spawnPose.rotation = Quaternion.LookRotation(hit.normal.Flat(), Vector3.up);
                    spawnPose.position = FindGroundEditor(hit.point, layerMask);
                }
                else if (hit.collider.name == "Horizontal")
                {
                    spawnPose.position = hit.point;
                }
            }
            else
            {
                spawnPose.position = FindGroundEditor(ray.GetPoint(spawnDistance), layerMask);
            }
        }
        else
        {
            if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                if (hits[0].distance < spawnDistance)
                {
                    var trackable = planeManager.GetPlane(hits[0].trackableId);
                    if (trackable.alignment == PlaneAlignment.Vertical)
                    {
                        spawnPose.rotation = Quaternion.LookRotation(hits[0].pose.rotation.eulerAngles.Flat() * -1f, Vector3.up);
                        spawnPose.position = FindGround(hits[0].pose.position);
                    }
                    else
                    {
                        spawnPose.position = hits[0].pose.position;
                    }
                }
                else
                {
                    spawnPose.position = FindGround(ray.GetPoint(spawnDistance));
                }
            }
            else
            {
                spawnPose.position = FindGround(ray.GetPoint(spawnDistance));
            }
        }

        return spawnPose;

        Vector3 FindGround(Vector3 point)
        {
            var groundRay = new Ray(point, Vector3.down);

            if (raycastManager.Raycast(groundRay, groundHits, TrackableType.PlaneWithinInfinity))
            {
                foreach (var hit in groundHits)
                {
                    var trackable = planeManager.GetPlane(hit.trackableId);
                    if (trackable.alignment == PlaneAlignment.HorizontalUp)
                    {
                        return hit.pose.position;
                    }
                }

                return new Vector3(point.x, cam.transform.position.y - spawnHeight, point.z);
            }
            else
            {
                return new Vector3(point.x, cam.transform.position.y - spawnHeight, point.z);
            }
        }

        Vector3 FindGroundEditor(Vector3 point, int layerMask)
        {
            var groundRay = new Ray(point, Vector3.down);

            if (Physics.Raycast(groundRay, out RaycastHit groundHit, Mathf.Infinity, layerMask))
            {
                if (groundHit.collider.name == "Horizontal")
                {
                    return groundHit.point;
                }
                else
                {
                    return new Vector3(point.x, cam.transform.position.y - spawnHeight, point.z);
                }
            }
            else
            {
                return new Vector3(point.x, cam.transform.position.y - spawnHeight, point.z);
            }
        }
    }

    private void OnDestroy()
    {
        planeManager.planesChanged -= PlanesChanged;
    }
}
