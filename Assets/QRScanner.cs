using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;

public class QRScanner : MonoBehaviour
{
    private bool camAvailabe;
    private WebCamTexture backCam;
    private Texture defaultBackGround;
    string QrCode = string.Empty;
    public RawImage backGround;
    public AspectRatioFitter fit;
    public GameObject buttonScan;   
    public GameObject ResultBox; 
    public TextMeshProUGUI textBox;

    void Start()
    {
        SetUpCamera();
    }

    private void SetUpCamera()
    {
        defaultBackGround = backGround.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No Camera detected");
            camAvailabe = false;
            return;
        }

        foreach (var d in devices)
        {
            if(!d.isFrontFacing)
            {
                backCam = new WebCamTexture(d.name, Screen.width, Screen.height);
            }            
        }

        if(backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCam.Play();
        backGround.texture = backCam;   

        camAvailabe = true;
    }

    private void Update() 
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if(!camAvailabe) return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        backGround.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        backGround.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
   
    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        var snap = new Texture2D(backCam.width, backCam.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {            
            try
            {
                snap.SetPixels32(backCam.GetPixels32());
                var Result = barCodeReader.Decode(snap.GetRawTextureData(), backCam.width, backCam.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result != null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        ResultBox.SetActive(true);
                        textBox.text = QrCode;
                        break;
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
    }
    
    public void OnScanClick()
    {
        StartCoroutine(GetQRCode());
        buttonScan.SetActive(false);
    }

    public void QuitTool()
    {
        Application.Quit();
    }

    public void EnterLink()
    {
        Application.OpenURL(QrCode);
        QuitTool();
    }

    public void Cancel()
    {
        buttonScan.SetActive(true);        
        ResultBox.SetActive(false);
        QrCode = string.Empty;
    }
}
