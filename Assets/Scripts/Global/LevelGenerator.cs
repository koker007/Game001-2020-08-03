using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������
/// <summary>
/// ���������� ��������� ������
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator main;
    private int Score�oefficient = 500;

    //����� ��������� �������� �� ���� �������
    private float existChance = 0.3f;
    private float boxChance = 0.4f;
    private float moldChance = 0.5f;
    private float noizeScale = 0.04f;

    private void Start()
    {
        main = this;
    }
    /// <summary>
    /// ��������� ������
    /// </summary>
    /// <param name="NumLevel"></param>
    /// <returns></returns>
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        //��� ��������� �������
        float RandomFactor = 1.234f * NumLevel;
        //�������� �� ������� ������
        if (LevelsScript.main.ReturnLevel(NumLevel) != null)
        {
            return LevelsScript.main.ReturnLevel(NumLevel);
        }

        //��������� ����
        float NoizeResult = Mathf.PerlinNoise(Mathf.Cos(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Sin(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Tan(NumLevel), 0f) * 1000000;
        //��������� �������� ���������� ������
        int Width = RandomInt(8) + 6;
        int Height = RandomInt((int)(Width / 2)) + 6;

        int NeedScore = Width * Height * (RandomInt(Score�oefficient) + Score�oefficient / 2);
        float move = (float)30 / (Width * Height * Score�oefficient) * NeedScore;

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

        //�������� ���� ��� ������ �� 3� ����
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

        //��������� �������� �������� �� ������� ����
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

            //������ ��������� ���������
            void ExistRandom()
            {
                SetArray(ref exist, 1);

                //������� ����
                bool upInjections = false;
                //������ ����
                bool downInjections = false;
                //����� ����
                bool Sidecenter = false;
                //�����
                bool center = false;

                upInjections = RandomBool(0.6f);
                downInjections = RandomBool(0.6f);
                if (!upInjections && !downInjections)
                {
                    Sidecenter = RandomBool(0.8f);
                }

                if(Width >= 8)
                {
                    if(!Sidecenter)
                    {
                        center = RandomBool(0.8f);
                    }
                }

                //��������� �������� ����
                if (upInjections)
                {
                    int heightExist = Height / 2 - 1;
                    for (int x = 0; x < Width / 2 - 1; x++)
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

                //��������� ������� ����
                if (downInjections)
                {
                    int heightExist = Height / 2;
                    for (int x = 0; x < Width / 2 - 1; x++)
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
                
                //��������� ������ ����
                if (Sidecenter)
                {
                    int heightExist = RandomInt(Height / 2) + Height / 2 - 1;
                    int startheight = RandomInt(Height - heightExist) - 1;

                    for (int x = 0; x < (Width + 1) / 2; x++)
                    {
                        if (startheight >= Height || startheight < 0)
                        {
                            break;
                        }
                        for (int y = startheight; y < startheight + heightExist; y++)
                        {
                            exist[y, x] = 0;
                        }
                        int he = heightExist;
                        heightExist -= 2;
                        heightExist = RandomInt(heightExist);
                        startheight = startheight + RandomInt(he - heightExist);
                    }
                }

                //��������� �����
                if (center)
                {
                    int widthExist = (Width + 1) / 2 - 4;
                    int tempWidth = widthExist;
                    int centerHeightExist = Height - RandomInt(Height - Height / 3 - 1);
                    //������ �����
                    for (int y = centerHeightExist; y < Height; y++)
                    {
                        if (y >= Height || Height < 0)
                        {
                            break;
                        }
                        for (int x = 4 + (widthExist - tempWidth); x <= (Width + 1) / 2 - 1; x++)
                        {
                            if(x >= Width || x < 0)
                            {
                                break;
                            }
                            exist[y, x] = 0;
                        }
                        if (RandomBool(0.5f))
                        {
                            tempWidth--;
                        }
                    }
                    tempWidth = widthExist;
                    //������� �����
                    for (int y = centerHeightExist; y >= 0; y--)
                    {
                        if (y >= Height || Height < 0)
                        {
                            break;
                        }
                        for (int x = 4 + (widthExist - tempWidth); x <= (Width + 1) / 2 - 1; x++)
                        {
                            if (x >= Width || x < 0)
                            {
                                break;
                            }
                            exist[y, x] = 0;
                        }
                        tempWidth = RandomInt(tempWidth);
                    }
                }

                MirrorXArray(ref exist);
            }

            //�������� ������ �� �
            void MirrorXArray(ref int[,] arr)
            {

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < (Width + 1) / 2; x++)
                    {
                        arr[y, Width - 1 - x] = arr[y, x];
                    }
                }
            }

            //�������� ������ ������ value
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

            //��������� �������� � ������� ���� �������
            void PerlinAllRandom()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        float randExist = Mathf.PerlinNoise((x + NumLevel * 100) * Mathf.PI * noizeScale * 3, (y + NumLevel) * Mathf.PI * noizeScale);
                        float randBox = Mathf.PerlinNoise((x + (NumLevel * 2000)) * Mathf.PI * noizeScale * 2, (y + NumLevel) * Mathf.PI * noizeScale);
                        float randMold = Mathf.PerlinNoise((x + (NumLevel * 30000)) * Mathf.PI * noizeScale, (y + NumLevel) * Mathf.PI * noizeScale);
                        //��������� ���������
                        if (randBox > 1 - existChance)
                        {
                            exist[y, x] = 0;
                        }
                        else
                        {
                            exist[y, x] = 1;
                        }

                        //��������� �������
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

                        //��������� �������
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

                        //��������� ����� � ����
                        IColors[y, x] = (int)(Mathf.PerlinNoise(x * NumLevel * Mathf.Deg2Rad, y * NumLevel * Mathf.Deg2Rad) * 1000000) % 4;
                        Type[y, x] = 0;
                    }
                }
            }
        }

        //��������� ����� � ��������� �� 0 �� maxValue
        int RandomInt(int maxValue)
        {
            if(maxValue > 0)
            {
                RandomFactor = Mathf.PerlinNoise(RandomFactor, 0);
                return Mathf.Abs((int)(NoizeResult * RandomFactor) % (maxValue + 1));
            }
            else
            {
                return 0;
            }
        }

        //��������� ����������� chance - �� 0.0 �� 1.0
        bool RandomBool(float chance)
        {
            RandomFactor = Mathf.PerlinNoise(RandomFactor, 0);
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
