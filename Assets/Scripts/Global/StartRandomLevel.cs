using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomLevel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            UICTRL.main.OpenWorld();
            Gameplay.main.levelSelect = Random.Range(100, 999);
            LevelGenerator.main.GenerateLevel(Gameplay.main.levelSelect);
            UICTRL.main.OpenGameplay();
        }
    }
}