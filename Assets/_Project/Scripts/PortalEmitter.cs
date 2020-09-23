using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEmitter : MonoBehaviour
{
    [SerializeField] GameObject frameObject;
    [SerializeField] bool forward;
    [SerializeField] float frequencyFromPI;
    [SerializeField] int burstSize;
    [SerializeField] float burstFrequency;
    [SerializeField] float speed;
    [SerializeField] float timeout;
    [SerializeField] float pulseBottom;


    private Coroutine emission;
    private WaitForSeconds wait;
    private GameObject prefab;

    private Coroutine pulse;
    private LineRenderer line;
    private float lineWidth;

    private void OnEnable()
    {
        var frame = frameObject.transform;
        prefab = Instantiate(frameObject, frame.position, frame.rotation, frame.parent);
        prefab.SetActive(false);

        line = frameObject.GetComponent<LineRenderer>();
        lineWidth = line.startWidth;
        pulse = StartCoroutine(Pulse());

        wait = new WaitForSeconds(Mathf.PI / frequencyFromPI);
        emission = StartCoroutine(Emission());
    }

    private void OnDisable()
    {
        StopCoroutine(emission);
        StopCoroutine(pulse);
    }

    IEnumerator Pulse()
    {
        var amp = 1 - pulseBottom;
        amp /= 2f;
        var level = 1 - amp;
        Color lineColor = Color.white;
        float alpha = 1f;

        while (true)
        {
            alpha = level + amp * Mathf.Cos(2f * Time.time * frequencyFromPI);
            lineColor.a = alpha;
            line.material.SetColor("_BaseColor", lineColor);
            yield return null;
        }
    }

    IEnumerator Emission()
    {
        while (true)
        {
            StartCoroutine(BurstEmmision());
            yield return wait;
        }
    }

    IEnumerator BurstEmmision()
    {
        for (int i = 0; i < burstSize; i++)
        {
            Emit();
            yield return new WaitForSeconds(burstFrequency);
        }
    }

    [ContextMenu("Emit")]
    void Emit()
    {
        var frame = frameObject.transform;
        var go = Instantiate(prefab, frame.position, frame.rotation, frame.parent);
        var fade = go.AddComponent<PortalFade>();
        fade.speed = speed;
        fade.timeout = timeout;
        fade.forward = forward;
        go.SetActive(true);
    }

}
