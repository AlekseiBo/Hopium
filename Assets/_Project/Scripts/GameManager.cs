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

    static GameManager instance;
    static List<GameObject> spawnedExperiments = new List<GameObject>();

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private Camera cam;

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
        spawnedExperiments.DestroyContent();
    }

    public static void Spawn(GameObject obj) => instance.SpawnObject(obj);

    public static void Spawn() => instance.SpawnObject(instance.prefab);

    public void SpawnObject(GameObject obj)
    {
        var spawnRotation = Quaternion.LookRotation(cam.transform.forward.Flat() * -1f, Vector3.up);
        var spawnPosition = cam.transform.position + cam.transform.forward.FlatNormilized() * spawnDistance;

        if (obj != null)
        {
            spawnedObject = Instantiate(obj, spawnPosition, spawnRotation);
            spawnedExperiments.Add(spawnedObject);
        }
    }
}
