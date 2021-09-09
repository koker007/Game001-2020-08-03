using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alexandr
/// <summary>
/// хранит данные об уровнях
/// </summary>
public class LevelsScript : MonoBehaviour
{
    public static LevelsScript main;
    /// <summary>
    /// хранит данные о ячейке
    /// </summary>
    public class CellInfo
    {
        /// <summary>
        /// цвет обьекта в ячейке
        /// </summary>
        public CellInternalObject.InternalColor colorCell;
        /// <summary>
        /// тип обькта ячейке 
        /// </summary>
        public CellInternalObject.Type typeCell;
        /// <summary>
        /// коробка с жизнями
        /// </summary>
        public int boxHealth;
        public int moldHealth;
        public int Panel;
        public int rock;
        public CellInfo(int box, int mold, int color, int type, int panel, int rockF)
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

            rock = rockF;

            if (panel != 0)
            {
                Panel = panel;
            }
            else if (mold != 0)
            {
                moldHealth = mold;
            }
        }

    }
    /// <summary>
    /// хранит данные о уровне
    /// </summary>
    public class Level
    {
        /// <summary>
        /// ширина игрового поля
        /// </summary>
        public int Width;
        /// <summary>
        /// высота игровго поля
        /// </summary>
        public int Height;
        /// <summary>
        /// номер уровня
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
        /// количество цветов
        /// </summary>
        public int NumColors;
        /// <summary>
        /// массив ячеек на поле
        /// </summary>
        public CellInfo[,] cells;
        ///цели для прохождения уровня
        public bool PassedWitScore = false;
        public bool PassedWithCrystal = false;
        public bool PassedWithBox = false;
        public bool PassedWitMold = false;
        public bool PassedWitPanel = false;
        public bool PassedWitRock = false;

        public int NeedBox;
        public int NeedMold;
        public int NeedPanel;
        public int NeedCrystal;
        public int NeedRock;
        public CellInternalObject.InternalColor NeedColor;

        //массивы обьектов
        int[,] exist;
        int[,] box;
        int[,] mold;
        int[,] panel;
        int[,] internalColors;
        int[,] type;
        int[,] rock;

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

        /// <summary>
        /// новый рекорд
        /// </summary>
        /// <param name="score"></param>
        public void NewMaxScore(int score)
        {
            if(score > MaxScore)
            {
                MaxScore = score;
            }
        }

        /// <summary>
        /// устанавливает значения определенного массива уровня
        /// </summary>
        /// <param name="values"></param>
        /// <param name="massName"></param>
        public void SetMass(int[,] values, string massName)
        {
            switch (massName)
            {
                case "box":
                    box = values;
                    break;
                case "mold":
                    mold = values;
                    break;
                case "type":
                    type = values;
                    break;
                case "color":
                    internalColors = values;
                    break;
                case "panel":
                    panel = values;
                    break;
                case "exist":
                    exist = values;
                    break;
                case "rock":
                    rock = values;
                    break;
            }
        }

        /// <summary>
        /// заносит в каждую ячейку информацию о ней из массивов
        /// </summary>
        public void SetCells()
        {
            if(exist == null)
            {
                massNull(ref exist, 1);
            }
            if (internalColors == null)
            {
                massNull(ref internalColors, 0);
            }
            if (type == null)
            {
                massNull(ref type, 0);
            }
            if (box == null)
            {
                massNull(ref box, 0);
            }
            if (mold == null)
            {
                massNull(ref mold, 0);
            }
            if (panel == null)
            {
                massNull(ref panel, 0);
            }
            if (rock == null) {
                massNull(ref rock, 0);
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (exist[y, x] == 0)
                    {
                        cells[x, cells.GetLength(1) - 1 - y] = null;
                    }
                    else
                    {
                        cells[x, cells.GetLength(1) - 1 - y] = new CellInfo(box[y, x], mold[y, x], internalColors[y, x], type[y, x], panel[y, x], rock[y,x]);
                    }
                }
            }
        }

        /// <summary>
        /// заполняет массив указанным значением
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="value"></param>
        private void massNull(ref int[,] mas, int value)
        {
            mas = new int[Height,Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    mas[y, x] = value;
                }
            }
        }
    }

    private Level level;
    //все уровни
    public Level[] Levels = new Level[1000];

    private void Start()
    {
        main = this;

        //уровень 1
        Levels[1] = CreateLevel(1, 6, 5, 2000000, 50, 4);

        Levels[1].PassedWitMold = true;
        Levels[1].PassedWitPanel = true;

        Levels[1].SetMass(
        new int[,] //exist
            {
                { 0,0,1,0,0 },
                { 0,0,1,0,0 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,1 },
                { 1,1,1,1,0 }
            },
            "exist"
            );
        Levels[1].SetMass(
            new int[,] //color
            {
                { 4,3,0,3,0 },
                { 4,1,2,3,0 },
                { 0,1,3,1,3 },
                { 0,4,0,1,0 },
                { 1,1,2,0,0 },
                { 0,1,2,3,0 }
            },
            "color"
            );
        Levels[1].SetMass(
            new int[,] //type
            {
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,5,0,0 },
                { 0,0,4,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
            },
            "type"
            );
        Levels[1].SetMass(
            new int[,] //mold
            {
                { 1,0,1,0,1 },
                { 1,0,1,0,1 },
                { 0,1,1,1,0 },
                { 1,1,1,1,1 },
                { 0,1,1,1,0 },
                { 1,0,1,0,1 }
            },
            "mold"
            );
        Levels[1].SetMass(
           new int[,]
           {
                { 0,1,0,1,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 }
           },
           "panel"
           );
        Levels[1].SetCells();

        //уровень 2
        Levels[2] = CreateLevel(2, 8, 8, 2000000, 1000, 4);

        Levels[2].PassedWitScore = true;

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
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 }
            },
            "type"
            );
        Levels[2].SetCells();

        //уровень 3
        Levels[3] = CreateLevel(3, 10, 10, 10000, 99, 5);

        Levels[3].PassedWitScore = true;

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
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1 }
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
                { 1,1,1,1,5,5,1,1,1,1 },
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
                { 5,4,0,0,0,0,0,0,0,0 },
                { 3,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,3,3,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
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
                { 5,5,5,5,5,5,5,5,5,5 }
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
                { 0,0,0,0,0,0,0,0,0,0 },
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
                { 0,0,0,0,1,1,0,0,0,0 },
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

    //заполнение основных параметров уровня
    public Level CreateLevel
        (
        int NumLevel, 
        int Width, 
        int Height, 
        int NeedScore, 
        int move,
        int numColors
        )
    {
        level = new Level {
            NumLevel = NumLevel,
            Width = Height,
            Height = Width,
            NeedScore = NeedScore,
            Move = move,
            NumColors = numColors
        };

        level.cells = new CellInfo[Height, Width];

        return level;
    }

    /// <summary>
    /// возвращает стартовое количество ходов текущего уровня
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
    /// <summary>
    /// возвращает стартовое количество ходов текущего уровня
    /// </summary>
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