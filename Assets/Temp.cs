using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] private string filenamer;

    [ContextMenu("Screensot")]
    void Screensot()
    {
        ScreenCapture.CaptureScreenshot(filenamer);
    }
}
