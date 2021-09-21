using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������
/// <summary>
/// �������� ������� (��������� ��� �� LevelsScript)
/// </summary>
public class LevelsArray : LevelsScript
{
    private void Start()
    {
        main = this;

        //������� 1
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

        //������� 2
        Levels[2] = CreateLevel(2, 8, 8, 2000000, 1000, 4, 0);

        Levels[2].PassedWithScore = true;

        Levels[2].SetMass(
        new int[,] //exist
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,0,1,1,1,1,0,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,0,1,1,1,1,0,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,1,1,1,1,1,1 }
            },
            "exist"
            );
        Levels[2].SetMass(
            new int[,] //color
            {
                { 1,0,3,0,0,3,0,2 },
                { 0,1,0,3,3,0,2,0 },
                { 3,0,1,0,0,2,0,3 },
                { 0,3,0,1,2,0,3,0 },
                { 0,3,0,2,1,0,3,0 },
                { 3,0,2,0,0,1,0,3 },
                { 0,2,0,3,3,0,1,0 },
                { 2,0,3,0,0,3,0,1 }
            },
            "color"
            );
        Levels[2].SetMass(
            new int[,] //type
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 }
            },
            "type"
            );
        Levels[2].SetCells();

        //������� 3
        Levels[3] = CreateLevel(3, 10, 10, 10000, 99, 5, 30);

        Levels[3].PassedWithScore = true;

        Levels[3].SetMass(
        new int[,] //exist
            {
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,1,1,1,0 },
                { 0,0,1,1,1,1,1,1,0,0 },
                { 0,1,1,1,1,1,1,1,1,0 },
                { 0,1,1,1,1,1,1,1,1,0 },
                { 0,1,1,1,1,1,1,1,1,0 },
                { 0,1,1,1,1,1,1,1,1,0 }
            },
            "exist"
            );
        Levels[3].SetMass(
            new int[,] //color
            {
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 2,1,3,1,2,3,1,2,3,1 }
            },
            "color"
            );
        Levels[3].SetMass(
            new int[,] //type
            {
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 }
            },
            "type"
            );
        Levels[3].SetMass(
             new int[,] //box
             {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 5,5,5,5,5,5,5,5,5,5 },
                { 5,5,5,5,5,5,5,5,5,5 },
                { 0,0,5,5,5,5,5,5,5,5 }
             },
             "box"
             );
        Levels[3].SetMass(
     new int[,] //mold
     {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 2,2,2,2,2,2,2,2,2,2 },
                { 3,3,3,3,3,3,3,3,3,3 },
                { 4,4,4,4,4,4,4,4,4,4 },
                { 5,5,5,5,5,5,5,5,5,5 }
     },
            "mold"
     );
        Levels[3].SetMass(
        new int[,] //panel
        {
                { 1,1,1,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
     },
            "panel"
     );
        Levels[3].SetCells();
        Levels[3].SetMass(
    new int[,] //rock
    {
                { 1,0,1,1,1,1,1,1,0,1 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
    },
    "rock"
    );
        Levels[3].SetCells();
    }
}

/*

//������� 1
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