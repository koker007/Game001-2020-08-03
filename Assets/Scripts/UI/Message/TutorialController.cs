using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//јндрей
public class TutorialController : MonoBehaviour
{
    public static TutorialController main;

    [System.Serializable]
    public class LevelTutorial
    {
        public int _levelNum;
        public float[] _tutorialNumMessages = new float[0];
    }

    [SerializeField] private LevelTutorial[] _tutorials = new LevelTutorial[0];

    private void Start()
    {
        main = this;
    }
    
    public bool CheckLevelTutorial(int levelNum)
    {
        int levelID = 0;
        if (FindIdLevel(ref levelID, levelNum))
        {
            GlobalMessage.LevelTutorial(_tutorials[levelID]._tutorialNumMessages[0]);
            return true;
        }
        else
        {
            return false;
        }
    }
    //находит идентификатор туториала в массиве если есть туториал
    private bool FindIdLevel(ref int levelID, int levelNum)
    {
        for (int i = 0; i < _tutorials.Length; i++)
        {
            //≈сли у выбранного уровн€ есть туториал
            if (levelNum == _tutorials[i]._levelNum)
            {
                levelID = i;
                return true;
            }
        }
        return false;
    }
    //запускает следующий туториал если найдет его
    public bool CheckNextTutorial(float tutorialNum)
    {
        int levelID = 0;
        if (FindIdLevel(ref levelID, (int)tutorialNum))
        {
            for (int i = 0; i < _tutorials[levelID]._tutorialNumMessages.Length; i++)
            {
                //≈сли найден туториал и у выбранного уровн€ есть следующий туториал вызываем его
                if (tutorialNum == _tutorials[levelID]._tutorialNumMessages[i] && i + 1 < _tutorials[levelID]._tutorialNumMessages.Length)
                {
                    GlobalMessage.LevelTutorial(_tutorials[levelID]._tutorialNumMessages[i + 1]);
                    return true;
                }
            }
        }
        return false;
    }
}
