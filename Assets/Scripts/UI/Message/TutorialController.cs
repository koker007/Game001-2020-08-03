using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController main;
    [SerializeField] private int[] tutorialLevelNums;
    private bool[] levelTutorialShown;

    private void Start()
    {
        main = this;
        levelTutorialShown = new bool[tutorialLevelNums.Length];
    }

    public void CheckLevelTutorial(int levelNum)
    {
        for (int i = 0; i < tutorialLevelNums.Length; i++)
        {
            if (levelNum == tutorialLevelNums[i] && !levelTutorialShown[i])
            {
                GlobalMessage.LevelTutorial(levelNum);
                levelTutorialShown[i] = true;
                break;
            }
        }
    }
}
