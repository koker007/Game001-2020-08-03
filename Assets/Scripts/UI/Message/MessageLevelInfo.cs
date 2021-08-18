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
    }

    public void ClickButtonStartLevel() {
        UICTRL.main.OpenGameplay();
        GlobalMessage.Close();
    }
}
