using UnityEngine;
using UnityEngine.XR.ARFoundation;
using DG.Tweening;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(Renderer))]
public class FadePlane : MonoBehaviour
{
    [SerializeField] float blinkTime = 0.25f;
    [SerializeField] float fadeValue = 0.7f;

    const float boundaryTimeout = 1f;

    ARPlane plane;
    Renderer planeRenderer;
    float timer = 0;
    bool updatingPlane = false;

    private void OnEnable()
    {
        planeRenderer = GetComponent<Renderer>();
        plane = GetComponent<ARPlane>();

        plane.boundaryChanged += PlaneOnBoundaryChanged;
    }

    private void OnDisable()
    {
        plane.boundaryChanged -= PlaneOnBoundaryChanged;
    }

    void Update()
    {
        if (updatingPlane)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                planeRenderer.material.DOFade(0f, "_TexTintColor", blinkTime);
                updatingPlane = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RevealPlane();
    }

    private void RevealPlane()
    {
        planeRenderer.material.DOFade(fadeValue, "_TexTintColor", blinkTime);
        updatingPlane = true;
        timer = blinkTime;
    }

    private void PlaneOnBoundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        planeRenderer.material.DOFade(fadeValue, "_TexTintColor", blinkTime).OnComplete(() =>
        {
            updatingPlane = true;
            timer = blinkTime;
        });
    }
}