using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public static class ExtensionMethods
{
    public static void SetLocalScale(this Transform trans, float scale)
    {
        trans.localScale = new Vector3(scale, scale, scale);
    }

    public static Vector3 Flat(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    public static Vector3 FlatNormilized(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z).normalized;
    }

    public static void SetActive(this GameObject[] objects, bool active)
    {
        foreach (var item in objects)
        {
            item.SetActive(active);
        }
    }

    public static void Enabled(this Canvas[] objects, bool enable)
    {
        foreach (var item in objects)
        {
            item.enabled = enable;
        }
    }

    public static void Enabled(this Renderer[] objects, bool enable)
    {
        foreach (var item in objects)
        {
            item.enabled = enable;
        }
    }

    public static void Reset(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static string Unescape(this string str)
    {
        return System.Text.RegularExpressions.Regex.Unescape(str);
    }

    public static void DestroyContent(this List<GameObject> objects)
    {
        foreach (GameObject item in objects)
        {
            Object.Destroy(item);
        }

        objects.Clear();
    }

    public static void Clear(this Transform trans)
    {
        foreach (Transform item in trans)
        {
            Object.Destroy(item.gameObject);
        }
    }

}