using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RingController : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] int segments;

    private Sequence sequence;
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        SetAnimation();
        FormRing();
    }

    // private void Start() => PlaySequence();

    private void OnValidate() => FormRing();

    public void PlaySequence()
    {
        if (!sequence.playedOnce)
            sequence.Play();
    }

    private void SetAnimation()
    {
        sequence = DOTween.Sequence();
        transform.SetLocalScale(0f);
        sequence.Append(transform.DOScale(1f, 1f));
        sequence.AppendInterval(2f);
        sequence.Append(transform.DOMoveY(5f, 10f));
        sequence.Join(DOTween.To(width => line.startWidth = width, line.startWidth, 0f, 10f));
    }

    [ContextMenu("Form ring")]
    private void FormRing()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        line.positionCount = segments;
        var segmentStep = (float)Mathf.PI * 2.0f / segments;
        var segmentAngle = 0f;

        for (int i = 0; i < line.positionCount; i++)
        {
            var x = radius * Mathf.Cos(segmentAngle);
            var z = radius * Mathf.Sin(segmentAngle);

            var pos = new Vector3(x, 0f, z);
            line.SetPosition(i, pos);
            segmentAngle += segmentStep;
        }
    }
}
