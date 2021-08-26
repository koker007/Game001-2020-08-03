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
    private float noizeScale = 0.04f;

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
        int Width = (int)NoizeResult % 5 + 8;
        int Height = (int)NoizeResult * 123 % 5 + 8;
        int NeedScore = Width * Height * ((int)NoizeResult % ScoreÑoefficient + ScoreÑoefficient / 2);
        float move = (float)30 / (Width * Height * ScoreÑoefficient) * NeedScore;
        LevelsScript.PassedType passedType = (LevelsScript.PassedType)(NoizeResult % 3);
        byte[,] exist = new byte[Width, Height];
        int[,] box = new int[Width, Height];
        int[,] mold = new int[Width, Height];
        int[,] IColors = new int[Width, Height];
        int[,] Type = new int[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float randExist = Mathf.PerlinNoise((x + NumLevel * 100) * Mathf.PI * noizeScale * 3, (y + NumLevel) * Mathf.PI * noizeScale);
                float randBox = Mathf.PerlinNoise((x + (NumLevel * 2000)) * Mathf.PI * noizeScale * 2, (y + NumLevel) * Mathf.PI * noizeScale);
                float randMold = Mathf.PerlinNoise((x + (NumLevel * 30000)) * Mathf.PI * noizeScale, (y + NumLevel) * Mathf.PI * noizeScale);

                if (randBox > 1 - existChance)
                {
                    exist[x, y] = 0;
                }
                else
                {
                    exist[x, y] = 1;
                }

                if (randBox < boxChance)
                {
                    for(int k = 5; k > 0; k--)
                    {
                        if (randBox < boxChance - 0.04 * (k - 1))
                        {
                            box[x, y] = k;
                            break;
                        }
                    }
                }
                else
                {
                    box[x, y] = 0;
                }

                if (passedType == LevelsScript.PassedType.mold && randMold < moldChance)
                {
                    for (int k = 5; k > 0; k--)
                    {
                        if (randMold < moldChance / k)
                        {
                            mold[x, y] = k;
                            break;
                        }
                    }
                }
                else
                {
                    mold[x, y] = 0;
                }


                IColors[x, y] = (int)(Mathf.PerlinNoise(x * NumLevel * Mathf.Deg2Rad, y * NumLevel * Mathf.Deg2Rad) * 1000000) % 4;
                Type[x, y] = 0;
            }
        }
        /*
        for (int y = 0; y < Height; y++)
        {
            for (int x = Width - 1; x >= 0; x--)
            {
                if (x + 1 < Width)
                {
                    if (exist[x, y] == 0 && exist[x + 1, y] == 1)
                    {
                        exist[x, y] = 1;
                        Debug.Log("exist" + x.ToString() + y.ToString());
                    }
                }
            }
        }*/
        
        LevelsScript.main.Levels[NumLevel] = LevelsScript.main.CreateLevel(NumLevel, Width, Height, NeedScore, (int)move, passedType, exist, box, mold, IColors, Type);
        return LevelsScript.main.Levels[NumLevel];
    }
}
