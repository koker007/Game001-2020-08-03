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
    private float existChance = 0.3f;
    private float boxChance = 0.4f;
    private float moldChance = 0.5f;

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
        LevelsScript.PassedType passedType = (LevelsScript.PassedType)(NoizeResult % 3);
        byte[,] exist = new byte[Width, Height];
        int[,] box = new int[Width, Height];
        int[,] mold = new int[Width, Height];
        int[,] IColors = new int[Width, Height];
        int[,] Type = new int[Width, Height];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                float rand = Mathf.PerlinNoise(j * NumLevel * Mathf.PI * 0.005f, i * NumLevel * Mathf.PI * 0.008f);
                if(rand < existChance)
                {
                    exist[j, i] = 0;
                }
                else
                {
                    exist[j, i] = 1;
                }

                rand = Mathf.PerlinNoise(j * (NumLevel+1) * Mathf.PI * 0.005f, i * NumLevel * Mathf.PI * 0.008f);
                if (rand < boxChance)
                {
                    for(int k = 5; k > 0; k--)
                    {
                        if (rand < boxChance / k)
                        {
                            box[j, i] = k;
                            break;
                        }
                    }
                }
                else
                {
                    box[j, i] = 0;
                }

                rand = Mathf.PerlinNoise(j * (NumLevel + 2) * Mathf.PI * 0.008f, i * NumLevel * Mathf.PI * 0.008f);
                if (passedType == LevelsScript.PassedType.mold && rand < moldChance)
                {
                    for (int k = 5; k > 0; k--)
                    {
                        if (rand < boxChance / k)
                        {
                            mold[j, i] = k;
                            break;
                        }
                    }
                }
                else
                {
                    mold[j, i] = 0;
                }


                IColors[j, i] = (int)(Mathf.PerlinNoise(j * NumLevel * Mathf.Deg2Rad, i * NumLevel * Mathf.Deg2Rad) * 1000000) % 4;
                Type[j, i] = 0;
            }
        }
        LevelsScript.main.Levels[NumLevel] = LevelsScript.main.CreateLevel(NumLevel, Width, Height, NeedScore, (int)move, passedType, exist, box, mold, IColors, Type);
        return LevelsScript.main.Levels[NumLevel];
    }
}
