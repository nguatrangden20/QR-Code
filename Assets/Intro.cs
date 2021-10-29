using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private Image backGround;
    public Sprite backGround1;
    public Sprite backGround2;
    private WebCamTexture webcam;

    void Start()
    {
        backGround = gameObject.GetComponent<Image>();
        webcam = new WebCamTexture();                          
    }

    void Update()
    {
        if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            backGround.sprite = backGround1;
        }else
        {
            backGround.sprite = backGround2;
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
