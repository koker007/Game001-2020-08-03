using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorIni : MonoBehaviour
{
    [SerializeField]
    Vector2Int sizeTexture = new Vector2Int(100, 100);
    [SerializeField]
    DoctorScene.Emotions emotion = DoctorScene.Emotions.Normal;
    [SerializeField]
    float CamAngle = 20;
    [SerializeField]
    Vector2 CamOffset = new Vector2(0,0);

    [Header("Other")]
    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    RenderTexture render;

    void iniRawImage() {

        rawImage = GetComponent<RawImage>();

        if (rawImage == null) {
            rawImage = GetComponentInChildren<RawImage>();
        }

        rawImage.texture = DoctorScene.GetRender(sizeTexture.x, sizeTexture.y);
        sizeTextureOld = sizeTexture;

        //Переместить камеру в связи с заданными параметрами
        DoctorScene.SetViewAngle(CamAngle);
        DoctorScene.SetCamOffset(CamOffset);
        DoctorScene.SetEmotion(emotion);
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    Vector2Int sizeTextureOld = new Vector2Int(0,0);
    void ReSize() {
        if (sizeTexture == sizeTextureOld &&
            emotion == DoctorScene.main.emotion)
            return;

        iniRawImage();

    }

    // Update is called once per frame
    void Update()
    {
        ReSize();
    }

    private void OnEnable()
    {
        iniRawImage();
        if(DoctorScene.main)
            DoctorScene.main.CameraSetActive(true);
    }

    private void OnDisable()
    {
        if(DoctorScene.main)
            DoctorScene.main.CameraSetActive(false);
    }

}
