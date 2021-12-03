using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
///////////////////////////////////////////////////////////////////////////////////
//Контролирует всплываюший текст в геймплее (Всплывание через аниматор)
public class MidleTextGameplay: MonoBehaviour
{

    [SerializeField]
    Image fon;
    [SerializeField]
    Text text;

    [SerializeField]
    AudioClip audioClip;

    [SerializeField]
    Animator AnimMigleTextComponent;

    public void SetText(string testNew) {
        text.text = testNew;
    }

    public void SetColorFon(Color colorNew) {
        fon.color = colorNew;
    }

    //Удаляется через событие в аниматоре. Оставить
    void Destroy() {
        EnemyController.canEnemyMove = true;

        GameFieldCTRL.main.canPassTurn = true;
        
        Destroy(gameObject);
    }
}
