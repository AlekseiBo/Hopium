using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEmitter : MonoBehaviour
{
    [SerializeField] GameObject frameObject;
    [SerializeField] bool forward;
    [SerializeField] float frequencyFromPI;
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
        var amp = lineWidth - lineWidth * pulseBottom;
        amp /= 2f;
        var level = lineWidth - amp;

        while (true)
        {
            line.startWidth = level + amp * Mathf.Cos(2f * Time.time * frequencyFromPI);
            yield return null;
        }
    }

    IEnumerator Emission()
    {
        while (true)
        {
            Emit();
            yield return wait;
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
