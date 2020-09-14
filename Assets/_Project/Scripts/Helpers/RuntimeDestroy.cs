using UnityEngine;

public class RuntimeDestroy : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor)
        {
            Destroy(gameObject);
        }
    }
}
