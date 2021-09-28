using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����
/// <summary>
/// ������������ ������ ���� �������� �� �����
/// </summary>
public class World2Dobject : MonoBehaviour
{

    [SerializeField]
    float distToCamNow = 0;

    [SerializeField]
    SpriteRenderer mySprite;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    bool isAnimated = false;
    [SerializeField]
    float speedAnimation = 0.1f;
    float offsetAnimationTime = 0;

    [Header("SeeOnCamera")]
    [SerializeField]
    bool DontRotateY = false;
    [SerializeField]
    bool DontRotateZ = false;

    //��������� ��� �������� ������������ ��������� �� ������
    [Header("MoveOnAngle")]
    [SerializeField]
    Vector3 SpeedAngle = new Vector3();
    [SerializeField]
    Vector3 PosStart = new Vector3();
    [SerializeField]
    float angleStart = 80;
    [SerializeField]
    float angleNow = 0;

    [Header("MoveBasic")]
    [SerializeField]
    Vector3 speedBasic = new Vector3();
    [SerializeField]
    Vector2 DiapasonMoveX = new Vector2(-7, 7);
    [SerializeField]
    Vector2 DiapasonMoveY = new Vector2(10, 15);
    [SerializeField]
    Vector2 DiapasonSpawnZ = new Vector2(0, 90);
    [SerializeField]
    bool ChangeImageOutDiapasone = false;

    [Header("Randomize On Restart")]
    [SerializeField]
    bool RandomStartPosX = false;
    [SerializeField]
    bool RandomStartPosY = false;
    [SerializeField]
    bool RandomStartPosZ = false; 
    [SerializeField]
    Vector2 speedBasicRandomX = new Vector2();
    [SerializeField]
    Vector2 speedBasicRandomY = new Vector2();
    [SerializeField]
    Vector2 scaleRandom = new Vector2();
    [SerializeField]
    bool mirrorRandomX = false;
    [SerializeField]
    bool mirrorRandomY = false;



    bool randomize = false;


    private void Start()
    {

        //�������� ��������� �������
        float startx = transform.localPosition.x;
        float starty = transform.localPosition.y;
        float startz = transform.parent.localRotation.eulerAngles.x;

        if (RandomStartPosX || RandomStartPosY || RandomStartPosZ) {
            if (RandomStartPosX && DiapasonMoveX.x - DiapasonMoveX.y != 0) {
                startx = Random.Range(DiapasonMoveX.x, DiapasonMoveX.y);
            }
            if (RandomStartPosY && DiapasonMoveY.x - DiapasonMoveY.y != 0) {
                starty = Random.Range(DiapasonMoveY.x, DiapasonMoveY.y);
            }

            if (RandomStartPosZ) {
                startz = Random.Range(DiapasonSpawnZ.x, DiapasonSpawnZ.y);
                //�������
                Quaternion rotate = transform.parent.localRotation;
                rotate.eulerAngles = new Vector3(startz, rotate.eulerAngles.y, rotate.eulerAngles.z);
                transform.parent.localRotation = rotate;
            }
                        

            transform.localPosition = new Vector3(startx, starty, 0);
        }

        PosStart = new Vector3(startx, starty, startz);

        randomize = true;

        //������� ������� � ���������
        if (angleStart == 0)
        {
            Invoke("GetStartAngle", 0.1f);
        }
    }

    void Update()
    {
        distToCamNow = Vector3.Distance(MainComponents.MainCamera.transform.position, transform.position);

        //������� ������� � ������
        transform.LookAt(MainComponents.MainCamera.transform);
        //�������� ������� �� ��� Y
        if (DontRotateY) {
            Quaternion rotate = transform.localRotation;
            rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, 180, rotate.eulerAngles.z);
            transform.localRotation = rotate;
        }
        //�������� ������� �� ��� Z
        if (DontRotateZ)
        {
            Quaternion rotate = transform.localRotation;
            rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, rotate.eulerAngles.y, 0);
            transform.localRotation = rotate;
        }

        MoveAngle();
        MoveBasic();

        TestRestart();
        Animate();
    }

    //�������� ��������� �� ��������� �� ������
    void MoveAngle() {
        //���� ������� ��� ������� //���� �� ��������� ������� ��������, ���
        angleNow = GetAngleNow();

        //������ ���� ���� ����������� �����������
        if (speedBasic.x != 0 || speedBasic.y != 0 || speedBasic.z != 0) return;

        //���� �������� ������� �� �������� �� ��������� �� ���������
        if (SpeedAngle.x == 0 && SpeedAngle.y == 0 && SpeedAngle.z == 0) return;

        //��������� ��������
        float offSet = angleStart - angleNow;

        //������� ���������
        transform.localPosition = new Vector3(PosStart.x + SpeedAngle.x * offSet, PosStart.y + SpeedAngle.y * offSet, 0);

        //�������
        Quaternion rotate = transform.parent.localRotation;
        rotate.eulerAngles = new Vector3(PosStart.z, rotate.eulerAngles.y, rotate.eulerAngles.z);
        transform.parent.localRotation = rotate;


    }

    //���������� ������������� ��������
    void MoveBasic() {
        //������ ��������� ���������� ������
        if (speedBasic.x == 0 && speedBasic.y == 0 && speedBasic.z == 0) {
            return;
        }

        Vector3 positionNew = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        //����������� �� x
        if (speedBasic.x > 0) {
            positionNew.x += speedBasic.x * Time.unscaledDeltaTime;
            //���� ����� �� �������
            if (positionNew.x > DiapasonMoveX.y) {
                positionNew.x = DiapasonMoveX.x;
                randomize = true;
            }

        }
        else {
            positionNew.x += speedBasic.x * Time.unscaledDeltaTime;
            if (positionNew.x < DiapasonMoveX.x) {
                positionNew.x = DiapasonMoveX.y;
                randomize = true;
            }
        }
        //����������� �� y
        if (speedBasic.y > 0)
        {
            positionNew.y += speedBasic.y * Time.unscaledDeltaTime;
            if (positionNew.y > DiapasonMoveY.y) {
                positionNew.y = DiapasonMoveY.x;
                randomize = true;
            }
        }
        else {
            positionNew.y += speedBasic.y * Time.unscaledDeltaTime;
            if (positionNew.y < DiapasonMoveY.x) {
                positionNew.y = DiapasonMoveY.y;
                randomize = true;
            }
        }

        transform.localPosition = positionNew;
    }

    void Animate() {
        if (!isAnimated) {
            return;
        }

        //������� ������� �����
        int numSprite = (int)((Time.unscaledTime + offsetAnimationTime)/speedAnimation)%sprites.Length;

        mySprite.sprite = sprites[numSprite];
    }

    float GetAngleNow() {
        //���� ������� ��� ������� //���� �� ��������� ������� ��������, ���
        float angleNow = 0;
        int parentNum = 0;
        Transform parent = gameObject.transform.parent;
        bool found = false;

        //������� ���� ������� ������ ������� �������
        while (parentNum < 10)
        {

            //���� ��� ������ ����
            if (parent.gameObject.tag == "World")
            {
                //������ �� ����� ��������� ���
                found = true;
                //���������� ������ ����
                angleNow += WorldGenerateScene.main.rotationNow;
                break;
            }

            //��� �� ������ ���
            angleNow += parent.transform.localRotation.eulerAngles.x;

            //������� ���� ������ ������� ���
            if (parent.parent == null) {
                angleNow = 0;
                return angleNow;
            }

            //������������� �� ���������� ������
            parentNum++;
            parent = parent.parent;
        }

        if (!found) angleNow = 0;

        return angleNow;
    }

    //������������ ������ ����������� �� ������ �� ������������ ���� ���������
    void TestRestart() {
        if (!randomize) {
            return;
        }
        randomize = false;

        //������������� ������
        if (sprites != null && sprites.Length > 0) {
            int random = Random.Range(0, sprites.Length);

            if (sprites[random] != null) {
                mySprite.sprite = sprites[random];
            }
        }


        //������������� �������� ���� ����
        if (speedBasicRandomX.x != 0 || speedBasicRandomX.y != 0) {
            speedBasic.x = Random.Range(speedBasicRandomX.x, speedBasicRandomX.y);
        }
        if (speedBasicRandomY.x != 0 || speedBasicRandomY.y != 0) {
            speedBasic.y = Random.Range(speedBasicRandomY.x, speedBasicRandomY.y);
        }

        //������������� ������
        if (scaleRandom.x != 0 || scaleRandom.y != 0) {
            float random = Random.Range(scaleRandom.x, scaleRandom.y);
            transform.localScale = new Vector3(random, random, random);
        }

        //�������������
        if (mirrorRandomX) {
            if (Random.Range(0, 100) < 50)
            {
                mySprite.flipX = false;
            }
            else {
                mySprite.flipX = true;
            }
        }

        if (mirrorRandomY) {
            if (Random.Range(0, 100) < 50)
            {
                mySprite.flipY = false;
            }
            else {
                mySprite.flipY = true;
            }
        }

    }

    void GetStartAngle() {
        angleStart = GetAngleNow();
    }

}
