using System.Collections;
using EasyMobile;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [SerializeField] Canvas hideCanvas;
    [SerializeField] GameObject captureButton;
    [SerializeField] string filename;
    [SerializeField] string message;

    public void Take()
    {
        StartCoroutine(OneStepSharing());
    }

    IEnumerator OneStepSharing()
    {
        hideCanvas.enabled = false;
        yield return new WaitForEndOfFrame();

        Sharing.ShareScreenshot(filename, message);
        hideCanvas.enabled = true;
    }
}
