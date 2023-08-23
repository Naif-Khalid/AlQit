using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetButtonDown("1Key"))
        {
            cam1.SetActive(true);
            cam2.SetActive(false); 
        }

        if (Input.GetButtonDown("2Key"))
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
            screenshotCount++;
            ScreenCapture.CaptureScreenshot("Screenshots/camswitch" + screenshotCount + ".png");
        }
    }
}