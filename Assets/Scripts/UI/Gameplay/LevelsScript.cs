using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alexandr
/// <summary>
/// ������ ������ �� �������
/// </summary>
public class LevelsScript : MonoBehaviour
{
    public static LevelsScript main;
    /// <summary>
    /// ������ ������ � ������
    /// </summary>
    public class CellInfo
    {
        /// <summary>
        /// ���� ������� � ������
        /// </summary>
        public CellInternalObject.InternalColor colorCell;
        /// <summary>
        /// ��� ������ ������ 
        /// </summary>
        public CellInternalObject.Type typeCell;
        public CellInfo(int color, int type)
        {
            colorCell = (CellInternalObject.InternalColor)color;
            typeCell = (CellInternalObject.Type)type;

        }
    }
    /// <summary>
    /// ������ ������ � ������
    /// </summary>
    public class Level
    {
        /// <summary>
        /// ������ �������� ����
        /// </summary>
        public int Width;
        /// <summary>
        /// ������ ������� ����
        /// </summary>
        public int Height;
        /// <summary>
        /// ����� ������
        /// </summary>
        public int NumLevel;
        /// <summary>
        /// ���������� �����
        /// </summary>
        public int MaxScore;
        /// <summary>
        /// ���������� �����
        /// </summary>
        public int Move;
        /// <summary>
        /// ������ ����� �� ����
        /// </summary>
        public CellInfo[,] cells;
    }

    private Level level;

    public Level[] Levels = new Level[5];

    private void Start()
    {
        main = this;

        //������� 1
        Levels[1] = CreateLevel(
            //numLevel, width, long, max score, move
            1, 5, 5, 2000000, 50,

            new int[,] //color
            {
                { 0,3,2,3,0 },
                { 3,1,2,1,3 },
                { 1,0,1,0,1 },
                { 3,1,2,1,3 },
                { 0,3,2,3,0 }
            },

            new int[,] //type
            {
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            });

        //������� 2
        Levels[2] = CreateLevel(
            //numLevel, width, long, max score, move
            2, 8, 8, 50000, 20,

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

            new int[,] //type
            {
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 }

            });
    }

    //�������� ������ (����� ���������� ��� ����������� ��������� ����� ������ � Start)
    private Level CreateLevel(int NumLevel, int Width, int Height, int maxScore, int move, int[,] internalColors, int[,] type)
    {
        level = new Level();

        level = new Level { 
            NumLevel = NumLevel, 
            Width = Width, 
            Height = Height,
            MaxScore = maxScore,
            Move = move
        };

        level.cells = new CellInfo[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                level.cells[i, j] = new CellInfo(internalColors[i,j], type[i,j]);
            }
        }

        return level;
    }
    /// <summary>
    /// ���������� ������ � ������ �������� ���� �������� ������
    /// </summary>
    public Vector2Int ReturnLevelSize()
    {
        try
        {
            return new Vector2Int(Levels[Gameplay.main.levelSelect].Width, Levels[Gameplay.main.levelSelect].Height);
        }
        catch
        {
            return new Vector2Int(Levels[1].Width, Levels[1].Height);
        }
    }
    /// <summary>
    /// ���������� ���������� � ������ �� ������� ������
    /// </summary>
    public CellInfo ReturneCell(Vector2Int PositionOnField)
    {
        try
        {
            return Levels[Gameplay.main.levelSelect].cells[PositionOnField.x, PositionOnField.y];
        }
        catch
        {
            return Levels[1].cells[PositionOnField.x, PositionOnField.y];
        }
    }

    /// <summary>
    /// ���������� ��������� ���������� ����� �������� ������
    /// </summary>
    public int ReturnMove()
    {
        try
        {
            return Levels[Gameplay.main.levelSelect].Move;
        }
        catch
        {
            return Levels[1].Move;
        }
    }

    /// <summary>
    /// ���������� ������������ ����������� ���������� ����� �������� ������
    /// </summary>
    public int ReturnMaxScore()
    {
        try
        {
            return Levels[Gameplay.main.levelSelect].MaxScore;
        }
        catch
        {
            return Levels[1].MaxScore;
        }
    }
}
