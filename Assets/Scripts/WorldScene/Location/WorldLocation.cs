using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//семен
//Андрей
/// <summary>
/// Представляет из себя данные о текущей локации
/// </summary>
public class WorldLocation : MonoBehaviour
{
    [SerializeField]
    public Texture fonGameplay;

    /// <summary>
    /// Размер локации в градусах
    /// </summary>
    [SerializeField]
    public float lenghtAngle = 90;
    [SerializeField]
    float rotateNow = 0;

    /// <summary>
    /// Текущий угл этой локации определяется при инициализации
    /// </summary>
    [SerializeField]
    public float myAngle = 0;

    [SerializeField]
    public int myLvlNumStart = 0;
    [SerializeField]
    public LevelPosition[] LevelPositions;

    [SerializeField] private List<GameObject> LevelButtons = new List<GameObject>();

    public GameObject[] Graunds;

    //Инициализировать локацию
    public void Inicialize(float myAngleNew, int myLVLnumStartNew) {
        myAngle = myAngleNew;

        //Вращяем так как того требует наш новый градус
        Quaternion rot = transform.localRotation;
        rot.eulerAngles = new Vector3(Mathf.Abs(myAngle), rot.eulerAngles.y, rot.eulerAngles.z);
        transform.localRotation = rot;

        myLvlNumStart = myLVLnumStartNew;

        CreateGraunds();

        void CreateGraunds() {

            if (Graunds == null)
            {
                Graunds = new GameObject[(int)lenghtAngle / 10];
            }

            //плюсОдин если остаток
            int plus = 0;
            if ((int)lenghtAngle % 10 > 0.01f)
            {
                plus++;
            }

            //Если локаций меньше чем должно быть
            if (Graunds.Length < lenghtAngle / 10)
            {
                //Расширяем массив
                GameObject[] GraundsNew = new GameObject[((int)lenghtAngle / 10) + plus];
                for (int num = 0; num < Graunds.Length; num++) {
                    GraundsNew[num] = Graunds[num];
                }

                Graunds = GraundsNew;
            }

            //Проверяем на пустоты
            for (int num = 0; num < Graunds.Length; num++) {
                //Если уже есть земля выходим
                if (Graunds[num] != null) continue;

                Graunds[num] = Instantiate(WorldGenerateScene.main.PrefGroundLoc10Grad, transform);

                //Нужно повернуть на необходимый градус
                Quaternion rotate = Graunds[num].transform.localRotation;
                rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, rotate.eulerAngles.y, rotate.eulerAngles.z + num*10);
                Graunds[num].transform.localRotation = rotate;

                //Если было прибавыление на 1
                if (num == Graunds.Length + 1 && plus > 0) {
                    //Делаем размер немного меньше
                    Graunds[num].transform.localScale = new Vector3(Graunds[num].transform.localScale.x*0.99f, Graunds[num].transform.localScale.y, Graunds[num].transform.localScale.z);
                }
            }

        }

    }

    public void AddLevelButton(GameObject levelButton)
    {
        LevelButtons.Add(levelButton);
    }

    public void TestDelete() {
        rotateNow = WorldGenerateScene.main.rotationNow;
        //Первое растояние спереди от которого надо удалять локацию. второе растояние сзади после которого надо удалять
        //Сзади удаляется сразу как за камерой
        if (WorldGenerateScene.main.rotationNow - myAngle > WorldGenerateScene.main.angleForvardSpawn 
            || WorldGenerateScene.main.rotationNow - myAngle < (lenghtAngle+45) * -1) {
            //gameObject.SetActive(false);
            foreach(GameObject but in LevelButtons)
            {
                but.SetActive(false);
                but.transform.SetParent(WorldGenerateScene.main.RotatableObj);
            }
            Destroy(gameObject);           
        }
    }
}
