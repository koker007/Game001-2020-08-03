using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
/// <summary>
/// отвечает за сообщение уровней
/// </summary>
public class MessageLevelInfo : MonoBehaviour
{

    [SerializeField]
    Text LevelText;
    [SerializeField]
    Text LevelInfoText;
    [SerializeField]
    Image[] LevelStars = new Image[3];


    // Start is called before the first frame update
    void Start()
    {
        Inicialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Inicialize() {
        LevelText.text = System.Convert.ToString(Gameplay.main.levelSelect);

        string textInfo = "Required number of points:";
        textInfo += System.Convert.ToString(LevelsScript.main.ReturnLevel().NeedScore);
        textInfo += "\nRecord:\n";
        textInfo += System.Convert.ToString(LevelsScript.main.ReturnLevel().MaxScore);

        LevelInfoText.text = textInfo;

        Gameplay.main.CountStars(LevelsScript.main.ReturnLevel().MaxScore, ref LevelStars);

    }

    public void ClickButtonStartLevel() {
        UICTRL.main.OpenGameplay();

        MessageCTRL myMessageCTRL = gameObject.GetComponent<MessageCTRL>();
        myMessageCTRL.ClickButtonClose();
    }
}
