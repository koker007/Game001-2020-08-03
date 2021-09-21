using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//александр
/// <summary>
/// редактор уровней (наследует все от LevelsScript)
/// </summary>
public class LevelsArray : LevelsScript
{
    private void Start()
    {
        main = this;

        //уровень 1
        Levels[1] = CreateLevel(1, 6, 5, 15000, 15, 3, 0);

        Levels[1].PassedWithCrystal = true;
        Levels[1].NeedColor = (CellInternalObject.InternalColor)0;
        Levels[1].NeedCrystal = 20;
        Debug.Log((int)Levels[1].NeedColor);
        Levels[1].SetMass(
        new int[,] //exist
            {
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 }
            },
            "exist"
            );
        Levels[1].SetMass(
            new int[,] //color
            {
                { 3,0,3,0,1 },
                { 0,3,1,1,0 },
                { 0,3,3,1,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            },
            "color"
            );
        Levels[1].SetMass(
            new int[,] //type
            {
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            },
            "type"
            );
        Levels[1].SetMass(
           new int[,]
           {
                { 0,0,1,0,0 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
           },
           "rock"
           );
        Levels[1].SetCells();

        //уровень 2
        Levels[2] = CreateLevel(2, 7, 7, 2000000, 1000, 3, 0);

        Levels[2].PassedWithCrystal = true;
        Levels[2].NeedColor = (CellInternalObject.InternalColor)1;
        Levels[2].NeedCrystal = 25;

        Levels[2].SetMass(
            new int[,] //exist
            {
                { 0,1,0,0,0,1,0 },
                { 1,1,1,0,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,0,1,1,1 }
            },
            "exist"
            );
        Levels[2].SetMass(
            new int[,] //color
            {
                { 0,2,0,0,0,1,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,1,0,0,0,2,0 },
                { 1,2,1,0,2,1,2 }
            },
            "color"
            );
        Levels[2].SetMass(
            new int[,] //type
            {
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,6,0,0,0,6,0 },
                { 1,1,1,0,1,1,1 }
            },
            "type"
            );
        Levels[2].SetMass(
            new int[,] //rock
            {
                { 0,1,0,0,0,1,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 }
            },
            "rock"
            );
        Levels[2].SetCells();

        //уровень 3
        Levels[3] = CreateLevel(3, 8, 8, 100000, 99, 3, 0);

        Levels[3].PassedWithPanel = true;

        Levels[3].SetMass(
            new int[,] //exist
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
            },
            "exist"
            );
        Levels[3].SetMass(
            new int[,] //color
            {
                { 2,1,2,1,2,1,2,1 },
                { 1,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,2 },
                { 2,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,2 },
                { 2,1,0,0,0,0,2,1 },
                { 1,2,1,2,1,2,1,2 },
            },
            "color"
            );
        Levels[3].SetMass(
            new int[,] //type
            
            {
                { 4,1,1,1,1,1,1,3 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,1,0,0,0,0,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 3,1,1,1,1,1,1,4 },
            },
            "type"
            );
        Levels[3].SetMass(
             new int[,] //panel
             {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 1,0,0,0,0,0,0,1 },
            },
             "panel"
             );
        Levels[3].SetMass(
            new int[,] //rock
            {
                { 0,1,1,1,1,1,1,0 },
                { 1,1,0,0,0,0,1,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,1,0,0,0,0,1,1 },
                { 0,0,1,0,0,1,0,0 },
                { 0,1,1,1,1,1,1,0 },
            },
            "rock"
            );
        Levels[3].SetCells();
    }
}

/*

//уровень 1
        Levels[1] = CreateLevel(1, 6, 5, 2000000, 50, 4, 0);

        Levels[1].PassedWithCrystal = true;
        Levels[1].NeedColor = (CellInternalObject.InternalColor)0;
        Levels[1].NeedCrystal = 20;
        Debug.Log((int)Levels[1].NeedColor);
        Levels[1].SetMass(
        new int[,] //exist
            {
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 }
            },
            "exist"
            );
        Levels[1].SetMass(
            new int[,] //color
            {
                { 3,0,3,0,1 },
                { 0,3,1,1,0 },
                { 0,3,3,1,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            },
            "color"
            );
        Levels[1].SetMass(
            new int[,] //type
            {
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            },
            "type"
            );
        Levels[1].SetMass(
           new int[,]
           {
                { 0,0,1,0,0 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
           },
           "rock"
           );
        Levels[1].SetCells();






 */