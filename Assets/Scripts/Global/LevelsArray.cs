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

        ////////////////////////////////////////////////////////////////////////////
        //������� 1
        int lvl = 1;
        Levels[lvl] = CreateLevel(lvl, 6, 5, 5000, 15, 3, 0);

        Levels[lvl].PassedWithCrystal = true;
        Levels[lvl].NeedColor = (CellInternalObject.InternalColor)0;
        Levels[lvl].NeedCrystal = 20;
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
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
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 2
        lvl = 2;
        Levels[lvl] = CreateLevel(lvl, 7, 7, 7500, 20, 3, 0);

        Levels[lvl].PassedWithCrystal = true;
        Levels[lvl].NeedColor = (CellInternalObject.InternalColor)1;
        Levels[lvl].NeedCrystal = 50;

        Levels[2].SetMass(
            new int[,] //exist
            {
                { 0,1,0,0,0,1,0 },
                { 1,1,1,0,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 0,0,1,1,1,0,0 },
                { 0,1,0,1,0,1,0 },
                { 1,1,1,0,1,1,1 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
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
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 3
        lvl = 3;
        Levels[lvl] = CreateLevel(lvl, 8, 8, 10000, 20, 3, 0);

        Levels[lvl].PassedWithPanel = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,0,1,1,0,1,1 },
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,4,4,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,1,0,0,1,1,1 },
            },
            "type"
            );
        Levels[lvl].SetMass(
             new int[,] //panel
             {
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
            },
             "panel"
             );
        Levels[lvl].SetMass(
             new int[,] //rock
             {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 1,1,0,0,0,0,1,1 },
                { 0,0,0,0,0,0,0,0 },
            },
             "rock"
             );
        Levels[lvl].SetCells();

        //////////////////////////////////////////////////////////////////////////////
        //������� 4
        lvl = 4;
        Levels[lvl] = CreateLevel(lvl, 8, 9, 15000, 20, 4, 0);

        Levels[lvl].PassedWithPanel = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 1,0,0,1,0,1,0,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
                { 1,0,1,1,1,1,1,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
                { 1,0,1,1,1,1,1,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
                { 1,0,1,1,1,1,1,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 1,0,0,1,0,1,0,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
                { 1,0,1,1,2,1,1,0,1 },
                { 0,0,1,1,1,1,1,0,0 },
                { 1,0,1,1,1,1,1,0,1 },
                { 0,0,2,0,3,0,3,0,0 },
                { 1,0,0,3,1,2,0,0,1 },
                { 0,0,3,0,1,0,2,0,0 },
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            {
                { 1,1,1,1,1,1,1,1,1 },
                { 1,1,0,0,0,0,0,1,1 },
                { 1,1,0,0,1,0,0,1,1 },
                { 1,1,0,0,0,0,0,1,1 },
                { 1,1,0,0,0,0,0,1,1 },
                { 1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,2,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1 },
            },
            "type"
            );
        Levels[lvl].SetMass(
             new int[,] //panel
             {
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,1,0,0,0,0 },
                { 0,0,0,0,1,0,0,0,0 },
            },
             "panel"
             );
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,0,0,1,0,1,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,1,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,1,1,1,1,1,0,0 },
                { 0,0,1,1,0,1,1,0,0 },
                { 0,0,1,1,0,1,1,0,0 },
            },
            "rock"
            );
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 5
        lvl = 5;
        Levels[lvl] = CreateLevel(lvl, 6, 6, 10000, 30, 4, 0);

        Levels[lvl].PassedWithCrystal = true;
        Levels[lvl].NeedColor = (CellInternalObject.InternalColor)2;
        Levels[lvl].NeedCrystal = 45;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 0,1,1,1,1,0 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 0,2,0,0,1,0 },
                { 0,0,0,2,0,0 },
                { 0,0,0,0,0,0 },
                { 0,0,0,0,0,0 },
                { 0,0,0,0,0,0 },
                { 0,0,0,0,0,0 }
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,0,0,0,0,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1,1 }
            },
            "type"
            );
        Levels[lvl].SetMass(
            new int[,] //box
            {
                { 2,0,0,0,0,2 },
                { 3,0,0,0,0,3 },
                { 4,1,1,1,1,4 },
                { 5,3,2,2,3,5 },
                { 5,4,3,3,4,5 },
                { 5,5,4,4,5,5 }
            },
            "box"
            );
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 6
        lvl = 6;
        Levels[lvl] = CreateLevel(lvl, 8, 8, 17500, 20, 4, 0);

        Levels[lvl].PassedWithPanel = true;

        Levels[lvl].SetMass(
            new int[,] //exist
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
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 2,1,2,1,2,1,2,1 },
                { 1,0,0,0,0,0,0,2 },
                { 2,0,0,0,3,0,0,1 },
                { 1,0,0,0,0,3,0,2 },
                { 2,0,3,0,0,0,0,1 },
                { 1,0,0,3,0,0,0,2 },
                { 2,1,0,0,0,0,2,1 },
                { 1,2,1,2,1,2,1,2 }
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 4,1,1,1,1,1,1,3 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,1,1,0,0,1 },
                { 1,0,1,0,0,1,0,1 },
                { 1,0,1,0,0,1,0,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 3,1,1,1,1,1,1,4 }
            },
            "type"
            );
        Levels[lvl].SetMass(
             new int[,] //panel
             {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 1,0,0,0,0,0,0,1 }
            },
             "panel"
             );
        Levels[lvl].SetMass(
            new int[,] //box
            {
                { 0,1,1,1,1,1,1,0 },
                { 1,1,0,0,0,0,1,1 },
                { 1,0,0,0,0,0,0,1 },
                { 1,0,0,2,2,0,0,1 },
                { 1,0,0,2,2,0,0,1 },
                { 0,1,0,0,0,0,1,0 },
                { 0,0,1,0,0,1,0,0 },
                { 0,1,1,1,1,1,1,0 }
            },
            "box"
            );
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,1,0,0,1,0,0 },
                { 0,0,1,0,0,1,0,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 }
            },
            "rock"
            );
        Levels[lvl].SetCells();


        ////////////////////////////////////////////////////////////////////////////
        //������� 7
        lvl = 7;
        Levels[lvl] = CreateLevel(lvl, 8, 7, 10000, 35, 4, 0);

        Levels[lvl].PassedWithBox = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 0,0,1,1,1,0,0 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 0,0,1,1,1,0,0 }
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 0,0,1,1,1,0,0 }
            },
            "type"
            );
        Levels[lvl].SetMass(
            new int[,] //box
            {
                { 0,0,0,0,0,0,0 },
                { 3,0,0,0,0,0,3 },
                { 0,0,0,0,0,0,0 },
                { 4,1,1,1,1,1,4 },
                { 0,2,2,2,2,2,0 },
                { 5,3,3,3,3,3,5 },
                { 0,4,4,4,4,4,0 },
                { 0,0,5,5,5,0,0 }
            },
            "box"
            );
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 8
        lvl = 8;
        Levels[lvl] = CreateLevel(lvl, 8, 8, 15000, 20, 4, 0);

        Levels[lvl].PassedWithPanel = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 0,1,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,1,0,0,1,0,0 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
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
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,3,1,1,1,1 },
                { 1,1,1,5,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 }
            },
            "type"
            );
        Levels[lvl].SetMass(
             new int[,] //panel
             {
                { 0,0,1,1,1,1,1,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,1,1,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 }
            },
             "panel"
             );
        Levels[lvl].SetMass(
            new int[,] //box
            {
                { 0,2,0,0,0,0,2,0 },
                { 1,0,0,0,0,0,0,1 },
                { 0,2,0,0,0,0,2,0 },
                { 2,0,0,0,0,0,0,2 },
                { 0,2,0,0,0,0,2,0 },
                { 2,1,1,0,1,1,1,2 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 }
            },
            "box"
            );
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,0,0,0,0,0,0,0 },
                { 0,1,0,0,0,0,1,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,1,0,0,0,0,1,0 },
                { 0,0,1,0,0,1,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,1,0,0,1,0,0 }
            },
            "rock"
            );
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 9
        lvl = 9;
        Levels[lvl] = CreateLevel(lvl, 9, 8, 15000, 25, 4, 0);

        Levels[lvl].PassedWithBox = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 0,0,1,0,0,1,0,0 },
                { 0,1,0,1,1,0,1,0 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,1,0 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 }
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,5,1,1,1,1,5,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,5,1,1,5,1,1 },
                { 5,1,1,1,1,1,1,5 },
                { 1,1,1,4,4,1,1,1 }
            },
            "type"
            );
        Levels[lvl].SetMass(
            new int[,] //box
            {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 1,0,0,0,0,0,0,1 },
                { 2,0,1,0,0,1,0,2 },
                { 3,2,2,2,2,2,2,3 },
                { 4,3,0,3,3,0,3,4 },
                { 0,4,4,4,4,4,4,0 },
                { 0,5,5,0,0,5,5,0 }
            },
            "box"
            );
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,1,0,0,0,0,1,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,1,0,0,1,0,0 },
                { 1,0,0,0,0,0,0,1 },
                { 0,0,0,1,1,0,0,0 }
            },
            "rock"
            );
        Levels[lvl].SetCells();

        ////////////////////////////////////////////////////////////////////////////
        //������� 10
        lvl = 10;
        Levels[lvl] = CreateLevel(lvl, 9, 7, 10000, 20, 4, 0);

        Levels[lvl].PassedWithMold = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 0,0,1,0,1,0,0 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 0,1,1,1,1,1,0 },
                { 0,0,1,1,1,0,0 },
                { 0,1,1,1,1,1,0 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 }
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 1,1,0,1,0,1,1 },
                { 1,1,2,1,2,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,0,1,1,1 },
                { 1,3,2,0,2,3,1 }
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0 },
                { 0,5,0,1,0,5,0 },
                { 0,0,1,1,1,0,0 },
                { 0,1,1,2,1,1,0 },
            },
            "type"
            );
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,0,1,0,1,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,1,1,1,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,1,0,1,0,1,0 },
                { 0,0,1,0,1,0,0 },
                { 0,1,1,0,1,1,0 }
            },
            "rock"
            );
        Levels[lvl].SetMass(
            new int[,] //mold
            {
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 }
            },
            "mold"
            );
        Levels[lvl].SetCells();

    }


}

/*

//////////////////////////////////////////////////////////////////////////////
        //������� 4
        lvl = 4;
        Levels[lvl] = CreateLevel(3, 8, 8, 100000, 99, 3, 0);

        Levels[lvl].PassedWithPanel = true;

        Levels[lvl].SetMass(
            new int[,] //exist
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
            },
            "exist"
            );
        Levels[lvl].SetMass(
            new int[,] //color
            {
                { 2,4,5,4,4,5,4,1 },
                { 4,1,4,5,5,4,1,4 },
                { 4,4,1,4,4,1,4,4 },
                { 5,5,4,1,1,4,5,5 },
                { 4,4,1,4,4,1,4,4 },
                { 4,1,4,1,1,4,1,4 },
                { 2,1,1,1,1,1,2,1 },
                { 1,2,1,2,1,2,1,2 },
            },
            "color"
            );
        Levels[lvl].SetMass(
            new int[,] //type
            
            {
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,2,1,1,1,1 },
            },
            "type"
            );
        Levels[lvl].SetMass(
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
        Levels[lvl].SetMass(
            new int[,] //rock
            {
                { 0,1,1,1,1,1,1,0 },
                { 1,0,1,1,1,1,0,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,1,1,0,0,1,1,1 },
                { 1,1,0,1,1,0,1,1 },
                { 1,0,1,0,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
            },
            "rock"
            );
        Levels[lvl].SetCells();

 */