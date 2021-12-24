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

    [Header("Images")]
    [SerializeField]
    Image imageMixed;
    public const string strMixed = "Mixed";
    [SerializeField]
    Image imageCombo;
    public const string strCombo = "Combo";
    [SerializeField]
    Image imageCongratulations;
    public const string strCongratulations = "Congratulations";
    [SerializeField]
    Image imageEnemyMove;
    public const string strEnemyTurn = "Enemy Move";
    [SerializeField]
    Image imageNotBad;
    public const string strNotBad = "Not Bad";
    [SerializeField]
    Image imageYourMove;
    public const string strYourTurn = "Your Move";

    [SerializeField]
    AudioClip audioClip;

    [SerializeField]
    Animator AnimMigleTextComponent;

    public void SetText(string testNew) {
        CloseAll();

        text.gameObject.SetActive(true);
        fon.gameObject.SetActive(true);

        text.text = testNew;
    }

    public void SetColorFon(Color colorNew) {
        fon.color = colorNew;
    }

    public void SetMixed() {
        CloseAll();
        imageMixed.gameObject.SetActive(true);
    }
    public void SetTextCombo() {
        CloseAll();
        imageCombo.gameObject.SetActive(true);
    }
    public void SetTextCongratulations()
    {
        CloseAll();
        imageCongratulations.gameObject.SetActive(true);
    }
    public void SetTextEnemyMove()
    {
        CloseAll();
        imageEnemyMove.gameObject.SetActive(true);
    }
    public void SetTextNotBad()
    {
        CloseAll();
        imageNotBad.gameObject.SetActive(true);
    }
    public void SetTextYourMove()
    {
        CloseAll();
        imageYourMove.gameObject.SetActive(true);
    }

    public void CloseAll() {
        fon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        imageMixed.gameObject.SetActive(false);
        imageCombo.gameObject.SetActive(false);
        imageCongratulations.gameObject.SetActive(false);
        imageEnemyMove.gameObject.SetActive(false);
        imageNotBad.gameObject.SetActive(false);
        imageYourMove.gameObject.SetActive(false);

    }

    //Удаляется через событие в аниматоре. Оставить
    void Destroy() {
        EnemyController.canEnemyMove = true;

        GameFieldCTRL.main.canPassTurn = true;
        
        Destroy(gameObject);
    }
}
