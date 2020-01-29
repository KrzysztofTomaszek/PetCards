using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject page1Cam = null;
    [SerializeField] private GameObject page2Cam = null;
    [SerializeField] private GameObject page3Cam = null;
    [SerializeField] private GameObject page4Cam = null;
    private List<GameObject> cameraList = new List<GameObject>();
    private int currentCam;
    [SerializeField] private AudioClip buttonClickSfx;
    private void Start()
    {
        currentCam = 0;
        cameraList.Add(page1Cam);
        cameraList.Add(page2Cam);
        cameraList.Add(page3Cam);
        cameraList.Add(page4Cam);
        page1Cam.SetActive(true);
        page2Cam.SetActive(false);
        page3Cam.SetActive(false);
        page4Cam.SetActive(false);
    }

    public void OnClickNext()
    {
        if (currentCam < 3)
        {
            currentCam++;
            cameraList[currentCam].SetActive(true);
            cameraList[currentCam - 1].SetActive(false);
            AudioController.instance.PlaySfx(buttonClickSfx);
        }
        else
        {
            currentCam = 0;
            cameraList[currentCam].SetActive(true);
            cameraList[3].SetActive(false);
            AudioController.instance.PlaySfx(buttonClickSfx);
        }
    }

    public void OnClickBack()
    {
        if (currentCam > 0)
        {
            currentCam--;
            cameraList[currentCam].SetActive(true);
            cameraList[currentCam + 1].SetActive(false);
            AudioController.instance.PlaySfx(buttonClickSfx);
        }
        else
        {
            currentCam = 3;
            cameraList[currentCam].SetActive(true);
            cameraList[0].SetActive(false);
            AudioController.instance.PlaySfx(buttonClickSfx);
        }
    }
}
