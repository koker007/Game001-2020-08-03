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
        /// <summary>
        /// ������� � �������
        /// </summary>
        public int boxHealth;
        public int moldHealth;
        public CellInfo(int color, int type)
        {
            colorCell = (CellInternalObject.InternalColor)color;
            typeCell = (CellInternalObject.Type)type;
        }
        public CellInfo(int color, int type , int mold)
        {
            moldHealth = mold;
            colorCell = (CellInternalObject.InternalColor)color;
            typeCell = (CellInternalObject.Type)type;
        }
        public CellInfo(int boxhealth)
        {
            boxHealth = boxhealth;
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

    public Level[] Levels = new Level[1000];

    private void Start()
    {
        main = this;

        //������� 1
        Levels[1] = CreateLevel(
            //numLevel, width, long, max score, move
            1, 5, 5, 2000000, 50,

            new byte[,] //exist
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


            new byte[,] //exist
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

        //������� 3
        Levels[3] = CreateLevel(
            //numLevel, width, long, max score, move
            3, 10, 10, 10000, 10,


            new byte[,] //exist
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
                { 1,1,1,1,1,1,1,1,1,1 }
            },

            new int[,] //type
            {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
            });
    }

    //�������� ������ (����� ���������� ��� ����������� ��������� ����� ������ � Start)
    public Level CreateLevel
        (
        int NumLevel, 
        int Width, 
        int Height, 
        int NeedScore, 
        int move, 
        byte[,] exist, 
        int[,] box,
        int[,] mold,
        int[,] internalColors, 
        int[,] type
        )
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
                else if (box[i, j] != 0)
                {
                    level.cells[i, j] = new CellInfo(box[i,j]);
                }
                else if (mold[i, j] != 0)
                {
                    level.cells[i, j] = new CellInfo(internalColors[i, j], type[i, j], mold[i, j]);
                }
                else
                {
                    level.cells[i, j] = new CellInfo(internalColors[i, j], type[i, j]);
                }
            }
        }

        return level;
    }
    public Level CreateLevel
       (
       int NumLevel,
       int Width,
       int Height,
       int NeedScore,
       int move,
       byte[,] exist,
       int[,] internalColors,
       int[,] type
       )
    {
        int[,] box = new int[Width, Height];
        int[,] mold = new int[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                box[i, j] = 0;
                mold[i, j] = 0;
            }
        }
        return CreateLevel(NumLevel, Width, Height, NeedScore, move, exist, box, mold, internalColors, type);
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
            return LevelGenerator.main.GenerateLevel(NumLevel);
        }
    }
    public Level ReturnLevel()
    {
        return ReturnLevel(Gameplay.main.levelSelect);
    }


    private void Update()
    {
    }

}

/*
 

                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0 },
                { 0,0 },
                { 0 },

                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1 },
                { 1,1,1 },
                { 1,1 },
                { 1 },




*/