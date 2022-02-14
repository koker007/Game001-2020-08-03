using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexandr
public class MessageLevelTutorial : MonoBehaviour
{
    [SerializeField] private TextTranslator _tutorialText;
    [SerializeField] private Animator _tutorialAnimation;

    private int _tutorialTypeNum;
    private int _tutorialNum;
    private int _numLevel;

    public void Inicializate(int numLevel, string tutorialName, TutorialController.Tutorials tutorialType, int tutorialTypeNum, int tutorialNum)
    {
        _numLevel = numLevel;
        _tutorialTypeNum = tutorialTypeNum;
        _tutorialNum = tutorialNum;
        SetTextKey($"Tutorial{tutorialName}_{tutorialNum}");
        SetAnimation(((int)tutorialType + 1) * 10 + tutorialNum);
    }

    public void SetTextKey(string value)
    {
        _tutorialText.key = value;
        _tutorialText.UpdateText();
    }

    public void SetAnimation(int value)
    {
        _tutorialAnimation.SetInteger("TutorialNum", value);
    }

    public void NextTutorial()
    {
        TutorialController.main.CheckNextTutorial(_numLevel, _tutorialTypeNum, _tutorialNum);
    }
}
