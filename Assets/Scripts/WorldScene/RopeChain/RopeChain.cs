using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Семен
/// <summary>
/// Скрипт который контролирует линию от точки к точке
/// </summary>
public class RopeChain : MonoBehaviour
{
    [SerializeField]
    LVLChain myChain;

    //Позиция меша по факту
    [SerializeField]
    Transform[] posRigs;

    //позиция кости которая должна быть
    [SerializeField]
    Transform[] posVector;

    [SerializeField]
    float strange = 4f;

    [SerializeField]
    SkinnedMeshRenderer mesh;


    //Пересчет позиции с учетом направляющего вектора
    public void ReCalcPositions3(LVLChain myChainFunc) {
        myChain = myChainFunc;

        if (posVector.Length < 2) {
            return;
        }

        float one = 1.0f/(posVector.Length-1);

        //Перебираем все кости от начала до конца
        for (int num = 0; num < posVector.Length; num++) {
            float progress = one * num;

            // теперь знаем позицию
            Vector3 position = myChain.GetPosition(progress, 1);

            //перемещаем эту кость на эту позицию
            //если это первая кость
            if (num == 0) {
                //просто перемещаем всю модель на эту позицию
                gameObject.transform.position = position;
                //И поворачиваем так чтобы она смотрела по направляющему вектору этой кнопки
                Quaternion rot = Quaternion.LookRotation(myChain.forvard);
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);

                gameObject.transform.rotation = rot;


            }
            else{
                //поворачиваем родителя в сторону новой позиции
                Vector3 forvard = (position - posVector[num - 1].transform.position).normalized;

                //И поворачиваем так чтобы она смотрела по направляющему вектору к следующей ступени
                Quaternion rot = Quaternion.LookRotation(forvard);
                
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
                posVector[num - 1].transform.rotation = rot;

                //Узнаем растояние между точками
                float distance = Vector3.Distance(position, posVector[num - 1].transform.position)*0.01f;
                //перемемещаем точку на указанное растояние
                posVector[num].position = myChain.GetPosition(progress, 1); //new Vector3(0, 0, distance);

                GameObject sphere = Instantiate(WorldRopeChainCTRL.main.PrefabSphere, gameObject.transform);
                sphere.transform.position = posVector[num].position;

                //Поворачиваем прошлый меш
                Quaternion rotMesh = posRigs[num].rotation;
                rotMesh.eulerAngles = new Vector3(posVector[num-1].rotation.eulerAngles.x+90, posVector[num-1].rotation.eulerAngles.y, posVector[num-1].rotation.eulerAngles.z);
                posRigs[num].rotation = rotMesh;

                //Перемещаем текущий меш
                posRigs[num].position = posVector[num].position;
            }
        }

        mesh.gameObject.SetActive(false);
    }

    

    //Перерасчет позиции с учетом направляющего вектора
    public void ReCalcPositions4(LVLChain myChainFunc) {
        //присваиваем веревке ее звено
        myChain = myChainFunc;

        if (posVector.Length < 2)
        {
            return;
        }



    }

    private void Start()
    {
        Invoke("ReCalc", 0.5f);
    }

    private void Update()
    {
        ReCalc();
    }

   [SerializeField]
    bool needRecalc = false;

    void ReCalc()
    {
        if (!needRecalc)
            return;

        myChain.iniChain();
        needRecalc = false;
    }
}
