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

    static GameManager instance;
    static List<GameObject> spawnedObjects = new List<GameObject>();
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private Camera cam;
    private Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        cam = Camera.main;
    }

    public void ClearScene()
    {
        spawnedObjects.DestroyContent();
    }

    public static void Spawn(GameObject obj) => instance.SpawnObject(obj);

    public static void Spawn() => instance.SpawnObject(instance.prefab);

    public void SpawnObject(GameObject obj)
    {
        var spawnRotation = Quaternion.LookRotation(cam.transform.forward.Flat() * -1f, Vector3.up);
        var spawnPosition = GetSpawnLocation();

        if (obj != null)
        {
            spawnedObject = Instantiate(obj, spawnPosition, spawnRotation);
            spawnedObjects.Add(spawnedObject);
        }
    }

    private Vector3 GetSpawnLocation()
    {
        Ray ray = cam.ViewportPointToRay(screenCenter);

        if (Application.isEditor)
        {
            int layerMask = LayerMask.GetMask("Plane");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                return hit.point;
            }
        }
        else
        {
            if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
            {
                return hits[0].pose.position;
            }
        }

        return cam.transform.position + cam.transform.forward.FlatNormilized() * spawnDistance - new Vector3(0f, spawnHeight, 0f);
    }
}
