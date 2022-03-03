using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingParticleObj : MonoBehaviour
{

    [SerializeField]
    RectTransform rectSpawn;
    [SerializeField]
    RectTransform rectCenter;
    [SerializeField]
    GameObject sizeObj;

    [SerializeField]
    Vector2 PositionNeed;
    [SerializeField]
    Image sprite;
    [SerializeField]
    RawImage image;


    Vector2 vectorMove = new Vector2(0,0); //������ �������� �������
    Vector2 sizeStart = new Vector2(100,100);
    float speedNow = 0; //������� ��������
    float distStart = 0; //��������� ��������� �� ����, ����� �������� ����� �� ���������� ��� �����������
    float speedRotateNow = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateTransform();
    }



     public void IniFlyingParticleData(GameObject ParentObj, Texture image, Sprite sprite, Vector2 sizeimage, RectTransform rectStart, RectTransform rectTarget) {
        //�������� ������ �������� ������������
        if (gameObject.transform.parent != ParentObj.transform)
            gameObject.transform.parent = ParentObj.transform;


        //������������� ����������� //������ � ����������
        if (sprite != null)
        {
            this.sprite.sprite = sprite;
            this.sprite.type = Image.Type.Sliced;

            this.sprite.gameObject.SetActive(true);
            this.image.gameObject.SetActive(false);
        }
        else if (image != null)
        {
            this.image.texture = image;

            this.image.gameObject.SetActive(true);
            this.sprite.gameObject.SetActive(false);
        }

        //������������� ��������� ������� � ������
        Vector2 startPos = new Vector2(rectStart.position.x + sizeimage.x, rectStart.position.y + sizeimage.y/2);
        rectSpawn.position = startPos;
        sizeStart = sizeimage * 100;
        rectSpawn.sizeDelta = sizeStart;

        //������� ���� ���� ���������
        PositionNeed = rectTarget.position;

        //������ ��������� ����� ������� ���������� � �������
        distStart = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y), 
            new Vector2(PositionNeed.x, PositionNeed.y));

        //������������� ��������� ������
        vectorMove = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vectorMove.Normalize();

        //������������� ��������� ��������
        speedRotateNow = Random.Range(1f, 3f);
        if(Random.Range(0,100) < 50)
            speedRotateNow *= -1;

    }

    void updateTransform() {

        //������ ������� ��������� �� ����
        float distNow = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y),
            new Vector2(PositionNeed.x, PositionNeed.y));

        //���� ��������� ������ ��������
        if (distNow > distStart/2) {
            //���������� ��������
            speedNow += Time.deltaTime * 10;
        }
        else {
            //��������� ��������, ��� ����� ��� ������� �� ����� �������������
            speedNow -= speedNow * Time.deltaTime;
            //���� �������� ����� ������ ������� �� �����
            if (speedNow < Time.deltaTime) speedNow = Time.deltaTime;
        }

        //������ ������ �� ����
        Vector2 vectorTarget = new Vector2(
            PositionNeed.x - rectCenter.position.x, 
            PositionNeed.y - rectCenter.position.y);

        vectorTarget.Normalize();

        //��������� ����� ������ ������
        float lerpCoof = Time.deltaTime * speedNow;
        if (lerpCoof > 1) lerpCoof = 1;
        vectorMove = Vector2.Lerp(vectorMove, vectorTarget, lerpCoof);
        

        //������� ����� ���������
        float xNew = rectCenter.position.x + vectorMove.x * speedNow; //��x
        float yNew = rectCenter.position.y + vectorMove.y * speedNow; //��y

        //���������� ������ �� �������� ���������
        rectCenter.position = new Vector3(xNew, yNew);

        distNow = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y),
            new Vector2(PositionNeed.x, PositionNeed.y));


        ////////////////////////////////////////////////////////
        //�������� � ��������
        speedRotateNow += speedRotateNow * Time.deltaTime * 0.5f;
        rectCenter.Rotate(0,0, speedRotateNow);

        //��������� ������
        float scale = 1;
        //���� ������� ��������� ������ ��������
        if (distNow < distStart/4) {
            scale = distNow / (distStart / 4);
        }
        //��������� ����� ������ ������ ��� �� ������ ��� ��� ��� ���
        if(sizeObj.transform.localScale.x >= scale)
            sizeObj.transform.localScale = new Vector3(scale, scale, scale); //sizeStart/2 * scale;

        //���� ��������� ��� ������� ��������
        if (distNow > 0.1f) return;

        //�������
        Destroy(gameObject);
    }
}
