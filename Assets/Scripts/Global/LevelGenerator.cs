using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//àëåêñàíäğ
/// <summary>
/// ãåíåğèğóåò ğàíäîìíûå óğîâíè
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator main;
    private int ScoreÑoefficient = 500;
    private int existChance = 5;
    private int boxChance = 10;

    private void Start()
    {
        main = this;
    }
    //ãåíåğàöèÿ óğîâíÿ
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        if (LevelsScript.main.ReturnLevel(NumLevel) != null)
        {
            return LevelsScript.main.ReturnLevel(NumLevel);
        }
        float NoizeResult = Mathf.PerlinNoise(Mathf.Cos(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Sin(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Tan(NumLevel), 0f) * 1000000;
        int Width = (int)NoizeResult % 5 + 5;
        int Height = (int)NoizeResult * 123 % 5 + 5;
        int NeedScore = Width * Height * ((int)NoizeResult % ScoreÑoefficient + ScoreÑoefficient / 2);
        float move = (float)30 / (Width * Height * ScoreÑoefficient) * NeedScore;
        byte[,] exist = new byte[Width, Height];
        int[,] box = new int[Width, Height];
        int[,] mold = new int[Width, Height];
        int[,] IColors = new int[Width, Height];
        int[,] Type = new int[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                int rand = (int)(Mathf.PerlinNoise(i * NumLevel * Mathf.Deg2Rad, j * NumLevel * Mathf.Deg2Rad) * 1000000) % 100;
                if(rand < existChance)
                {
                    exist[i, j] = 0;
                }
                else
                {
                    exist[i, j] = 1;
                }
                
                if(rand < boxChance + existChance)
                {
                    box[i, j] = (int)(Mathf.PerlinNoise(i * NumLevel * Mathf.Deg2Rad, j * NumLevel * Mathf.Deg2Rad) * 1000000) % 4 + 1;
                }
                else
                {
                    box[i, j] = 0;
                }
                IColors[i, j] = (int)(Mathf.PerlinNoise(i * NumLevel * Mathf.Deg2Rad, j * NumLevel * Mathf.Deg2Rad) * 1000000) % 4;
                Type[i, j] = 0;
                mold[i, j] = 0;
            }
        }
        LevelsScript.main.Levels[NumLevel] = LevelsScript.main.CreateLevel(NumLevel, Width, Height, NeedScore, (int)move, exist, box, mold, IColors, Type);
        return LevelsScript.main.Levels[NumLevel];
    }
}
