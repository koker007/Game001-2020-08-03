using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAbility : MonoBehaviour
{

    [SerializeField]
    Text Count;

    [SerializeField]
    MenuGameplay.SuperHitType type;
    MenuGameplay.SuperHitType typeActiveOld;

    [SerializeField]
    Animator animator;

    void Update()
    {
        UpdateCount();
    }

    void UpdateCount() {
        if (type == MenuGameplay.SuperHitType.internalObj)
        {
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopInternal.Amount);
        }
        else if (type == MenuGameplay.SuperHitType.rosket2x)
        {
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopRocket.Amount);
        }
        else if (type == MenuGameplay.SuperHitType.bomb)
        {
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopBomb.Amount);
        }
        else if (type == MenuGameplay.SuperHitType.Color5)
        {
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopColor5.Amount);
        }
        else if (type == MenuGameplay.SuperHitType.mixed)
        {
            Count.text = System.Convert.ToString(PlayerProfile.main.ShopMixed.Amount);
        }

        TestAnimator();

        void TestAnimator() {
            if (typeActiveOld != MenuGameplay.main.SuperHitSelected) {
                typeActiveOld = MenuGameplay.main.SuperHitSelected;

                if (type != MenuGameplay.main.SuperHitSelected)
                {
                    animator.SetFloat("TypeAnimationBasic", 0);
                }
                else {
                    animator.SetFloat("TypeAnimationBasic", 1);
                }
            }
        }
    }

    public void ButtonClickAbility() {

        //Если текущее состояние магазина совпадает с типом кнопки
        if (MenuGameplay.main.SuperHitSelected == type)
        {
            //выключаем активацию
            MenuGameplay.main.SuperHitSelected = MenuGameplay.SuperHitType.none;
        }
        //иначе включаем активацию в соответствии с типом кнопки
        else if (MenuGameplay.SuperHitType.internalObj == type)
        {
            MenuGameplay.main.ButtonClickDestroyInternal();
        }
        else if (MenuGameplay.SuperHitType.rosket2x == type)
        {
            MenuGameplay.main.ButtonClickRosket();
        }
        else if (MenuGameplay.SuperHitType.bomb == type)
        {
            MenuGameplay.main.ButtonClickBomb();
        }
        else if (MenuGameplay.SuperHitType.Color5 == type)
        {
            MenuGameplay.main.ButtonClickSuperColor();
        }
        else if (MenuGameplay.SuperHitType.mixed == type)
        {
            MenuGameplay.main.ButtonClickMixed();
        }


    }
}
