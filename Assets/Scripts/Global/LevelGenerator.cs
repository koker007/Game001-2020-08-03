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
    private float panelChance = 0.2f;
    private float noizeScale = 0.04f;

    private void Start()
    {
        main = this;
    }
    //ãåíåðàöèÿ óðîâíÿ
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        float RandomFactor = 1.234567f * NumLevel;
        RandomFactor = Mathf.PerlinNoise(0, RandomFactor) + 1;

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
            numColors = 5;
        }
        else
        {
            numColors = 4;
        }


        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, (int)(NoizeResult * 100));

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
                if (rand == 0 && !level.PassedWithBox)
                {
                    level.PassedWithRock = true;
                }
                else if (rand == 1 && !level.PassedWithScore)
                {
                    level.PassedWithCrystal = true;
                    level.NeedCrystal = (int)NoizeResult % 15 + 15;
                    level.NeedColor = (CellInternalObject.InternalColor)(int)(NoizeResult % 5);
                }
                else if (rand == 2 && !level.PassedWithScore && !level.PassedWithRock)
                {
                    level.PassedWithBox = true;
                }
                else if (rand == 3 && !level.PassedWithScore && !level.PassedWithPanel)
                {
                    level.PassedWithMold = true;
                }
                else if (rand == 4 && !level.PassedWithScore && !level.PassedWithMold)
                {
                    level.PassedWithPanel = true;
                }
            }
        }

        void ArraysRandom()
        {
            int[,] exist = new int[Height, Width];
            int[,] box = new int[Height, Width];
            int[,] mold = new int[Height, Width];
            int[,] panel = new int[Height, Width];
            int[,] rock = new int[Height, Width];
            int[,] IColors = new int[Height, Width];
            int[,] Type = new int[Height, Width];

            PerlinAllRandom();

            CrystalRandom();
            ExistRandom();
            BoxRandom();
            MoldRandom();
            PanelRandom();

            level.SetMass(exist, "exist");
            level.SetMass(IColors, "color");
            level.SetMass(Type, "type");
            level.SetMass(box, "box");
            level.SetMass(mold, "mold");
            level.SetMass(panel, "panel");
            level.SetMass(rock, "rock");
            level.SetCells();

            void CrystalRandom()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        IColors[y, x] = (int)(Mathf.PerlinNoise(x * NumLevel * Mathf.Deg2Rad, y * NumLevel * Mathf.Deg2Rad) * 1000000) % numColors;
                        Type[y, x] = 1;
                    }
                }
            }

            void ExistRandom()
            {
                SetArray(ref exist, 1);

                bool upInjections = false;
                bool downInjections = false;
                bool Sidecenter = false;
                bool center = false;
                bool doubleCenter = false;

                upInjections = RandomBool(0.6f);
                downInjections = RandomBool(0.6f);
                if (!upInjections && !downInjections)
                {
                    Sidecenter = RandomBool(0.5f);
                }

                if(Width >= 8)
                {
                    if(!Sidecenter)
                    {
                        center = RandomBool(0.9f);
                    }
                }

                if (Width >= 10)
                {
                    if (!center)
                    {
                        doubleCenter = RandomBool(1f);
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
                        }
                        heightExist++;
                    }
                }

                if (Sidecenter)
                {
                    int heightExist = RandomInt(Height / 2) + 1;
                    int startExist = Height - RandomInt(Height / 2) - 1;

                    for (int x = 0; x >= (Width + 1) / 2; x++)
                    {
                        if(heightExist > startExist)
                        {
                            break;
                        }
                        for (int y = startExist; y >= heightExist; y--)
                        {
                            exist[y, x] = 0;
                        }
                        startExist = RandomInt(startExist - 1);
                        heightExist += RandomInt(Height - heightExist);
                    }

                }

                if (center)
                {
                    int widthExist = (Width + 1) / 2 - 4;
                    widthExist = RandomInt(widthExist);
                    int tempWidth = widthExist;
                    int centerHeightExist = Height - (RandomInt(Height - Height / 3) + 2);
                    for (int y = centerHeightExist; y < Height; y++)
                    {
                        if (tempWidth == 0)
                        {
                            break;
                        }

                        for (int x = (Width + 1) / 2; x >= (Width + 1) / 2 - tempWidth; x--)
                        {
                            exist[y, x] = 0;
                        }
                        if (RandomBool(0.9f))
                        {
                            tempWidth--;
                        }
                    }
                    tempWidth = widthExist;
                    for (int y = centerHeightExist - 1; y >= 0; y--)
                    {
                        if (tempWidth == 0)
                        {
                            break;
                        }

                        for (int x = (Width + 1) / 2; x >= (Width + 1) / 2 - tempWidth; x--)
                        {
                            exist[y, x] = 0;
                        }
                        tempWidth = RandomInt(tempWidth);
                    }
                }

                SymmetryArrayForGorizontal(ref exist);
            }

            void BoxRandom()
            {
                SetArray(ref box, 0);

                if (RandomBool(0.7f) && !level.PassedWithBox)
                {
                    return;
                }

                bool downBox = false;
                bool downCenterBox = false;
                bool rightAndLeftBox = false;

                downBox = RandomBool(0.4f);
                if(Width >= 10)
                {
                    downCenterBox = RandomBool(0.5f);
                }
                if (!downCenterBox && !downBox && Width >= 8 || !downCenterBox && Width >= 10)
                {
                    rightAndLeftBox = RandomBool(0.8f);
                }

                if (!downBox && !downCenterBox && !rightAndLeftBox)
                {
                    downBox = true;
                }

                if (downBox)
                {
                    int heightBox = Height - RandomInt(Height / 2 - 2) - 1;
                    for(int x = 0; x < Width; x++)
                    {
                        for(int y = heightBox; y < Height; y++)
                        {
                            box[y, x] = 5;
                        }
                    }
                }
                if (downCenterBox)
                {
                    int widthBox = RandomInt(Width / 2 - 4) + 3;
                    int heightBox = Height - RandomInt(Height / 2 - 3) - 2;
                    for (int x = widthBox; x < Width - widthBox; x++)
                    {
                        for (int y = heightBox; y < Height; y++)
                        {
                            box[y, x] = 5;
                        }
                    }
                }
                if (rightAndLeftBox)
                {
                    int widthtBox = RandomInt(Height / 2 - 4) + 1;
                    Debug.Log(widthtBox);
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = widthtBox; x >= 0; x--)
                        {
                            box[y, x] = 5;
                            box[y, Width - 1 - x] = 5;
                        }
                    }
                }
            }

            void MoldRandom()
            {
                SetArray(ref mold, 0);

                if (!level.PassedWithMold)
                {
                    return;
                }

                RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width / 2; x++)
                    {
                        float randMold = Mathf.PerlinNoise((x + (NumLevel * 30000 * RandomFactor)) * Mathf.PI * noizeScale, (y + NumLevel) * Mathf.PI * noizeScale);
                        for (int k = 5; k > 0; k--)
                        {
                            if (randMold < moldChance / k)
                            {
                                mold[y, x] = k;
                                break;
                            }
                        }
                    }
                }

                SymmetryArrayForGorizontal(ref mold);
            }

            void PanelRandom()
            {
                SetArray(ref panel, 0);

                if (!level.PassedWithPanel)
                {
                    return;
                }
                for(int numPanel = 0; numPanel < 4; numPanel++)
                {
                    RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width / 2; x++)
                        {
                            float randPanel = Mathf.PerlinNoise((x + (NumLevel * 400000 * RandomFactor)) * Mathf.PI * noizeScale, (y + NumLevel) * Mathf.PI * noizeScale);
                            if (randPanel < panelChance)
                            {
                                panel[y, x] = 1;
                                if (box[y, x] == 0 && exist[y, x] == 1 && rock[y, x] == 0)
                                {
                                    numPanel++;
                                    if(numPanel >= 10)
                                    {
                                        SymmetryArrayForGorizontal(ref panel);
                                        return;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    for (int x = 0; x < Width / 2; x++)
                    {
                        if (box[0, x] == 0 && exist[0, x] == 1 && rock[0, x] == 0 && panel[0, x] == 0)
                        {
                            panel[0, x] = 1;
                            break;
                        }
                    }
                }

                SymmetryArrayForGorizontal(ref panel);
            }

            void SymmetryArrayForGorizontal(ref int[,] arr)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width / 2; x++)
                    {
                        arr[y, Width - 1 - x] = arr[y, x];
                    }
                }
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
                            box[y, x] = y;
                        }

                        if (level.PassedWithMold && randMold < moldChance)
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

                        panel[y, x] = 0;

                        IColors[y, x] = (int)(Mathf.PerlinNoise(x * NumLevel * Mathf.Deg2Rad, y * NumLevel * Mathf.Deg2Rad) * 1000000) % numColors;
                        Type[y, x] = 1;
                    }
                }
            }
        }
        int RandomInt(int maxValue)
        {
            if(maxValue <= 0)
            {
                return 0;
            }
            RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
            return Mathf.Abs((int)(NoizeResult * RandomFactor) % (maxValue + 1));
        }
        bool RandomBool(float chance)
        {
            RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
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
