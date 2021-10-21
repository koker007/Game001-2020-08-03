using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Контролирует ячейку льда
/// </summary>
public class IceCTRL : MonoBehaviour
{
    [SerializeField]
    AnimatorCTRL animator;

    CellCTRL myCell;
    RectTransform myRect;


    int HealthOld = -1;

    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni)
    {
        if (isInicialize) return;

        myCell = cellIni;

        //Добавляем в список эту плесень
        myCell.myField.iceCTRLs[cellIni.pos.x, cellIni.pos.y] = this;

        IniRect();

        ReCalcIceCount();

        isInicialize = true;
    }

    void IniRect()
    {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
    }


    void UpdateLife()
    {
        //Если здоровье не менялось
        if (HealthOld == myCell.ice)
            return;

        ChangeImage();


        if (myCell.ice > 0)
        {
            return;
        }

        DestroyIce();
        HealthOld = myCell.ice;

        void ChangeImage()
        {

        }

        //Уничтожить если жизни кончились
        void DestroyIce()
        {
            if (myCell.rock > 0) return;

            animator.PlayAnimation("Destroy");

            //Звук разбивания камня
            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipDamageRock, 0.5f, Random.Range(0.9f, 1.1f));
        }
    }

    void ReCalcIceCount()
    {

        int count = 0;

        //Перебираем все игровое поле
        for (int x = 0; x < myCell.myField.iceCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < myCell.myField.iceCTRLs.GetLength(1); y++)
            {
                if (myCell.myField.iceCTRLs[x, y])
                {
                    count++;
                }
            }
        }

        myCell.myField.CountIce = count;
    }

    public void Destroy() {

        myCell.myField.iceCTRLs[myCell.pos.x, myCell.pos.y] = null;
        ReCalcIceCount();
        Destroy(gameObject);
    }
}
