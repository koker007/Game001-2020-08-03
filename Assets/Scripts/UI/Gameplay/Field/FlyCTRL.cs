using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������������ ����� �������, �������� ��������
/// </summary>
public class FlyCTRL : MonoBehaviour
{

    static public List<FlyCTRL> flyCTRLs = new List<FlyCTRL>();

    float DistMinForHitTarget = 0.25f;
    [SerializeField]
    RawImage image;

    [SerializeField]
    GameFieldCTRL myField;

    [SerializeField]
    Vector2 PivotTarget;
    [SerializeField]
    public CellCTRL CellTarget;

    [SerializeField]
    GameObject RotateVectorMove;
    [SerializeField]
    GameObject RotateObj;

    [SerializeField]
    float SpeedRotate = 0;
    [SerializeField]
    float SpeedMove = 0;

    RectTransform myRect;
    Vector2 PivotStart;

    [Header("Test Data")]
    [SerializeField]
    float rotNow;
    [SerializeField]
    float rotNeed;
    [SerializeField]
    float raznica;
    [SerializeField]
    Vector2 vectorMove;
    [SerializeField]
    float radianAngleNow;

    //��� �� ����������� �������
    [SerializeField]
    bool Activated = false;
    float ActivatedTime = 0;

    bool partnerHave = false;
    CellInternalObject.Type partnerType; //������ ��������
    GameFieldCTRL.Combination comb; //������ ����������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isInicialized = false;
    public void inicialize(CellCTRL CellStart, CellCTRL CellTargetFunc, CellInternalObject partner, GameFieldCTRL.Combination combFunc) {

        //����� ���� ������ ��� ��������������������
        if (isInicialized) return;

        //���� ������� ����
        if (partner != null)
        {
            partnerHave = true;
            partnerType = partner.type;
        }
        comb = combFunc;

        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-CellStart.pos.x, -CellStart.pos.y);

        myField = CellStart.myField;
        CellTarget = CellTargetFunc;
        PivotTarget = new Vector2(-CellTarget.pos.x, -CellTarget.pos.y);

        flyCTRLs.Add(this);

        isInicialized = true;
    }

    void RandomTarget() {
        PivotTarget = new Vector2Int(Random.Range(0, -5), Random.Range(0,-5));
    }

    // Update is called once per frame
    void Update()
    {
        CalcTransform();
    }

    void CalcTransform() {
        //���� �� ����������������, �������
        if (!isInicialized) return;

        float distToTarget = Vector2.Distance(PivotTarget, myRect.pivot);

        Rotate();
        Move();
        Damage();

        //����������� �� ��������
        Destroying();

        void Rotate() {

            if (distToTarget < DistMinForHitTarget) { return; }


            //����������� � ���� ������������ �������
            Vector2 vectorTarget = PivotTarget - myRect.pivot;
            Vector2 vectorTargetNormalized = vectorTarget.normalized;

            //���� ����������
            //float radianSin = Mathf.Asin(vectorTargetNormalized.x);
            //�� ������ ��� �������� ������� ��� �� ������� ���������� ���������
            float radianCos = Mathf.Acos(vectorTargetNormalized.y);
            //�� ������ ���� ������������� ��� ������������� X ������ ������������� ��� ������������� ������ �����

            if (vectorTargetNormalized.x < 0)
            {
                radianCos = Mathf.PI + (Mathf.PI - radianCos);
            }
            //����� ���� � ��������
            //����� ������ �� ����
            //������ ����� ��� ��� ��������

            //��������� �� ������ � �������
            float gradTarget = radianCos / Calculate.PIinOnegrad;
            //�������� ��� �� ������� ����� ��������� ������ ����� �� ������� �� ����

            //���� ������� � ���� ������ ��� �� 180 ������ ���������� ������ 360 ���� ��������� ����� ��������
            raznica = gradTarget - RotateVectorMove.transform.localRotation.eulerAngles.z;

            //���� ����� ������� ����� ������ ��� �� 180 �������� �� ������� ��� ���� �� ���� ����.
            if (raznica > 180) {
                gradTarget = gradTarget - 360;
            }
            else if (raznica < -180) {
                gradTarget = gradTarget + 360;
            }

            //���������� ������� ��������
            SpeedRotate += Time.deltaTime * 60 * (6/distToTarget); //� ������� ������� �������� � ����������� �� ���������
            //��������� ��������� ������� ��������
            float coofRotSpeed = SpeedRotate * SpeedMove * Time.deltaTime;
            //���� ��������� ���� ������ 1 ������������ � 1. 1 ��� ������������ �������� � ������� ����;
            if (coofRotSpeed > 1)
                coofRotSpeed = 1;
            


            //�������� ��� ��� �������� �����
            float gradNow = RotateVectorMove.transform.localRotation.eulerAngles.z + (gradTarget - RotateVectorMove.transform.localRotation.eulerAngles.z) * coofRotSpeed;

            //

            //��������� ����� �������� �� ������
            Quaternion rotateVectorNew = RotateVectorMove.transform.rotation;
            rotateVectorNew.eulerAngles = new Vector3(0,0, gradNow);
            RotateVectorMove.transform.rotation = rotateVectorNew;

            //��������� �������� ����� �������� �� ���������� ����� ������������ �������
            Quaternion rotateObjNew = RotateVectorMove.transform.rotation;
            rotateObjNew.eulerAngles = new Vector3(0, 0, (-gradNow) + 180);
            RotateObj.transform.rotation = rotateObjNew;

            rotNeed = gradTarget;
            rotNow = RotateVectorMove.transform.localRotation.eulerAngles.z;

            //

        }
        void Move() {

            if (distToTarget < DistMinForHitTarget) { return; }

            //�������� ������ ����������� �������
            //������ ��� ��������
            rotNow = RotateVectorMove.transform.localRotation.eulerAngles.z;

            //��������� ��� � �������
            radianAngleNow = RotateVectorMove.transform.localRotation.eulerAngles.z * Calculate.PIinOnegrad;
            //������ ������ �������� � ���������� � ����������� �� ���������
            vectorMove = new Vector2(Mathf.Sin(radianAngleNow), Mathf.Cos(radianAngleNow));

            //���������� ������� ������� ��������
            if (distToTarget > 0.75f) {
                SpeedMove += (0.1f - SpeedMove) * Time.deltaTime * 2f;
            }
            //�������� � ����������� � �����
            else {
                SpeedMove += (0.01f - SpeedMove) * Time.deltaTime * 15f;
            }
            Vector2 moving = vectorMove * SpeedMove;
            //������ ����� �������
            myRect.pivot = new Vector2(myRect.pivot.x + moving.x, myRect.pivot.y + moving.y);
            

        }

        void Damage() {
            //�������, ���� ��� ������������ ���� ��������� ������ ��� �����
            if (Activated || distToTarget >= DistMinForHitTarget) return;

            //������ ������� ����� ������� ����
            CellTarget.Damage(null, comb, false);

            if (partnerHave) {
                //���� ������� �������� ���� �����
                if (partnerType == CellInternalObject.Type.bomb) {
                    myField.CreateBomb(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    CellTarget.cellInternal.activateNum = 1; //����� � �������� ������������ ����� 1 ���
                    CellTarget.cellInternal.activateCount = 2;
                    CellTarget.Damage();
                }
                //���� ������� �������� ���� ������ ��������������
                else if (partnerType == CellInternalObject.Type.rocketHorizontal) {
                    myField.CreateRocketHorizontal(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    CellTarget.Damage();
                }
                //���� ������� �������� ���� ������ ������������
                else if (partnerType == CellInternalObject.Type.rocketVertical) {
                    myField.CreateRocketVertical(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    CellTarget.Damage();
                }
            }

            Activated = true;
            //ActivatedTime = Time.unscaledTime; //����� ���������
        }

        void Destroying() {
            if (Activated)
            {
                float alpha = image.color.a - Time.deltaTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

                if (alpha < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    //����������
    ~FlyCTRL() {

        reCalcList();


        void reCalcList() {
            List<FlyCTRL> flyCTRLsNew = new List<FlyCTRL>();

            //������� ����� ������ ��������� ������ ���� � ������ ����������
            foreach (FlyCTRL flyCTRL in flyCTRLs) {
                
                if (flyCTRL != null && flyCTRL != this) {
                    flyCTRLsNew.Add(flyCTRL);
                }
            }
        }
    }
}
