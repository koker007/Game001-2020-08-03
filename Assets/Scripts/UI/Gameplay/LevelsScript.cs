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
    public class Cell
    {
        /// <summary>
        /// цвет обьекта в €чейке
        /// </summary>
        public CellInternalObject.InternalColor colorCell;
        /// <summary>
        /// тип обькта €чейке 
        /// </summary>
        public CellInternalObject.Type typeCell;
        public Cell(int color, int type)
        {
            colorCell = (CellInternalObject.InternalColor)color;
            typeCell = (CellInternalObject.Type)type;

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
        /// массив €чеек на поле
        /// </summary>
        public Cell[,] cells;
    }

    private Level level;

    public Level Level1 = new Level();
    public Level Level2 = new Level();
    public Level Level3 = new Level();
    public Level Level4 = new Level();
    public Level Level5 = new Level();

    private void Start()
    {
        main = this;

        //уровень 1
        Level1 = CreateLevel(
            1, 5, 5, //numLevel, width. long

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
        Level2 = CreateLevel(
            2, 8, 8, //numLevel, width. long

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

    //создание уровн€ (метод существует дл€ зрительного упрощени€ схемы уровн€ в Start)
    private Level CreateLevel(int NumLevel, int Width, int Height, int[,] internalColors, int[,] type)
    {
        level = new Level();

        level = new Level { NumLevel = NumLevel, Width = Width, Height = Height };
        level.cells = new Cell[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                level.cells[i, j] = new Cell(internalColors[i,j], type[i,j]);
            }
        }

        return level;
    }


}
