using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private WebCamTexture webcam;

    void Start()
    {
        webcam = new WebCamTexture();                          
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
