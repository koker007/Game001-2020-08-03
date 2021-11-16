using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alexandr
//������
/// <summary>
/// ������ ������ �� �������
/// </summary>
public class LevelsScript : MonoBehaviour
{
    private void Start()
    {
        main = this;


    }

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
        public int HealthBox;
        public int HealthMold;
        public int HealthIce;
        public int Panel;
        public int rock;
        public int wall;

        public CellInfo(int box, int mold, int color, int type, int panel, int rockF, int ice, int walls)
        {
            if (box != 0)
            {
                HealthBox = box;
            }
            else
            {
                colorCell = (CellInternalObject.InternalColor)color;
                typeCell = (CellInternalObject.Type)type;
            }

            rock = rockF;

            if (ice > 0) {
                HealthIce = ice;
            }

            if (panel != 0)
            {
                Panel = panel;
            }
            else if (mold != 0)
            {
                HealthMold = mold;
            }

            if (walls > 0)
            {
                wall = walls;
            }
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
        /// ���������� ������
        /// </summary>
        public int NumColors;
        /// <summary>
        /// ������� ��������� ����� ����� �� 0 �� 100
        /// </summary>
        public int SuperColorPercent = 0;

        /// <summary>
        /// ������� ��������� ������������ ������� ������������ ��� ����� � �� ���� ���������������� ������ 
        /// </summary>
        public int TypeBlockerPercent = 0;
        

        /// <summary>
        /// ������ ����� �� ����
        /// </summary>
        public CellInfo[,] cells;
        //������� ����������� ������
        public bool PassedWithScore = false;
        public bool PassedWithCrystal = false;
        public bool PassedWithBox = false;
        public bool PassedWithMold = false;
        public bool PassedWithIce = false;
        public bool PassedWithPanel = false;
        public bool PassedWithRock = false;
        public bool PassedWithEnemy = false;
        //��� ����������� � ������� ����� ����������
        public int NeedCrystal;
        public CellInternalObject.InternalColor NeedColor;

        int[,] exist;
        int[,] box;
        int[,] mold;
        int[,] ice;
        int[,] panel;
        int[,] internalColors;
        int[,] type;
        int[,] rock;
        int[,] walls;

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
        /// <summary>
        ///������� �������� � ������ ������
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
                case "ice":
                    ice = values;
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
                case "walls":
                    walls = values;
                    break;
            }
        }
        /// <summary>
        ///������� �������� � ������ ������
        /// </summary>
         
        /*
        public void SetMass(string[,] values, string massName)
        {
            switch (massName)
            {
                case "walls":
                    walls = values;
                    break;
            }
        }
        */
        /// <summary>
        ///������������� � ������� ��������� ��������
        ///������� ���������� � ������� �� ��������
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
            if (ice == null) {
                massNull(ref ice, 0);
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
                massNull(ref walls, 0);
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
                        cells[x, cells.GetLength(1) - 1 - y] = new CellInfo(box[y, x], mold[y, x], internalColors[y, x], type[y, x], panel[y, x], rock[y,x], ice[y,x], walls[y,x]);
                    }
                }
            }
           
        }

        /// <summary>
        ///������� � ������ ������� ������������ ��������
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
        ///������� � ������ ������� ������������ ��������
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
    private const int mainLevelsCount = 51;
    public Level[] Levels = new Level[mainLevelsCount];



    /// <summary>
    ///�������� ������(����� ���������� ��� ����������� ��������� ����� ������ � Start)
    /// </summary>
    public Level CreateLevel
        (
        int NumLevel,
        int Width,
        int Height,
        int NeedScore,
        int move,
        int numColors,
        int superColorPercent,
        int typeBlockerPercent
        )
    {
        level = new Level {
            NumLevel = NumLevel,
            Width = Height,
            Height = Width,
            NeedScore = NeedScore,
            Move = move,
            NumColors = numColors,
            SuperColorPercent = superColorPercent,
            TypeBlockerPercent = typeBlockerPercent
            
        };

        level.cells = new CellInfo[Height, Width];

        return level;
    }

    /// <summary>
    /// ���������� ��������� ���������� ����� �������� ������
    /// </summary>
    public Level ReturnLevel()
    {
        int NumLevel = Gameplay.main.levelSelect;
        Level result;

        if (NumLevel < Levels.Length && Levels[NumLevel] != null)
        {
            result = Levels[NumLevel];
        }
        else
        {
            result = LevelGenerator.main.GenerateLevelV2(NumLevel);
        }
        return result;
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