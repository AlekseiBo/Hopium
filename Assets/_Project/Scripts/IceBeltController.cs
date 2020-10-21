using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBeltController : MonoBehaviour
{
    [SerializeField] GameObject[] iceRockPrefabs;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Transform iceRockContainer;
    [SerializeField] float radius;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] int segments;

    private void Start()
    {
        FormRing();
    }

    [ContextMenu("Build Belt")]
    void FormRing()
    {
        var segmentStep = (float)Mathf.PI * 2.0f / segments;
        var segmentAngle = 0f;

        for (int i = 0; i < segments; i++)
        {
            var x = radius * Mathf.Cos(segmentAngle);
            var z = radius * Mathf.Sin(segmentAngle);

            var pos = new Vector3(x, 0f, z);
            pos += iceRockContainer.transform.position;
            var rot = Random.rotation;
            var scale = Vector3.one * Random.Range(minSize, maxSize);

            var index = (i >= iceRockPrefabs.Length) ? (int)i % iceRockPrefabs.Length : i;

            var asteroid = Instantiate(asteroidPrefab, pos, rot, iceRockContainer);
            Instantiate(iceRockPrefabs[index], asteroid.transform);
            asteroid.transform.localScale = scale;
            asteroid.GetComponent<AsteroidController>().orbitRadius = radius;
            segmentAngle += segmentStep;
        }
    }
}
