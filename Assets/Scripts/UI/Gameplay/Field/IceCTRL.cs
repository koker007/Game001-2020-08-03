using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Контролирует ячейку льда
/// </summary>
public class IceCTRL : MonoBehaviour
{
    [SerializeField]
    AnimatorCTRL animator;
    [SerializeField]
    CanvasGroup canvasGroup;

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

        HealthOld = myCell.ice;
        ChangeImage();


        if (myCell.ice > 0)
        {
            return;
        }

        DestroyIce();

        void ChangeImage()
        {
            if (myCell.ice == 1) {
                animator.PlayAnimation("lvl1");
            }
            else if (myCell.ice == 2) {
                animator.PlayAnimation("lvl2");
            }
            else if (myCell.ice == 3)
            {
                animator.PlayAnimation("lvl3");
            }
            else if (myCell.ice == 4)
            {
                animator.PlayAnimation("lvl4");
            }
            else if (myCell.ice == 5)
            {
                animator.PlayAnimation("lvl5");
            }
        }

        //Уничтожить если жизни кончились
        void DestroyIce()
        {
            if (myCell.ice > 0) return;

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
