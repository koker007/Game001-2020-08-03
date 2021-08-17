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
    public class Cell
    {
        /// <summary>
        /// ���� ������� � ������
        /// </summary>
        public CellInternalObject.InternalColor colorCell;
        /// <summary>
        /// ��� ������ ������ 
        /// </summary>
        public CellInternalObject.Type typeCell;
        public Cell(int color, int type)
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
        /// ������ ����� �� ����
        /// </summary>
        public Cell[,] cells;
    }

    private Level level;

    public Level[] Levels = new Level[5];

    private void Start()
    {
        main = this;

        //������� 1
        Levels[1] = CreateLevel(
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

        //������� 2
        Levels[2] = CreateLevel(
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

    //�������� ������ (����� ���������� ��� ����������� ��������� ����� ������ � Start)
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
    /// <summary>
    /// ���������� ������ � ������ �������� ����
    /// </summary>
    /// <param name="NumLevel"></param>
    /// <returns></returns>
    public Vector2Int ReturnLevelSize(int NumLevel)
    {
        return new Vector2Int(Levels[NumLevel].Width, Levels[NumLevel].Height);
    }
    /// <summary>
    /// ���������� ���������� � ������
    /// </summary>
    /// <param name="NumLevel"></param>
    /// <param name="PositionOnField"></param>
    /// <returns></returns>
    public Cell ReturneCell(int NumLevel, Vector2Int PositionOnField)
    {
        return Levels[NumLevel].cells[PositionOnField.x, PositionOnField.y];
    }
}
