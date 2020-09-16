using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFade : MonoBehaviour
{
    [HideInInspector] public float timeout;
    [HideInInspector] public float speed;
    [HideInInspector] public bool forward;

    private LineRenderer line;
    private float lineWidth;
    private Transform target;
    private Vector3 direction;

    private void Start()
    {
        if (!forward)
            target = Camera.main.transform;

        line = GetComponent<LineRenderer>();
        lineWidth = line.startWidth;
        transform.parent = null;
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        float timer = 0f;

        if (forward)
            direction = -transform.forward;
        else
            direction = (target.position - transform.position).normalized;

        while (timer < 1f)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            line.startWidth = Mathf.Lerp(lineWidth, 0f, timer);
            timer += Time.deltaTime / timeout;
            yield return null;
        }

        Destroy(gameObject);
    }

    //private Material material;
    //private Color solid;
    //private Color transparent;
    //private Color emission;
    //private Color zeroEmission;
    //private float intensity;

    //material = GetComponent<LineRenderer>().material;
    //solid = material.color;
    //transparent = solid;
    //transparent.a = 0f;

    //emission = material.GetColor("_EmissionColor");
    //intensity = (emission.r + emission.g + emission.b) / 3f;
    //var factor = 1f / intensity;
    //zeroEmission = new Color(emission.r * factor, emission.g * factor, emission.b * factor);
    //zeroEmission = emission;
    //zeroEmission.a = 0f;


    //material.color = Color.Lerp(solid, transparent, timer);
    //material.SetColor("_EmissionColor", Color.Lerp(emission, zeroEmission, timer));
}
