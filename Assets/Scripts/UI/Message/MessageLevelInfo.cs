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

        try
        {
            LevelInfoText.text = "Required number of points:" + LevelsScript.main.ReturnLevel().NeedScore.ToString() + "\nRecord:\n" + LevelsScript.main.ReturnLevel().MaxScore.ToString();
            Gameplay.main.CountStars(LevelsScript.main.ReturnLevel().MaxScore, ref LevelStars);
        }
        catch { }
    }

    public void ClickButtonStartLevel() {
        UICTRL.main.OpenGameplay();
        GlobalMessage.Close();
    }
}
