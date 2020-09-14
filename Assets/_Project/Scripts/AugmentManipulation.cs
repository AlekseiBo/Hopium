using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Lean.Touch;
using System.IO;
using System;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class AugmentManipulation : MonoBehaviour
{
    [SerializeField] float maxVelocity;

    private Camera cam;
    private Vector3 destination;
    private float grabDistance;
    private Vector2 grabOffset;
    private Quaternion grabRotation;
    private Rigidbody body;
    private bool cachedGravity;

    void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (body != null)
        {
            if (!body.isKinematic)
            {
                body.velocity = Vector3.ClampMagnitude((destination - body.position) * maxVelocity, maxVelocity);
            }
        }
    }

    void OnEnable()
    {
        LeanSelectable.OnSelectGlobal += OnSelect;
        LeanSelectable.OnDeselectGlobal += OnDeselect;
        LeanSelectable.OnSelectSetGlobal += HandleAugmentSelection;
    }


    void OnDisable()
    {
        LeanSelectable.OnSelectGlobal -= OnSelect;
        LeanSelectable.OnSelectSetGlobal -= HandleAugmentSelection;
    }

    void OnSelect(LeanSelectable selectable, LeanFinger finger)
    {
        body = selectable.GetComponent<Rigidbody>();

        if (body != null)
        {
            cachedGravity = body.useGravity;
            body.useGravity = false;
            body.angularVelocity = Vector3.zero;
        }

        grabRotation = Quaternion.Inverse(cam.transform.rotation) * selectable.transform.rotation;

        var fingerWorldPosition = cam.ScreenToWorldPoint(finger.ScreenPosition);
        grabDistance = (fingerWorldPosition - selectable.transform.position).magnitude - cam.nearClipPlane;

        var selectableScreenPosition = cam.WorldToScreenPoint(selectable.transform.position);
        grabOffset = (Vector2)selectableScreenPosition - finger.ScreenPosition;
    }

    void OnDeselect(LeanSelectable selectable)
    {
        if (body != null)
        {
            body.useGravity = cachedGravity;
            body = null;
        }
    }

    void HandleAugmentSelection(LeanSelectable selectable, LeanFinger finger)
    {
        Ray ray = cam.ScreenPointToRay(finger.ScreenPosition + grabOffset);

        if (body != null)
        {
            destination = ray.GetPoint(grabDistance);
            body.MoveRotation(cam.transform.rotation * grabRotation);

            if (body.isKinematic)
            {
                body.position = destination;
            }
        }
        else
        {
            selectable.transform.position = ray.GetPoint(grabDistance);
            selectable.transform.rotation = cam.transform.rotation * grabRotation;
        }
    }
}
