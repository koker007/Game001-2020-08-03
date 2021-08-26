using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alexandr
/// <summary>
/// хранит данные об уровн€х
/// </summary>
public class LevelsScript : MonoBehaviour
{
    public static LevelsScript main;
    /// <summary>
    /// хранит данные о €чейке
    /// </summary>
    public enum PassedType
    {
        score,
        box,
        mold
    }
    public class CellInfo
    {
        /// <summary>
        /// цвет обьекта в €чейке
        /// </summary>
        public CellInternalObject.InternalColor colorCell;
        /// <summary>
        /// тип обькта €чейке 
        /// </summary>
        public CellInternalObject.Type typeCell;
        /// <summary>
        /// коробка с жизн€ми
        /// </summary>
        public int boxHealth;
        public int moldHealth;
        public CellInfo(int box, int mold, int color, int type)
        {
            if (box != 0)
            {
                boxHealth = box;
            }
            else
            {
                colorCell = (CellInternalObject.InternalColor)color;
                typeCell = (CellInternalObject.Type)type;
            }

            if (mold != 0)
            {
                moldHealth = mold;
            }
        }
        public void Cell(int mold)
        {
            moldHealth = mold;
        }
        public void CellBox(int boxhealth)
        {
            boxHealth = boxhealth;
        }

    }
    /// <summary>
    /// хранит данные о уровне
    /// </summary>
    public class Level
    {
        /// <summary>
        /// ширина игрового пол€
        /// </summary>
        public int Width;
        /// <summary>
        /// высота игровго пол€
        /// </summary>
        public int Height;
        /// <summary>
        /// номер уровн€
        /// </summary>
        public int NumLevel;
        /// <summary>
        /// необходимое количество очков
        /// </summary>
        public int NeedScore;
        /// <summary>
        /// максимально набранное количество очков
        /// </summary>
        public int MaxScore;
        /// <summary>
        /// количество ходов
        /// </summary>
        public int Move;
        /// <summary>
        /// массив €чеек на поле
        /// </summary>
        public CellInfo[,] cells;

        public PassedType pasType;

        public int NeedBox;
        public int NeedMold;


        /// <summary>
        /// возвращает информацию о клетке на текущем уровне
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

        //уровень 1
        Levels[1] = CreateLevel(
            //numLevel, width, long, max score, move, passType
            1, 5, 5, 2000000, 50, PassedType.mold,

            new int[,] //mold
            {
                { 1,0,1,0,1 },
                { 0,1,1,1,0 },
                { 1,1,1,1,1 },
                { 0,1,1,1,0 },
                { 1,0,1,0,1 }
            },

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

        //уровень 2
        Levels[2] = CreateLevel(
            //numLevel, width, long, max score, move, passType
            2, 8, 8, 2000000, 1000, PassedType.box,


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
            }
            );

        //уровень 3
        Levels[3] = CreateLevel(
            //numLevel, width, long, max score, move, passType
            3, 10, 10, 10000, 100, PassedType.score,


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
            },

            new int[,] //box
            {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,1,0,1,0,1,0,5,5 },
                { 3,3,0,3,3,3,0,3,3,3 },
                { 5,5,5,0,5,0,5,5,5,5 },
                { 5,5,5,5,0,5,5,5,5,5 },
                { 5,5,5,5,5,0,5,5,5,5 },
                { 5,5,5,5,5,5,0,5,5,5 },
                { 5,5,5,5,5,5,5,0,0,0 },
                { 5,5,5,5,5,5,5,0,0,0 }
            });
    }

    //создание уровн€ (метод существует дл€ зрительного упрощени€ схемы уровн€ в Start)
    public Level CreateLevel
        (
        int NumLevel, 
        int Width, 
        int Height, 
        int NeedScore, 
        int move, 
        PassedType pasType,
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
            Move = move,
            pasType = pasType
        };

        level.cells = new CellInfo[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (exist[j, i] == 0)
                {
                    level.cells[i, j] = null;
                }
                else
                {
                    level.cells[i, j] = new CellInfo(box[j, i], mold[j, i], internalColors[j, i], type[j, i]);
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
       PassedType pasType,
       byte[,] exist,
       int[,] internalColors,
       int[,] type
       )
    {
        int[,] box = new int[Width, Height];
        int[,] mold = new int[Width, Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                box[j, i] = 0;
                mold[j, i] = 0;
            }
        }
        return CreateLevel(NumLevel, Width, Height, NeedScore, move, pasType, exist, box, mold, internalColors, type);
    }

    public Level CreateLevel
       (
       int NumLevel,
       int Width,
       int Height,
       int NeedScore,
       int move,
       PassedType pasType,
       byte[,] exist,
       int[,] internalColors,
       int[,] type,
       int[,] box
       )
    {
        int[,] mold = new int[Width, Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                mold[j, i] = 0;
            }
        }
        return CreateLevel(NumLevel, Width, Height, NeedScore, move, pasType, exist, box, mold, internalColors, type);
    }
    public Level CreateLevel
       (
       int NumLevel,
       int Width,
       int Height,
       int NeedScore,
       int move,
       PassedType pasType,
       int[,] mold,
       byte[,] exist,
       int[,] internalColors,
       int[,] type
       )
    {
        int[,] box = new int[Width, Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                box[j, i] = 0;
            }
        }
        return CreateLevel(NumLevel, Width, Height, NeedScore, move, pasType, exist, box, mold, internalColors, type);
    }

    /// <summary>
    /// возвращает стартовое количество ходов текущего уровн€
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