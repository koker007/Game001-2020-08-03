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
        public int rock;
        public bool upWall;
        public bool downWall;
        public bool leftWall;
        public bool rightWall;
        public CellInfo(int box, int mold, int color, int type, int panel, int rockF, char[] walls)
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

            for(int i = 0; i < walls.Length; i++)
            {
                switch (walls[i])
                {
                    case 'U':
                        upWall = true;
                        break;
                    case 'D':
                        downWall = true;
                        break;
                    case 'R':
                        rightWall = true;
                        break;
                    case 'L':
                        leftWall = true;
                        break;
                }
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
        /// ѕроцент выпадени€ супер цвета от 0 до 100
        /// </summary>
        public int SuperColorPercent = 0;
        /// <summary>
        /// массив €чеек на поле
        /// </summary>
        public CellInfo[,] cells;
        //способы прохождени€ уровн€
        public bool PassedWithScore = false;
        public bool PassedWithCrystal = false;
        public bool PassedWithBox = false;
        public bool PassedWithMold = false;
        public bool PassedWithPanel = false;
        public bool PassedWithRock = false;
        //дл€ прохождени€ с помощью сбора кристаллов
        public int NeedCrystal;
        public CellInternalObject.InternalColor NeedColor;

        int[,] exist;
        int[,] box;
        int[,] mold;
        int[,] panel;
        int[,] internalColors;
        int[,] type;
        int[,] rock;
        string[,] walls;

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
        /// <summary>
        ///заносит значени€ в массив уровн€
        /// </summary>
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
        ///заносит значени€ в массив уровн€
        /// </summary>
        public void SetMass(string[,] values, string massName)
        {
            switch (massName)
            {
                case "walls":
                    walls = values;
                    break;
            }
        }

        /// <summary>
        ///устанавливает в массивы дефолтные значени€
        ///заносит информацию о клетках из массивов
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
                massNull(ref type, 1);
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
            if (walls == null)
            {
                massNull(ref walls);
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
                        cells[x, cells.GetLength(1) - 1 - y] = new CellInfo(box[y, x], mold[y, x], internalColors[y, x], type[y, x], panel[y, x], rock[y,x], walls[y,x].ToCharArray());
                    }
                }
            }
        }

        /// <summary>
        ///заносит в €чейки массиав определенное значение
        /// </summary>
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
        /// <summary>
        ///заносит в €чейки массиав определенное значение
        /// </summary>
        private void massNull(ref string[,] mas)
        {
            mas = new string[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    mas[y, x] = " ";
                }
            }
        }
    }

    private Level level;

    public Level[] Levels = new Level[1000];

    private void Start()
    {
        main = this;

        
    }

    /// <summary>
    ///создание уровн€(метод существует дл€ зрительного упрощени€ схемы уровн€ в Start)
    /// </summary>
    public Level CreateLevel
        (
        int NumLevel,
        int Width,
        int Height,
        int NeedScore,
        int move,
        int numColors,
        int superColorPercent
        )
    {
        level = new Level {
            NumLevel = NumLevel,
            Width = Height,
            Height = Width,
            NeedScore = NeedScore,
            Move = move,
            NumColors = numColors,
            SuperColorPercent = superColorPercent
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
    /// <summary>
    /// возвращает стартовое количество ходов текущего уровн€
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