using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������
//������
//�����
/// <summary>
/// ���������� ��������� ������
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelScriptableObject _levelsObject;

    public static LevelGenerator main;
    private int Score�oefficient = 500;
    private float existChance = 0.3f;
    private float boxChance = 0.4f;
    private float moldChance = 0.5f;
    private float panelChance = 0.2f;
    private float noizeScale = 0.04f;
    public LevelsScript.Level thisLevel;


    private void Start()
    {
        main = this;
        GenerateRangeLevel(900, 999);
    }
    /// <summary>
    /// ����������� ���� ���, ���������� ������ � ��������� �� � ����, ����� ������������ ������������ ������
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void GenerateRangeLevel(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            LevelsScript.Level lev = new LevelsScript.Level(GenerateLevelV3(i));
            lev.ConvertTwoCellsToOneCells();
            _levelsObject.levels[i] = new LevelsScript.Level(lev);
        }
    }

    private void MoveLevel(int startPos, int dif)
    {
        if(dif > 0)
        {
            for (int i = _levelsObject.levels.Count - 1; i >= startPos; i--)
            {
                try
                {
                    _levelsObject.levels[i + dif] = new LevelsScript.Level(_levelsObject.levels[i]);
                }
                catch { }
            }
        }
        else
        {

            for (int i = startPos; i < _levelsObject.levels.Count; i++)
            {
                try
                {
                    _levelsObject.levels[i + dif] = new LevelsScript.Level(_levelsObject.levels[i]);
                }
                catch { }
            }
        }
    }

    //��������� ������
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        float RandomFactor = 1.234567f * NumLevel;
        RandomFactor = Mathf.PerlinNoise(0, RandomFactor) + 1;



        //����� ����������
        float NoizeResult = Mathf.PerlinNoise(Mathf.Cos(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Sin(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Tan(NumLevel), 0f) * 1000000;
        //������������� ������ ������
        int Width = (int)NoizeResult % 8 + 6;
        int Height = (int)NoizeResult * 123 % (int)(Width / 2) + 6;
        //�������� ��������� ������
        int NeedScore = Width * Height * ((int)NoizeResult % Score�oefficient + Score�oefficient / 2);
        float move = (float)30 / (Width * Height * Score�oefficient) * NeedScore;
        //�������� ���������� ������
        int numColors;
        if (Width > 10)
        {
            numColors = 5;
        }
        else
        {
            numColors = 4;
        }

        //������� ������� ��� ��������
        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, (int)NoizeResult * 100, (int)NoizeResult * 100);

        PassRandom();

        ArraysRandom();

        LevelsScript.main.Levels[NumLevel] = level;

        return LevelsScript.main.Levels[NumLevel];

        //�������� ���� ������
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
        //������� �������� ������
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

            //��������� ����������
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

            //�������� ��������� ���������
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

                if (Width >= 8)
                {
                    if (!Sidecenter)
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
                        if (heightExist > startExist)
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

            //�������� ��������� �������
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
                if (Width >= 10)
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
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = heightBox; y < Height; y++)
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
            //�������� ��������� �������
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
            //�������� ��������� �������
            void PanelRandom()
            {
                SetArray(ref panel, 0);

                if (!level.PassedWithPanel)
                {
                    return;
                }
                for (int numPanel = 0; numPanel < 4; numPanel++)
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
                                    if (numPanel >= 10)
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
            //������������� ������ �� �����������
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
            //������� �������� � ������
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
            //��������� � ������� ���� �������
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
        //������ ��������� �������� �� 0 �� ����������
        int RandomInt(int maxValue)
        {
            if (maxValue <= 0)
            {
                return 0;
            }
            RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
            return Mathf.Abs((int)(NoizeResult * RandomFactor) % (maxValue + 1));
        }
        //��������� ���������� ��������
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

    public LevelsScript.Level GenerateNewLevelWithoutMassive(int NumLevel)
    {
        int perVar = (int)Mathf.Round(Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 0) * 10000 % 10);

        //������������� ������ ������
        int Width = perVar % 4 + 5;
        int Height = perVar * 123 % (Width / 2) + 6;

        //�������� ��������� ������
        int NeedScore = Width * Height * 50;// (perVar % Score�oefficient + Score�oefficient / 2);
        float move = Width * Height / 2;//(float)60 / (Width * Height * Score�oefficient) * NeedScore;

        //�������� ���������� ������
        int numColors = 4;

        //����� ������� ��� �������� ����� �����
        float perlinSuperColor = Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 1234) * 100;
        //���� ������ ������ ����� ����� ������ �� ������
        if (perlinSuperColor < 60)
        {
            perlinSuperColor = 0;
        }

        //����� ������� ��� �������� ������������
        float perlinBlocker = Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 9876) * 100;

        //������� ������� ��� ��������
        LevelsScript.Level level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, (int)perlinSuperColor, (int)perlinBlocker);

        return level;
    }

    public LevelsScript.Level GenerateLevelV2(int NumLevel)
    {
        int perVar = (int)Mathf.Round(Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 0) * 10000 % 10);

        int Width = perVar % 4 + 5;
        int Height = perVar * 123 % (Width / 2) + 6;

        //�������� ���������� ������
        int numColors = 4;

        LevelsScript.Level level = new LevelsScript.Level(GenerateNewLevelWithoutMassive(NumLevel));

        PassRandom();

        ArraysRandom();

        return level;

        //�������� ���� ������
        void PassRandom()
        {

            float rand = 6f / 10f * perVar;
            if ((int)rand == 0 && !level.PassedWithBox)
            {
                level.PassedWithRock = true;
            }
            else if ((int)rand == 1 && !level.PassedWithScore)
            {
                level.PassedWithCrystal = true;
                level.NeedCrystal = (int)perVar % 15 + 15;
                level.NeedColor = (CellInternalObject.InternalColor)(int)(perVar % 5);
            }
            else if ((int)rand == 2 && !level.PassedWithScore && !level.PassedWithRock)
            {
                level.PassedWithBox = true;
            }
            else if ((int)rand == 3 && !level.PassedWithScore && !level.PassedWithPanel)
            {
                level.PassedWithMold = true;
            }
            else if ((int)rand == 4 && !level.PassedWithScore && !level.PassedWithMold)
            {
                level.PassedWithPanel = true;
            }
            else if ((int)rand == 5 && !level.PassedWithScore)
            {
                level.PassedWithEnemy = true;
            }

        }
        //������� �������� ������
        void ArraysRandom()
        {
            int[,] exist = new int[Height, Width];
            int[,] ice = new int[Height, Width];
            int[,] box = new int[Height, Width];
            int[,] mold = new int[Height, Width];
            int[,] panel = new int[Height, Width];
            int[,] rock = new int[Height, Width];
            int[,] IColors = new int[Height, Width];
            int[,] Type = new int[Height, Width];
            int[,] dispencers = new int[Height, Width];
            int[,] walls = new int[Height, Width];
            int[,] teleports = new int[Height, Width];


            level.SetMass(CellRandom(exist, 100, 1, false), "exist");
            level.SetMass(ColorRandom(IColors, numColors - 1), "color");

            level.SetMass(TypeTest(Type), "type");
            level.SetMass(CellRandom(box, 20, 2, !level.PassedWithBox), "box");
            level.SetMass(CellRandom(mold, 20, 3, !level.PassedWithMold), "mold");
            level.SetMass(CellRandom(panel, 20, 4, !level.PassedWithPanel), "panel");
            level.SetMass(CellRandom(rock, 20, 5, !level.PassedWithRock), "rock");
            level.SetMass(CellRandom(ice, 20, 6, !level.PassedWithIce), "ice");
            level.SetMass(WallsRandom(walls, 15, 10), "walls");
            level.SetMass(DispencerRandom(dispencers, 10, 7, false), "dispencers");

            level.SetMass(TeleportsRandom(teleports, 10), "teleport");

            level.SetCells();


            //�������� ���������
            int[,] CellRandom(int[,] array, float percentOfField, int ID, bool zeroArray)
            {
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        if (!zeroArray)
                        {
                            perVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + ID + 0.777f, NumLevel + y + ID + 0.777f) * 10000 % 10 * 10);

                            if (perVar <= percentOfField)
                            {
                                array[x, y] = 1;
                            }
                            else
                            {
                                array[x, y] = 0;
                            }
                        }
                        else
                        {
                            array[x, y] = 0;
                        }
                    }
                }
                return array;
            }

            int[,] DispencerRandom(int[,] array, float percentOfField, int ID, bool zeroArray)
            {
                for (int x = 0; x < Height - 1; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        if (!zeroArray)
                        {
                            perVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + ID + 0.777f, NumLevel + y + ID + 0.777f) * 10000 % 10 * 10);

                            if (perVar <= percentOfField && array[x + 1, y] == 0 && exist[x + 1, y] == 1)
                            {
                                array[x, y] = 1;
                                rock[x, y] = 0;
                                walls[x, y] = 0;
                                walls[x + 1, y] = 0;
                                box[x, y] = 0;
                                ice[x, y] = 0;
                                mold[x, y] = 0;
                                if (level.PassedWithPanel)
                                {
                                    panel[x, y] = 1;
                                }
                            }
                            else
                            {
                                array[x, y] = 0;
                            }
                        }
                        else
                        {
                            array[x, y] = 0;
                        }
                    }
                }
                return array;
            }

            int[,] ColorRandom(int[,] array, float maxArrayValue)
            {
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        perVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + 0.777f, NumLevel + y + 0.777f) * 10000 % 10);

                        int randVal = (int)Mathf.Round(maxArrayValue / 10 * perVar);

                        array[x, y] = randVal;
                    }
                }
                return array;
            }

            int[,] WallsRandom(int[,] array, float maxArrayValue, int wallPercent)
            {
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        perVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + 0.777f, NumLevel + y + 0.777f) * 10000 % 10);
                        int percentPerVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + 0.666f, NumLevel + y + 0.666f) * 10000 % 10);

                        if (percentPerVar * 10 <= wallPercent)
                        {
                            int randVal = (int)Mathf.Round(maxArrayValue / 10 * perVar);
                            array[x, y] = randVal;
                        }
                        else
                        {
                            array[x, y] = 0;
                        }
                    }
                }
                return array;
            }

            int[,] TeleportsRandom(int[,] array, int teleportPercent)
            {
                int teleportID1 = 0;
                int teleportID2 = 0;
                int lastCreatedTeleportXPos = 0;
                int lastCreatedTeleportYPos = 0;
                for (int x = 0; x < Height - 1; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        perVar = (int)Mathf.Ceil(Mathf.PerlinNoise(NumLevel + x + 0.777f, NumLevel + y + 0.777f) * 10000 % 10 * 10);

                        if (perVar <= teleportPercent && exist[x + 1, y] != 0)
                        {
                            if (teleportID1 == 0 && exist[x, y] > 0)
                            {
                                teleportID1 = teleportID2 + 1;
                                array[x, y] = teleportID1;
                                lastCreatedTeleportXPos = x;
                                lastCreatedTeleportYPos = y;
                            }

                            else if (teleportID1 != 0 && exist[x, y] > 0)
                            {
                                teleportID2 = teleportID1;
                                teleportID1 = 0;
                                array[x, y] = teleportID2;
                            }
                        }
                    }
                }

                if (teleportID1 != 0)
                {
                    array[lastCreatedTeleportXPos, lastCreatedTeleportYPos] = 0;
                }
                return array;
            }

            int[,] TypeTest(int[,] array)
            {
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        array[x, y] = 1;
                    }
                }
                return array;
            }
        }
    }

    public LevelsScript.Level GenerateLevelV3(int NumLevel)
    {
        float symetryChance = 0.8f;
        bool symetry = false;

        float existscaler = 0.5f;
        float existchance = 0.4f;

        float boxScaler = 2f;
        float boxchance = 0.6f;

        float rockScaler = 30f;
        float rockChance = 0.7f;

        float panelScaler = 30f;
        float panelChance = 0.7f;

        float moldScaler = 0.4f;
        float moldChance = 0.45f;

        float iceScaler = 0.2f;
        float iceChance = 0.45f;

        int[] freeCellsBox = new int[10];
        int freeCellsUpNum = 0;

        int random = Random.Range(1000, 100000);

        //������������� ������ ������
        int Width = random % 4 + 6;
        int Height = random * 123 % 6 + 6;
        //�������� ��������� ������
        int NeedScore = Width * Height * (random % Score�oefficient + Score�oefficient / 2);
        float move = (float)30 / (Width * Height * Score�oefficient) * NeedScore;
        //�������� ���������� ������
        int numColors = 4;

        //������� ������� ��� ��������
        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, 0, 0);

        int[,] exist = new int[Height, Width];
        int[,] ice = new int[Height, Width];
        int[,] box = new int[Height, Width];
        int[,] mold = new int[Height, Width];
        int[,] panel = new int[Height, Width];
        int[,] rock = new int[Height, Width];
        int[,] IColors = new int[Height, Width];
        int[,] Type = new int[Height, Width];
        int[,] dispencers = new int[Height, Width];
        int[,] walls = new int[Height, Width];
        int[,] teleports = new int[Height, Width];

        PassRandom();
        Crystal();
        symetry = Chance(symetryChance);
        if (level.PassedWithBox == false && level.PassedWithRock == false)
        {
            for (int i = 0; i <= 100; i++)
            {
                ExistRandom();
                Dispencers();
                BoxRandom();
                RockRandom();
                CheckFreeCells();
                CheckFreeUpCells();
                if (freeCellsBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
        }

        for (int i = 0; i <= 1000; i++)
        {
            if (level.PassedWithPanel == true)
            {
                PanelRandom();
                if (CheckArr(ref panel) >= CheckArr(ref exist) / 10)
                    break;
            }
            else if (level.PassedWithIce == true)
            {
                IceRandom();
                if (CheckArr(ref ice) >= CheckArr(ref exist) / 2)
                    break;
            }
            else if (level.PassedWithMold == true)
            {
                MoldRandom();
                if (CheckArr(ref mold) >= CheckArr(ref exist) / 3)
                    break;
            }
            else if (level.PassedWithRock == true)
            {
                ArrayNull(ref box, 0);
                ExistRandom();
                Dispencers();
                RockRandomPass(1.2f);
                CheckFreeCells();
                CheckFreeUpCells();
                if (CheckArr(ref rock) >= CheckArr(ref exist) / 6 && freeCellsBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
            else if (level.PassedWithBox == true)
            {
                ArrayNull(ref rock, 0);
                ExistRandom();
                Dispencers();
                BoxRandomPass(1.1f);
                CheckFreeCells();
                CheckFreeUpCells();
                if (CheckArr(ref box) >= CheckArr(ref exist) / 6 && freeCellsBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
            else if (level.PassedWithCrystal == true)
            {
                level.NeedCrystal = random % CheckArr(ref exist) + 15;
                level.NeedColor = (CellInternalObject.InternalColor)(random % 5);
            }
            if (i == 1000)
                Debug.Log("pizda");
        }

        level.SetMass(exist, "exist");
        level.SetMass(IColors, "color");
        level.SetMass(Type, "type");
        level.SetMass(box, "box");
        level.SetMass(mold, "mold");
        level.SetMass(panel, "panel");
        level.SetMass(rock, "rock");
        level.SetMass(ice, "ice");
        level.SetMass(walls, "walls");
        level.SetMass(dispencers, "dispencers");
        level.SetMass(teleports, "teleport");

        level.SetCells();

        return level;

        //�������� ���� ������
        void PassRandom()
        {

            int rand = (int)(random % 7);
            switch (rand)
            {
                case 0:
                    level.PassedWithPanel = true;
                    break;
                case 1:
                    level.PassedWithMold = true;
                    break;
                case 2:
                    level.PassedWithEnemy = true;
                    break;
                case 3:
                    level.PassedWithRock = true;
                    break;
                case 4:
                    level.PassedWithBox = true;
                    break;
                case 5:
                    level.PassedWithIce = true;
                    break;
                default:
                    level.PassedWithCrystal = true;
                    break;
            }
        }

        //��������� �����
        void ExistRandom()
        {
            ArrayNull(ref exist, 0);
            ArrayRandomPerlin(existscaler, existchance, ref exist, 1);
        }

        //�������� ���������� ����� ������ ������� ��� ���������
        void CheckFreeCells()
        {
            for (int i = 0; i <= 9; i++)
            {
                freeCellsBox[i] = 0;
            }
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    int freeCellsNum = 0;
                    //����� �������� ����� ������(������ ���)
                    for (int y2 = -1; y2 <= 1; y2++)
                    {
                        for (int x2 = -1; x2 <= 1; x2++)
                        {
                            if (exist[y + y2, x + x2] == 1 && box[y + y2, x + x2] == 0 && rock[y + y2, x + x2] == 0)
                                freeCellsNum++;
                        }
                    }
                    for (int i = 0; i <= 9; i++)
                    {
                        if (freeCellsNum >= i)
                            freeCellsBox[i]++;
                    }
                }
            }
        }

        //�������� ���������� ��������� ����� ��� ������ ������(������� ����� ����� ����������)
        void CheckFreeUpCells()
        {
            freeCellsUpNum = 0;
            bool[,] free = new bool[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    free[y, x] = true;
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (exist[y, x] == 1 && box[y, x] == 0 && rock[y, x] == 0)
                    {
                        if (y == 0 || dispencers[y - 1, x] == 1 || free[y - 1, x] == true || (x != 0 && free[y - 1, x - 1] == true) || (x != Width - 1 && free[y - 1, x + 1] == true))
                        {
                            free[y, x] = true;
                            Type[y, x] = 1;
                            freeCellsUpNum++;
                        }
                    }
                    else
                    {
                        free[y, x] = false;

                        if(rock[y, x] == 0)
                            Type[y, x] = 0;
                        else
                            Type[y, x] = 1;
                    }
                }
            }
        }

        int CheckArr(ref int[,] arr)
        {
            int numArr = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (arr[y, x] == 1 && exist[y, x] == 1)
                        numArr++;
                }
            }
            return numArr;
        }

        //��������� ����������� � ����������� ������
        void Dispencers()
        {
            ArrayNull(ref dispencers, 0);
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (exist[y, x] == 0 && exist[y + 1, x] == 1)
                    {
                        if (x == 0 && exist[y, x + 1] == 0 || x == Width - 1 && exist[y, x - 1] == 0)
                        {
                            dispencers[y, x] = 1;
                        }
                        else if(x != 0 && x != Width - 1 && exist[y, x - 1] == 0 && exist[y, x + 1] == 0)
                        {
                            dispencers[y, x] = 1;
                        }
                    }
                }
            }
        }

        //��������� ����������
        void Crystal()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    IColors[y, x] = 0;
                    Type[y, x] = 1;
                }
            }
        }
        //��������� ������
        void BoxRandomPass(float value)
        {
            ArrayNull(ref box, 0);
            ArrayRandomPerlin(boxScaler, boxchance / value, ref box, 5);
        }

        //��������� ������
        void BoxRandom()
        {
            ArrayNull(ref box, 0);
            ArrayRandomPerlin(boxScaler, boxchance, ref box, 5);
        }

        //��������� ������
        void RockRandomPass(float value)
        {
            ArrayNull(ref rock, 0);
            ArrayRandomPerlin(rockScaler, rockChance / value, ref rock, 1);
        }

        //��������� ������
        void RockRandom()
        {
            ArrayNull(ref rock, 0);
            ArrayRandomPerlin(rockScaler, rockChance, ref rock, 1);
        }

        //��������� ����
        void IceRandom()
        {
            ArrayNull(ref ice, 0);
            ArrayRandomPerlin(iceScaler, iceChance, ref ice, 5);
        }

        //��������� ������
        void PanelRandom()
        {
            ArrayNull(ref panel, 0);
            ArrayRandomPerlin(panelScaler, panelChance, ref panel, 1);
        }

        //��������� �������
        void MoldRandom()
        {
            ArrayNull(ref mold, 0);
            ArrayRandomPerlin(moldScaler, moldChance, ref mold, 1);
        }

        void ArrayRandomPerlin(float scaler, float chance, ref int[,] arr, int maxValue)
        {
            float randomPositionX = Random.Range(0f, 10000f);
            float randomPositionY = Random.Range(0f, 10000f);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float noize = Mathf.PerlinNoise(x * scaler + randomPositionX * NumLevel, y * scaler + randomPositionY);
                    for (int i = 0; i < maxValue; i++)
                    {
                        if(noize > chance + i * 0.05f)
                        {
                            arr[y, x] = i + 1;
                        }
                    }
                }
            }
            if (symetry)
                SymmetryArrayForGorizontal(ref arr);
        }

        void ArrayNull(ref int[,] arr, int defaultValue)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    arr[y, x] = defaultValue;
                }
            }
        }

        //������������� ������ �� �����������
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
        //��������� ������ �����
        bool Chance(float i)
        {
            float random = Random.Range(0f, 1f);
            if (random < i)
                return true;
            else
                return false;
        }
    }
}
