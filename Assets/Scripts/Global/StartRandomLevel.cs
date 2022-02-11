using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomLevel : MonoBehaviour
{
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            UICTRL.main.OpenWorld();
            Gameplay.main.levelSelect = Random.Range(100, 999);
            //LevelGenerator.main.GenerateLevelV2(Gameplay.main.levelSelect);
            UICTRL.main.OpenGameplay();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GlobalMessage.OpenLevelRedactor();
        }
    }
    #endif
}
