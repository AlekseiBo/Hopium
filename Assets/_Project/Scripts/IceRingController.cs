using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRingController : MonoBehaviour
{
    [SerializeField] GameObject[] iceRockPrefabs;
    [SerializeField] Transform iceRockContainer;
    [SerializeField] float radius;
    [SerializeField] int segments;

    private void Start()
    {
        FormRing();
    }

    [ContextMenu("Build Ring")]
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
            var rot = Quaternion.identity;

            var index = (i >= iceRockPrefabs.Length) ? (int)i % iceRockPrefabs.Length : i;

            Instantiate(iceRockPrefabs[index], pos, rot, iceRockContainer);
            segmentAngle += segmentStep;
        }
    }
}
