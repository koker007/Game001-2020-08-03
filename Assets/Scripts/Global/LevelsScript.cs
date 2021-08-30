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
        public int Panel;
        public CellInfo(int box, int mold, int color, int type, int panel)
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
        /// количество цветов
        /// </summary>
        public int NumColors;
        /// <summary>
        /// массив €чеек на поле
        /// </summary>
        public CellInfo[,] cells;

        public bool PassedWitScore = false;
        public bool PassedWithCrystal = false;
        public bool PassedWithBox = false;
        public bool PassedWitMold = false;
        public bool PassedWitPanel = false;

        public int NeedBox;
        public int NeedMold;
        public int NeedPanel;
        public int NeedCrystal;
        public CellInternalObject.InternalColor NeedColor;


        int[,] exist;
        int[,] box;
        int[,] mold;
        int[,] panel;
        int[,] internalColors;
        int[,] type;

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
            }
        }

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
                        cells[x, cells.GetLength(1) - 1 - y] = new CellInfo(box[y, x], mold[y, x], internalColors[y, x], type[y, x], panel[y, x]);
                    }
                }
            }
        }

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
                { 0,1,1,1,1 },
                { 1,1,1,1,1 },
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
                { 0,3,0,3,0 },
                { 0,3,2,3,0 },
                { 3,1,2,1,3 },
                { 1,0,1,0,1 },
                { 3,1,2,1,3 },
                { 0,3,2,3,0 }
            },
            "color"
            );
        Levels[1].SetMass(
            new int[,] //type
            {
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
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
        Levels[3] = CreateLevel(3, 10, 10, 10000, 99, 4);

        Levels[3].PassedWitScore = true;

        Levels[3].SetMass(
        new int[,] //exist
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
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,1,3,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,1,4,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 1,1,2,2,3,3,4,4,1,5 }
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
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 }
             },
             "box"
             );
        Levels[3].SetCells();
    }

    //создание уровн€ (метод существует дл€ зрительного упрощени€ схемы уровн€ в Start)
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