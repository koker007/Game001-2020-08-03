
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Alexandr
//јндрей
/// <summary>
/// хранит данные об уровн€х
/// </summary>
public class LevelsScript : MonoBehaviour
{
    private void Start()
    {
        main = this;


    }

    public static LevelsScript main;

    [SerializeField] protected LevelScriptableObject _levelsObject;

    /// <summary>
    /// хранит данные о €чейке
    /// ”ƒјЋя“№ ѕќЋя Ќ≈Ћ№«я »Ќј„≈ —Ћ≈“я“ ”–ќ¬Ќ»
    /// </summary>
    [System.Serializable]
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

        public int Exist;
        /// <summary>
        /// коробка с жизн€ми
        /// </summary>
        public int HealthBox;
        public int HealthMold;
        public int HealthIce;
        public int Panel;
        public int rock;
        public int wall;
        public int teleport;
        public bool dispencer;
        public int underObj;
        public CellInfo()
        {
            colorCell = CellInternalObject.InternalColor.Red;
            typeCell = CellInternalObject.Type.color;
            Exist = 1;
        }
        public CellInfo(CellInfo cell)
        {
            colorCell = cell.colorCell;
            typeCell = cell.typeCell;
            Exist = cell.Exist;
            HealthBox = cell.HealthBox;
            HealthMold = cell.HealthMold;
            HealthIce = cell.HealthIce;
            Panel = cell.Panel;
            rock = cell.rock;
            wall = cell.wall;
            teleport = cell.teleport;
            dispencer = cell.dispencer;
            underObj = cell.underObj;
        }
        public CellInfo(int exist, int box, int mold, int color, int type, int panel, int rockF, int ice, int walls, int disp, int tp, int underObj)
        {
            if (exist != 0)
                Exist = exist;

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

            if (tp > 0)
            {
                teleport = tp;
            }

            if (underObj > 0) {
                this.underObj = underObj;
            }

            dispencer = (disp != 0);
            
        }

    }
    /// <summary>
    /// хранит данные о уровне 
    /// ”ƒјЋя“№ »Ћ» ѕ≈–≈»ћ≈Ќќ¬џ¬ј“№ ѕќЋя Ќ≈Ћ№«я »Ќј„≈ —Ћ≈“я“ ”–ќ¬Ќ»
    /// </summary>
    [System.Serializable]
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
        /// сложность уровн€(от 0 до 100) процент поражений
        /// </summary>
        public int Difficult;
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
        /// ѕроцент выпадени€ блокираторов которые уничтожаютс€ как €щики и не дают распростран€тьс€ взрыву 
        /// </summary>
        public int TypeBlockerPercent = 0;
        

        /// <summary>
        /// массив €чеек на поле
        /// </summary>
        [SerializeField] public CellInfo[,] cells = new CellInfo[0,0];
        [SerializeField] public CellInfo[] cellsArray = new CellInfo[0];

        //способы прохождени€ уровн€
        public bool PassedWithScore = false;
        public bool PassedWithCrystal = false;
        public bool PassedWithBox = false;
        public bool PassedWithMold = false;
        public bool PassedWithIce = false;
        public bool PassedWithPanel = false;
        public bool PassedWithRock = false;
        public bool PassedWithEnemy = false;
        public bool PassedWithUnderObj = false; //«авершение с помощью сбора всех подледных объектов
        //дл€ прохождени€ с помощью сбора кристаллов
        public int NeedCrystal;
        public CellInternalObject.InternalColor NeedColor;

        public int[,] exist;
        public int[,] box;
        public int[,] mold;
        public int[,] ice;
        public int[,] panel;
        public int[,] internalColors;
        public int[,] type;
        public int[,] rock;
        public int[,] walls;
        public int[,] teleport;

        /// <summary>
        /// ¬нимание! если надо создать раздатчик, то €чейка должна быть exist, иначе ничего не создастс€
        /// </summary>
        public int[,] dispencers;
        public int[,] underObjs;
        public Level()
        {
        }

        public Level(Level lev)
        {
            Width = lev.Width;
            Height = lev.Height;
            NumLevel = lev.NumLevel;
            NeedScore = lev.NeedScore;
            MaxScore = lev.MaxScore;
            Difficult = lev.Difficult;
            Move = lev.Move;
            NumColors = lev.NumColors;
            SuperColorPercent = lev.SuperColorPercent;
            TypeBlockerPercent = lev.TypeBlockerPercent;

            cells = new CellInfo[lev.cells.GetLength(0), lev.cells.GetLength(1)];
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i,j] = new CellInfo(lev.cells[i,j]);
                }
            }

            cellsArray = new CellInfo[lev.cellsArray.Length];
            for (int i = 0; i < cellsArray.Length; i++)
            {
                cellsArray[i] = new CellInfo(lev.cellsArray[i]);
            }

            PassedWithScore = lev.PassedWithScore;
            PassedWithCrystal = lev.PassedWithCrystal;
            PassedWithBox = lev.PassedWithBox;
            PassedWithMold = lev.PassedWithMold;
            PassedWithIce = lev.PassedWithIce;
            PassedWithPanel = lev.PassedWithPanel;
            PassedWithRock = lev.PassedWithRock;
            PassedWithRock = lev.PassedWithRock;
            PassedWithEnemy = lev.PassedWithEnemy;
            PassedWithUnderObj = lev.PassedWithUnderObj;

            NeedCrystal = lev.NeedCrystal;
            NeedColor = lev.NeedColor;

            exist = lev.exist;
            box = lev.box;
            mold = lev.mold;
            ice = lev.ice;
            panel = lev.panel;
            internalColors = lev.internalColors;
            type = lev.type;
            rock = lev.rock;
            walls = lev.walls;
            teleport = lev.teleport;
            dispencers = lev.dispencers;
            underObjs = lev.underObjs;
        }
        
        public void ConvertOneCellToTwoCells()
        {
            cells = new CellInfo[Width, Height];
            for(int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Height; x++)
                {
                    try{
                        cells[y, x] = new CellInfo(cellsArray[y + x * Width]);
                    }
                    catch { Debug.Log($"{NumLevel} error arrays"); }
                }
            }
        }
        public void ConvertTwoCellsToOneCells()
        {
            cellsArray = new CellInfo[Height * Width];
            for (int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Height; x++)
                {
                    cellsArray[y + x * Width] = new CellInfo(cells[y, x]);
                }
            }
        }

        /// <summary>
        ///заносит значени€ в массив уровн€
        ///box, mold, ice, type, color, panel, exist, rock, walls, dispencers, teleport
        ///≈сли нужно что-то создать, €чейка должна быть exist
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
                case "dispencers":
                    dispencers = values;
                    break;
                case "teleport":
                    teleport = values;
                    break;
                case "underObjs":
                    underObjs = values;
                    break;

            }
        }
        /// <summary>
        ///заносит значени€ в массив уровн€
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
            if (teleport == null)
            {
                massNull(ref teleport, 0);
            }
            if (dispencers == null)
            {
                massNull(ref dispencers, 0);
            }
            if (underObjs == null) {
                massNull(ref underObjs, 0);
            }


            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    cells[x, cells.GetLength(1) - 1 - y] = new CellInfo(exist[y,x], box[y, x], mold[y, x], internalColors[y, x], type[y, x], panel[y, x], rock[y,x], ice[y,x], walls[y,x], dispencers[y,x], teleport[y,x], underObjs[y,x]);
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
    private Level generatedLevel = new Level();
    private const int mainLevelsCount = 1000;
    public List<Level> Levels = new List<Level>(mainLevelsCount);

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
    /// возвращает стартовое количество ходов текущего уровн€
    /// </summary>
    public Level ReturnLevel()
    {
        if (Gameplay.main.levelSelect < _levelsObject.levels.Count && _levelsObject.levels[Gameplay.main.levelSelect].cellsArray.Length > 0)
        {
            _levelsObject.levels[Gameplay.main.levelSelect].ConvertOneCellToTwoCells();
            return _levelsObject.levels[Gameplay.main.levelSelect];
        }
        else 
        {
            if (generatedLevel.NumLevel != Gameplay.main.levelSelect)
                generatedLevel = new Level(LevelGenerator.main.GenerateLevelV3(Gameplay.main.levelSelect, true));
            return generatedLevel;
        }
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