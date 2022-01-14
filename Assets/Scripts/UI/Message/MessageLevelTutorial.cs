using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
public class MessageLevelTutorial : MonoBehaviour
{
    [SerializeField] private TextTranslator _tutorialText;
    [SerializeField] private Animator _tutorialAnimation;
    private MessageCTRL _MessageController;

    private float _tutorialNum;

    private void Awake()
    {
        _MessageController = GetComponent<MessageCTRL>();
    }

    public void Inicializate(float tutorialNum)
    {
        _tutorialNum = tutorialNum;
        SetTextKey("Tutorial" + tutorialNum.ToString());
        SetAnimation((int)(Mathf.Round(tutorialNum * 10)));
        Debug.Log((int)(Mathf.Round(tutorialNum * 10)));
    }

    public void SetTextKey(string value)
    {
        _tutorialText.key = value;
        _tutorialText.UpdtaeText();
    }

    public void SetAnimation(int value)
    {
        _tutorialAnimation.SetInteger("TutorialNum", value);
    }

    public void NextTutorial()
    {
        TutorialController.main.CheckNextTutorial(_tutorialNum);
    }
}
