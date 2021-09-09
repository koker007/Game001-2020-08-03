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
        TestTarget();
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


            if (partnerHave)
            {
                //���� ������� �������� ���� �����
                if (partnerType == CellInternalObject.Type.bomb)
                {
                    BombDamage();
                }
                //���� ������� �������� ���� ������ ��������������
                else if (partnerType == CellInternalObject.Type.rocketHorizontal)
                {
                    //myField.CreateRocketHorizontal(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    //CellTarget.Damage();

                    CellTarget.explosion = new CellCTRL.Explosion(true, true, false, false, 0.05f, comb);
                    CellTarget.BufferCombination = comb;
                    CellTarget.BufferNearDamage = false;
                    CellTarget.ExplosionBoomInvoke(CellTarget.explosion);
                }
                //���� ������� �������� ���� ������ ������������
                else if (partnerType == CellInternalObject.Type.rocketVertical)
                {
                    CellTarget.explosion = new CellCTRL.Explosion(false, false, true, true, 0.05f, comb);
                    CellTarget.BufferCombination = comb;
                    CellTarget.BufferNearDamage = false;
                    CellTarget.ExplosionBoomInvoke(CellTarget.explosion);

                    //myField.CreateRocketVertical(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    //CellTarget.Damage();
                }
            }
            else {
                //������� ����� ������� ����
                CellTarget.Damage(null, comb, false);
            }

            Activated = true;
            //ActivatedTime = Time.unscaledTime; //����� ���������

            void BombDamage() {
                //������� ���� �� ������� 3�3

                CellTarget.explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, comb);
                CellTarget.BufferCombination = comb;
                CellTarget.BufferNearDamage = false;
                CellTarget.ExplosionBoomInvoke(CellTarget.explosion);

                int radius = 1;
                //���������� ���� 3 �� 3
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        int fieldPosX = CellTarget.pos.x + x;
                        int fieldPosY = CellTarget.pos.y + y;
                        //���� ����� �� ������� ����� ��� ���� ������ ����
                        if (fieldPosX < 0 || fieldPosX >= myField.cellCTRLs.GetLength(0) ||
                            fieldPosY < 0 || fieldPosY >= myField.cellCTRLs.GetLength(1) ||
                            !myField.cellCTRLs[fieldPosX, fieldPosY]
                            )
                        {
                            continue;
                        }

                        //������� ����� �������� ������ ���� ������
                        float time = Vector2.Distance(new Vector2(), new Vector2(x, y)) * 0.05f;
                        //
                        myField.cellCTRLs[fieldPosX, fieldPosY].explosion = new CellCTRL.Explosion(false, false, false, false, time, comb);
                        myField.cellCTRLs[fieldPosX, fieldPosY].BufferCombination = comb;
                        myField.cellCTRLs[fieldPosX, fieldPosY].BufferNearDamage = false;
                        myField.cellCTRLs[fieldPosX, fieldPosY].ExplosionBoomInvoke(CellTarget.explosion);
                    }
                }
            }
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

    bool isRecalcTarget = false;
    void TestTarget() {
        //���� ��������� ��� ����������� ������� ���, ��� ��� �����, ��� ��� 
        if (!isRecalcTarget && isBadTarget()) {
            CellTarget = getNewCellPrioryty();
            PivotTarget = new Vector2(-CellTarget.pos.x, -CellTarget.pos.y);
        }

        bool isBadTarget() {
            bool result = false;

            if (!Activated && CellTarget.cellInternal == null && CellTarget.BlockingMove == 0 && CellTarget.mold == 0 && CellTarget.rock == 0) {
                result = true;
            }

            return result;
        }

        CellCTRL getNewCellPrioryty() {
            CellCTRL result = null;

            isRecalcTarget = true;

            //���� ������ � ������� ��� ����� �� �����
            foreach (CellCTRL cellPriority in myField.cellsPriority)
            {
                //� ����� ������ ������ ������� ����� ��� �������, � �������� ������ ���� ����
                result = cellPriority;

                //���� ����� ��� ������ � ������ ����� �� ��������� ������� � ������������� �����
                bool found = false;
                foreach (FlyCTRL fly in flyCTRLs)
                {
                    if (fly.CellTarget == cellPriority)
                    {
                        found = true;
                        break;
                    }
                }

                //���� ��������� ������� � �� ����� ������ � ������, ������ ��� �� ��� ����� ������� � �������� ����� ����
                if (!found)
                {
                    break;
                }


            }


            return result;


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
