using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
public class TutorialController : MonoBehaviour
{
    public static TutorialController main;

    [System.Serializable]
    public enum Tutorials
    {
        Start,
        Super,
        Combinations,
        Rock,
        Panel,
        Box,
        Ice,
        Mold,
        Portal,
        Dispencer,
        Blocker,
        Walls,
        Enemy,
        Ultimative,
        UnderObjects
    }

    [System.Serializable]
    public class TypeTutorial
    {
        public int length;
        public Tutorials _tutorialType;
    }

    [System.Serializable]
    public class LevelTutorial
    {
        public int _levelNum;
        public TypeTutorial[] _tutorialTypes = new TypeTutorial[0];
        public int[] _tutorialsFieldID = new int[0];
    }

    [SerializeField] private GameObject[] _tutorialsField = new GameObject[0];

    [SerializeField] private LevelTutorial[] _tutorials = new LevelTutorial[0];

    private void Start()
    {
        main = this;
    }
    /// <summary>
    /// проверка есть ли у уровня туториал
    /// </summary>
    /// <param name="levelNum"></param>
    /// <returns></returns>
    public bool CheckLevelTutorial(int levelNum)
    {
        CloseAllTutorialField();
        int levelID = 0;
        if (FindIdLevel(ref levelID, levelNum))
        {
            GlobalMessage.LevelTutorial(levelID, _tutorials[levelID]._tutorialTypes[0]._tutorialType.ToString(), _tutorials[levelID]._tutorialTypes[0]._tutorialType, 0, 1);
            if(_tutorials[levelID]._tutorialsFieldID.Length > 0)
                _tutorialsField[_tutorials[levelID]._tutorialsFieldID[0]].SetActive(true);
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
            //Если у выбранного уровня есть туториал
            if (levelNum == _tutorials[i]._levelNum)
            {
                levelID = i;
                return true;
            }
        }
        return false;
    }
    //запускает следующий туториал если найдет его
    public bool CheckNextTutorial(int levelID, int tutorialTypeNum, int tutorialNum)
    {
        if(_tutorials[levelID]._tutorialTypes[tutorialTypeNum].length > tutorialNum)
        {
            tutorialNum++;
            GlobalMessage.LevelTutorial(levelID, _tutorials[levelID]._tutorialTypes[tutorialTypeNum]._tutorialType.ToString(), _tutorials[levelID]._tutorialTypes[tutorialTypeNum]._tutorialType, tutorialTypeNum, tutorialNum);
            return true;
        }
        else if (_tutorials[levelID]._tutorialTypes.Length - 1 > tutorialTypeNum)
        {
            tutorialTypeNum++;
            GlobalMessage.LevelTutorial(levelID, _tutorials[levelID]._tutorialTypes[tutorialTypeNum]._tutorialType.ToString(), _tutorials[levelID]._tutorialTypes[tutorialTypeNum]._tutorialType, tutorialTypeNum, tutorialNum);
            return true;
        }
        return false;
    }
    //запускает следующее затемнение игрового поля если найдет его
    public bool CheckNextTutorialField(int levelNum, int tutorialFieldNum)
    {
        int levelID = 0;
        if (FindIdLevel(ref levelID, levelNum))
        {
            CloseAllTutorialFieldForLevel(levelID);

            if (_tutorials[levelID]._tutorialsFieldID.Length <= tutorialFieldNum)
            {
                return false;
            }
            else
            {
                _tutorialsField[_tutorials[levelID]._tutorialsFieldID[tutorialFieldNum]].SetActive(true);
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    //закрывает все затемнения игрового для уровня
    private void CloseAllTutorialFieldForLevel(int levelID)
    {
        foreach (int tutID in _tutorials[levelID]._tutorialsFieldID)
        {
            _tutorialsField[tutID].SetActive(false);
        }
    }

    //закрывает все существующие затемнения игрового
    public void CloseAllTutorialField()
    {
        foreach (GameObject tut in _tutorialsField)
        {
            tut.SetActive(false);
        }
    }
}
