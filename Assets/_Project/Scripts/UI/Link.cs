using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public void Open(string url) => Application.OpenURL(url);
}
