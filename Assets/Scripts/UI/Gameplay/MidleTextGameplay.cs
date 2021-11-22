using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// онтролирует движение всплывающего текста
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

    void Destroy() {
        EnemyController.canEnemyMove = true;

        GameFieldCTRL.main.canPassTurn = true;
        
        Destroy(gameObject);
    }
}
