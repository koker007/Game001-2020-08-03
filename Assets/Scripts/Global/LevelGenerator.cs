using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//àëåêñàíäð
/// <summary>
/// ãåíåðèðóåò ðàíäîìíûå óðîâíè
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
    //ãåíåðàöèÿ óðîâíÿ
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        float RandomFactor = 1.234f * NumLevel;

        if (LevelsScript.main.ReturnLevel(NumLevel) != null)
        {
            return LevelsScript.main.ReturnLevel(NumLevel);
        }
        float NoizeResult = Mathf.PerlinNoise(Mathf.Cos(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Sin(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Tan(NumLevel), 0f) * 1000000;

        int Width = (int)NoizeResult % 8 + 6;
        int Height = (int)NoizeResult * 123 % (int)(Width / 2) + 6;

        int NeedScore = Width * Height * ((int)NoizeResult % ScoreÑoefficient + ScoreÑoefficient / 2);
        float move = (float)30 / (Width * Height * ScoreÑoefficient) * NeedScore;

        int numColors;
        if (Width > 10)
        {
            numColors = 6;
        }
        else if (Width > 8)
        {
            numColors = 5;
        }
        else
        {
            numColors = 4;
        }

        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors);

        PassRandom();

        ArraysRandom();

        LevelsScript.main.Levels[NumLevel] = level;

        return LevelsScript.main.Levels[NumLevel];


        void PassRandom()
        {
            int numPass = (int)NoizeResult % 2 + 1;
            int k = 11;

            for (int i = 0; i < numPass; i++)
            {
                k++;
                int rand = (int)NoizeResult * k % 5;
                if (rand == 0 && i == 0)
                {
                    level.PassedWitScore = true;
                }
                else if (rand == 1 && !level.PassedWitScore)
                {
                    level.PassedWithCrystal = true;
                    level.NeedCrystal = (int)NoizeResult % 15 + 15;
                    level.NeedColor = (CellInternalObject.InternalColor)(int)(NoizeResult % 5);
                }
                else if (rand == 2 && !level.PassedWitScore)
                {
                    level.PassedWithBox = true;
                }
                else if (rand == 3 && !level.PassedWitScore && !level.PassedWitPanel)
                {
                    level.PassedWitMold = true;
                }
                else if (rand == 4 && !level.PassedWitScore && !level.PassedWitMold)
                {
                    level.PassedWitPanel = true;
                }
            }
        }

        void ArraysRandom()
        {
            int[,] exist = new int[Height, Width];
            int[,] box = new int[Height, Width];
            int[,] mold = new int[Height, Width];
            int[,] IColors = new int[Height, Width];
            int[,] Type = new int[Height, Width];

            PerlinAllRandom();

            ExistRandom();

            level.SetMass(exist, "exist");
            level.SetMass(IColors, "color");
            level.SetMass(Type, "type");
            level.SetMass(box, "box");
            level.SetMass(mold, "mold");
            //level.SetMass(panel, "panel");
            level.SetCells();

            void ExistRandom()
            {
                SetArray(ref exist, 1);

                bool upInjections = false;
                bool downInjections = false;
                bool Sidecenter = false;
                bool center = false;
                bool doubleCenter = false;

                upInjections = RandomBool(1);
                downInjections = RandomBool(1);
                if (!upInjections && !downInjections)
                {
                    Sidecenter = RandomBool(0.7f);
                }

                if(Width >= 8)
                {
                    if(!Sidecenter)
                    {
                        center = RandomBool(0.7f);
                    }
                }

                if (Width >= 10)
                {
                    if (!center)
                    {
                        doubleCenter = RandomBool(0.7f);
                    }
                }



                if (upInjections)
                {
                    int heightExist = Height / 2 - 1;
                    for (int x = 0; x < (Width + 1) / 2; x++)
                    {
                        if (heightExist < 0)
                        {
                            break;
                        }
                        heightExist = RandomInt(heightExist);
                        for (int y = heightExist; y >= 0; y--)
                        {
                            exist[y, x] = 0;
                            Debug.Log(x + " " + y);
                        }
                        heightExist--;
                    }
                }
                if (downInjections)
                {
                    int heightExist = Height / 2;
                    for (int x = 0; x < (Width + 1) / 2; x++)
                    {
                        if (heightExist >= Height)
                        {
                            break;
                        }
                        heightExist += RandomInt(Width - 1 - heightExist);
                        for (int y = heightExist; y < Height; y++)
                        {
                            exist[y, x] = 0;
                            Debug.Log(x + " " + y);
                        }
                        heightExist++;
                    }
                }
                /*
                if (center)
                {
                    int widthExist = (Width + 1) / 2 - 4;
                    int centerHeightExist = Height - RandomInt(Height - Height / 3 - 1);
                    int heightExist;
                    for (int x = (Width + 1) / 2; x >= ((Width + 1) / 2) - (widthExist / 2); x--)
                    {
                        if (heightExist >= Height)
                        {
                            break;
                        }
                        heightExist += RandomInt(Width - 1 - heightExist);
                        for (int y = heightExist; y < Height; y++)
                        {
                            exist[y, x] = 0;
                            Debug.Log(x + " " + y);
                        }
                        heightExist++;
                    }
                }*/
            }

            void SetArray(ref int[,] arr, int value)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        arr[y, x] = value;
                    }
                }
            }

            void PerlinAllRandom()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        float randExist = Mathf.PerlinNoise((x + NumLevel * 100) * Mathf.PI * noizeScale * 3, (y + NumLevel) * Mathf.PI * noizeScale);
                        float randBox = Mathf.PerlinNoise((x + (NumLevel * 2000)) * Mathf.PI * noizeScale * 2, (y + NumLevel) * Mathf.PI * noizeScale);
                        float randMold = Mathf.PerlinNoise((x + (NumLevel * 30000)) * Mathf.PI * noizeScale, (y + NumLevel) * Mathf.PI * noizeScale);

                        if (randBox > 1 - existChance)
                        {
                            exist[y, x] = 0;
                        }
                        else
                        {
                            exist[y, x] = 1;
                        }

                        if (randBox < boxChance)
                        {
                            for (int k = 5; k > 0; k--)
                            {
                                if (randBox < boxChance - 0.04 * (k - 1))
                                {
                                    box[y, x] = y;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            box[y, x] = 0;
                        }

                        if (level.PassedWitMold && randMold < moldChance)
                        {
                            for (int k = 5; k > 0; k--)
                            {
                                if (randMold < moldChance / k)
                                {
                                    mold[y, x] = k;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            mold[y, x] = 0;
                        }


                        IColors[y, x] = (int)(Mathf.PerlinNoise(x * NumLevel * Mathf.Deg2Rad, y * NumLevel * Mathf.Deg2Rad) * 1000000) % 4;
                        Type[y, x] = 0;
                    }
                }
            }
        }
        int RandomInt(int maxValue)
        {
            RandomFactor *= RandomFactor;
            return Mathf.Abs((int)(NoizeResult * RandomFactor) % (maxValue + 1));
        }
        bool RandomBool(float chance)
        {
            RandomFactor *= RandomFactor;
            if (NoizeResult * RandomFactor % 1 <= chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
