using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Контролирует полет объекта, например иньекции
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

    //Был ли активирован самолет
    [SerializeField]
    bool Activated = false;
    float ActivatedTime = 0;

    bool partnerHave = false;
    CellInternalObject.Type partnerType; //Данные партнера
    GameFieldCTRL.Combination comb; //Данные комбинации

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isInicialized = false;
    public void inicialize(CellCTRL CellStart, CellCTRL CellTargetFunc, CellInternalObject partner, GameFieldCTRL.Combination combFunc) {

        //Выйти если раннее уже проинициализированно
        if (isInicialized) return;

        //Если партнер есть
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
        //Если не инициализировано, выходим
        if (!isInicialized) return;

        float distToTarget = Vector2.Distance(PivotTarget, myRect.pivot);

        Rotate();
        Move();
        Damage();

        //Уничтожение со временем
        Destroying();

        void Rotate() {

            if (distToTarget < DistMinForHitTarget) { return; }


            //Направление к цели относительно объекта
            Vector2 vectorTarget = PivotTarget - myRect.pivot;
            Vector2 vectorTargetNormalized = vectorTarget.normalized;

            //Ищем арккосинус
            //float radianSin = Mathf.Asin(vectorTargetNormalized.x);
            //На основе арк косинуса находим угл на который наобходимо повернуть
            float radianCos = Mathf.Acos(vectorTargetNormalized.y);
            //на основе того отрицательный или положительный X узнаем положительный или отрицательный градус нужен

            if (vectorTargetNormalized.x < 0)
            {
                radianCos = Mathf.PI + (Mathf.PI - radianCos);
            }
            //Нашли угол в радианах
            //Нашли вектор до цели
            //Узнаем какой это угл поворота

            //Переводим из радиан в градусы
            float gradTarget = radianCos / Calculate.PIinOnegrad;
            //Получили угл на который нужно повернуть обьект чтобы он смотрел на цель

            //Если разница в угле больше чем на 180 значит необходимо отнять 360 либо прибавить чтобы уравнять
            raznica = gradTarget - RotateVectorMove.transform.localRotation.eulerAngles.z;

            //Если вдруг разница стала больше чем на 180 градусов то смещаем угл цели на один круг.
            if (raznica > 180) {
                gradTarget = gradTarget - 360;
            }
            else if (raznica < -180) {
                gradTarget = gradTarget + 360;
            }

            //Прибавялем угловую скорость
            SpeedRotate += Time.deltaTime * 60 * (6/distToTarget); //В скобках угловая скорость в зависимости от растояния
            //вычисляем коофицент угловой скорости
            float coofRotSpeed = SpeedRotate * SpeedMove * Time.deltaTime;
            //Если коофицент стал больше 1 приравниваем к 1. 1 это моментальное вращение в сторону цели;
            if (coofRotSpeed > 1)
                coofRotSpeed = 1;
            


            //Получаем угл для текущего кадра
            float gradNow = RotateVectorMove.transform.localRotation.eulerAngles.z + (gradTarget - RotateVectorMove.transform.localRotation.eulerAngles.z) * coofRotSpeed;

            //

            //Применяем новое вращение на вектор
            Quaternion rotateVectorNew = RotateVectorMove.transform.rotation;
            rotateVectorNew.eulerAngles = new Vector3(0,0, gradNow);
            RotateVectorMove.transform.rotation = rotateVectorNew;

            //Применяем прменяем новое вращение на визуальную часть относительно вектора
            Quaternion rotateObjNew = RotateVectorMove.transform.rotation;
            rotateObjNew.eulerAngles = new Vector3(0, 0, (-gradNow) + 180);
            RotateObj.transform.rotation = rotateObjNew;

            rotNeed = gradTarget;
            rotNow = RotateVectorMove.transform.localRotation.eulerAngles.z;

            //

        }
        void Move() {

            if (distToTarget < DistMinForHitTarget) { return; }

            //Получаем вектор направления взгляда
            //узнаем угл поворота
            rotNow = RotateVectorMove.transform.localRotation.eulerAngles.z;

            //Переводим угл в радианы
            radianAngleNow = RotateVectorMove.transform.localRotation.eulerAngles.z * Calculate.PIinOnegrad;
            //Узнаем вертор движения и перемещаем в соответсвии со скоростью
            vectorMove = new Vector2(Mathf.Sin(radianAngleNow), Mathf.Cos(radianAngleNow));

            //Нормальное быстрое далекое движение
            if (distToTarget > 0.75f) {
                SpeedMove += (0.1f - SpeedMove) * Time.deltaTime * 2f;
            }
            //Движение с замедлением в близи
            else {
                SpeedMove += (0.01f - SpeedMove) * Time.deltaTime * 15f;
            }
            Vector2 moving = vectorMove * SpeedMove;
            //Расчет новой позиции
            myRect.pivot = new Vector2(myRect.pivot.x + moving.x, myRect.pivot.y + moving.y);
            

        }

        void Damage() {
            //Выходим, если уже активировано либо растояние больше чем нужно
            if (Activated || distToTarget >= DistMinForHitTarget) return;


            if (partnerHave)
            {
                //Если партнер самолета была бомба
                if (partnerType == CellInternalObject.Type.bomb)
                {
                    BombDamage();
                }
                //Если партнер самолета была ракета горизонтальная
                else if (partnerType == CellInternalObject.Type.rocketHorizontal)
                {
                    //myField.CreateRocketHorizontal(CellTarget, CellInternalObject.InternalColor.Red, 0);
                    //CellTarget.Damage();

                    CellTarget.explosion = new CellCTRL.Explosion(true, true, false, false, 0.05f, comb);
                    CellTarget.BufferCombination = comb;
                    CellTarget.BufferNearDamage = false;
                    CellTarget.ExplosionBoomInvoke(CellTarget.explosion);
                }
                //Если партнер самолета была ракета вертикальная
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
                //наносим самый обычный урон
                CellTarget.Damage(null, comb, false);
            }

            Activated = true;
            //ActivatedTime = Time.unscaledTime; //Время активации

            void BombDamage() {
                //Наносим урон по области 3х3

                CellTarget.explosion = new CellCTRL.Explosion(false, false, false, false, 0.05f, comb);
                CellTarget.BufferCombination = comb;
                CellTarget.BufferNearDamage = false;
                CellTarget.ExplosionBoomInvoke(CellTarget.explosion);

                int radius = 1;
                //Перебираем поле 3 на 3
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        int fieldPosX = CellTarget.pos.x + x;
                        int fieldPosY = CellTarget.pos.y + y;
                        //Если вышли за пределы карты или этой ячейки нету
                        if (fieldPosX < 0 || fieldPosX >= myField.cellCTRLs.GetLength(0) ||
                            fieldPosY < 0 || fieldPosY >= myField.cellCTRLs.GetLength(1) ||
                            !myField.cellCTRLs[fieldPosX, fieldPosY]
                            )
                        {
                            continue;
                        }

                        //Считаем время задержки взрыва этой ячейки
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
        //Если оказалось что внутреннего объекта нет, или нет ящика, или нет 
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

            //ищем ячейку к которой еще никто не летит
            foreach (CellCTRL cellPriority in myField.cellsPriority)
            {
                //В любом случае задать текушую чейку как целевую, у самолета должна быть цель
                result = cellPriority;

                //Если нашли эту ячейку в списке целей то завершаем перебор и переключаемся далее
                bool found = false;
                foreach (FlyCTRL fly in flyCTRLs)
                {
                    if (fly.CellTarget == cellPriority)
                    {
                        found = true;
                        break;
                    }
                }

                //Если закончили перебор и не нашли ячейку в списке, значит это то что нужно выбрать в качестве новой цели
                if (!found)
                {
                    break;
                }


            }


            return result;


        }
    }

    //Деструктор
    ~FlyCTRL() {

        reCalcList();


        void reCalcList() {
            List<FlyCTRL> flyCTRLsNew = new List<FlyCTRL>();

            //Создаем новый список пропуская только этот и пустые экземпляры
            foreach (FlyCTRL flyCTRL in flyCTRLs) {
                
                if (flyCTRL != null && flyCTRL != this) {
                    flyCTRLsNew.Add(flyCTRL);
                }
            }
        }
    }
}
