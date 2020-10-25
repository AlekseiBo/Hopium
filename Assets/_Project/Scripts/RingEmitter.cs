using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RingEmitter : MonoBehaviour
{
    [SerializeField] GameObject baseRing;
    [SerializeField] float frequencyFromPI;
    [SerializeField] float speed;
    [SerializeField] float timeout;
    [SerializeField] float pulseBottom;

    protected Sequence sequence;
    protected LineRenderer line;

    GameObject prefab;
    Coroutine pulse;
    Coroutine emission;
    WaitForSeconds wait;

    protected void OnEnable()
    {
        var ring = baseRing.transform;
        prefab = Instantiate(baseRing, ring.position, ring.rotation, ring.parent);
        prefab.SetActive(false);

        line = baseRing.GetComponent<LineRenderer>();
        pulse = StartCoroutine(Pulse());

        wait = new WaitForSeconds(Mathf.PI / frequencyFromPI);
        emission = StartCoroutine(Emission());
    }

    protected void OnDisable()
    {
        StopCoroutine(emission);
        StopCoroutine(pulse);
    }

    void SetAnimation()
    {
        sequence = DOTween.Sequence();
        transform.SetLocalScale(0f);
        sequence.Append(transform.DOScale(1f, 1f));
        sequence.AppendInterval(2f);
        sequence.Append(transform.DOMoveY(5f, 10f));
        sequence.Join(DOTween.To(width => line.startWidth = width, line.startWidth, 0f, 10f));
    }

    protected IEnumerator Pulse()
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

    protected IEnumerator Emission()
    {
        while (true)
        {
            yield return wait;
            Emit();
        }
    }

    [ContextMenu("Emit")]
    protected void Emit()
    {
        var ring = baseRing.transform;
        var go = Instantiate(prefab, ring.position, ring.rotation, ring.parent);
        go.SetActive(true);
        StartCoroutine(FadeAway(go));
    }

    IEnumerator FadeAway(GameObject go)
    {
        float timer = 0f;
        var goLine = go.GetComponent<LineRenderer>();
        Color transparent = goLine.material.GetColor("_BaseColor");
        transparent.a = 0f;

        while (timer < 1f)
        {
            go.transform.localScale += go.transform.localScale * speed * Time.deltaTime;
            if (timer > 0.5f)
            {
                goLine.startWidth = Mathf.Lerp(line.startWidth, 0f, (timer - 0.5f) * 2f);
                goLine.material.SetColor("_BaseColor", Color.Lerp(Color.white, transparent, (timer - 0.5f) * 2f));
            }
            timer += Time.deltaTime / timeout;
            yield return null;
        }

        Destroy(go);
    }


}
