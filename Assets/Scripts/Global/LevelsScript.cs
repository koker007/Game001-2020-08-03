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
        /// ����������� ���������� �����
        /// </summary>
        public int NeedScore;
        /// <summary>
        /// ����������� ��������� ���������� �����
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

        /// <summary>
        /// ���������� ���������� � ������ �� ������� ������
        /// </summary>
        public CellInfo ReturneCell(Vector2Int PositionOnField)
        {
            try
            {
                return cells[PositionOnField.x, PositionOnField.y];
            }
            catch
            {
                return null;
            }
        }

        public void NewMaxScore(int score)
        {
            if(score > MaxScore)
            {
                MaxScore = score;
            }
        }
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

            new byte[,] //color
            {
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 }
            },
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
            2, 8, 8, 2000000, 1000,


            new byte[,] //type
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
    private Level CreateLevel(int NumLevel, int Width, int Height, int NeedScore, int move, byte[,] exist, int[,] internalColors, int[,] type)
    {
        level = new Level();

        level = new Level { 
            NumLevel = NumLevel, 
            Width = Width, 
            Height = Height,
            NeedScore = NeedScore,
            Move = move
        };

        level.cells = new CellInfo[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if(exist[i,j] == 0)
                {
                    level.cells[i, j] = null;
                }
                else
                {
                    level.cells[i, j] = new CellInfo(internalColors[i, j], type[i, j]);
                }
            }
        }

        return level;
    }

    /// <summary>
    /// ���������� ��������� ���������� ����� �������� ������
    /// </summary>
    public Level ReturnLevel(int NumLevel)
    {
        try
        {
            return Levels[NumLevel];
        }
        catch
        {
            return null;
        }
    }
    public Level ReturnLevel()
    {
        return ReturnLevel(Gameplay.main.levelSelect);
    }

}
