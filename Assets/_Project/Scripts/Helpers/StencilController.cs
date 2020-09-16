using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StencilVisibility
{ Always, Never, Inside, Outside }

[ExecuteInEditMode,  RequireComponent(typeof(Renderer))]
public class StencilController : MonoBehaviour
{
    [SerializeField] StencilVisibility visibility;
    private Renderer render;

    private void Awake()
    {
        if (!Application.isEditor)
            Destroy(this);

        render = GetComponent<Renderer>();
    }

    private void OnValidate()
    {
        if (render != null)
            render.sharedMaterial.SetFloat("_StencilComp", (int)visibility);
    }

}
