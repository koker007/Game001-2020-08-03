using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//александр
//Андрей
//Семен
/// <summary>
/// генерирует рандомные уровни
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelScriptableObject _levelsObject;

    public static LevelGenerator main;
    private int ScoreСoefficient = 500;
    private float existChance = 0.3f;
    private float boxChance = 0.4f;
    private float moldChance = 0.5f;
    private float panelChance = 0.2f;
    private float noizeScale = 0.04f;
    public LevelsScript.Level thisLevel;


    private void Start()
    {
        main = this;
        //GenerateRangeLevel(900, 999);
    }
    /// <summary>
    /// запускается один раз, генерирует уровни и сохраняет их в файл, может перезаписать существующие уровни
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void GenerateRangeLevel(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            LevelsScript.Level lev = new LevelsScript.Level(GenerateLevelV3(i, false));
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

    //генерация уровня
    public LevelsScript.Level GenerateLevel(int NumLevel)
    {
        float RandomFactor = 1.234567f * NumLevel;
        RandomFactor = Mathf.PerlinNoise(0, RandomFactor) + 1;



        //Иначе генерируем
        float NoizeResult = Mathf.PerlinNoise(Mathf.Cos(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Sin(NumLevel), 0f) * Mathf.PerlinNoise(Mathf.Tan(NumLevel), 0f) * 1000000;
        //устанавливаем размер уровня
        int Width = (int)NoizeResult % 8 + 6;
        int Height = (int)NoizeResult * 123 % (int)(Width / 2) + 6;
        //основные параметры уровня
        int NeedScore = Width * Height * ((int)NoizeResult % ScoreСoefficient + ScoreСoefficient / 2);
        float move = (float)30 / (Width * Height * ScoreСoefficient) * NeedScore;
        //выюираем количество цветов
        int numColors;
        if (Width > 10)
        {
            numColors = 5;
        }
        else
        {
            numColors = 4;
        }

        //создаем уровень без массивов
        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, (int)NoizeResult * 100, (int)NoizeResult * 100);

        PassRandom();

        ArraysRandom();

        LevelsScript.main.Levels[NumLevel] = level;

        return LevelsScript.main.Levels[NumLevel];

        //выбираем цели уровня
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
        //заносим значения клеток
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

            //генерация кристаллов
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

            //алгоритм генерации отверстий
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

            //алгоритм генерации коробок
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
            //алгоритм генерации плесени
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
            //алгоритм генерации панелей
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
            //отзеркаливает массив по горизонтали
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
            //заносит значения в массив
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
            //генерация с помощью шума перлина
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
        //выдает случайное значение от 0 до указонного
        int RandomInt(int maxValue)
        {
            if (maxValue <= 0)
            {
                return 0;
            }
            RandomFactor = Mathf.PerlinNoise(0, RandomFactor * 100) + 1;
            return Mathf.Abs((int)(NoizeResult * RandomFactor) % (maxValue + 1));
        }
        //случайное логическое значение
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

        //устанавливаем размер уровня
        int Width = perVar % 4 + 5;
        int Height = perVar * 123 % (Width / 2) + 6;

        //основные параметры уровня
        int NeedScore = Width * Height * 50;// (perVar % ScoreСoefficient + ScoreСoefficient / 2);
        float move = Width * Height / 2;//(float)60 / (Width * Height * ScoreСoefficient) * NeedScore;

        //выюираем количество цветов
        int numColors = 4;

        //Какой процент для создания супер цвета
        float perlinSuperColor = Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 1234) * 100;
        //Если меньше порога пусть лучше вообще не мешают
        if (perlinSuperColor < 60)
        {
            perlinSuperColor = 0;
        }

        //какой процент для создания блокираторов
        float perlinBlocker = Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 9876) * 100;

        //создаем уровень без массивов
        LevelsScript.Level level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, NeedScore, (int)move, numColors, (int)perlinSuperColor, (int)perlinBlocker);

        return level;
    }

    public LevelsScript.Level GenerateLevelV2(int NumLevel)
    {
        int perVar = (int)Mathf.Round(Mathf.PerlinNoise(777.777f / 666.666f + NumLevel, 0) * 10000 % 10);

        int Width = perVar % 4 + 5;
        int Height = perVar * 123 % (Width / 2) + 6;

        //выюираем количество цветов
        int numColors = 4;

        LevelsScript.Level level = new LevelsScript.Level(GenerateNewLevelWithoutMassive(NumLevel));

        PassRandom();

        ArraysRandom();

        return level;

        //выбираем цели уровня
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
        //заносим значения клеток
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


            //алгоритм генерации
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
    public LevelsScript.Level GenerateLevelV3(int NumLevel, bool isKey)
    {
        return GenerateLevelV3(NumLevel, isKey);
    }

    public LevelsScript.Level GenerateLevelV3(int NumLevel, LevelsScript.Level baseLevel, bool isKey)
    {
        int key = (int)((float)NumLevel * 12345.6789);
        float symetryChance = 0.8f;
        bool symetry = false;
        int[] freeCellsUpBox = new int[10];
        int[] freeCellsBox = new int[10];
        int freeCellsUpNum = 0;

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

        float iceScaler = 0.4f;
        float iceChance = 0.45f;

        //устанавливаем размер уровня
        int Width = isKey ? Key() % 4 + 6 : Random.Range(0, 1000) % 4 + 6;
        int Height = isKey ? Key() % 6 + 6 : Random.Range(0, 1000) % 6 + 6;

        //создаем уровень без массивов
        LevelsScript.Level level = LevelsScript.main.Levels[NumLevel];
        level = LevelsScript.main.CreateLevel(NumLevel, Height, Width, 0, 0, 0, 0, 0);

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
                if (freeCellsUpBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
        }

        for (int i = 0; i <= 1000; i++)
        {
            if (level.PassedWithPanel == true)
            {
                if (i == 1000)
                    Debug.Log("panel");
                PanelRandom();
                if (CheckArr(ref panel) >= CheckArr(ref exist) / 10)
                    break;
            }
            else if (level.PassedWithIce == true)
            {
                if (i == 1000)
                    Debug.Log("ice");
                IceRandom();
                if (CheckArr(ref ice) >= CheckArr(ref exist) / 2)
                    break;
            }
            else if (level.PassedWithMold == true)
            {
                if (i == 1000)
                    Debug.Log("mold");
                MoldRandom();
                if (CheckArr(ref mold) >= CheckArr(ref exist) / 3)
                    break;
            }
            else if (level.PassedWithRock == true)
            {
                if (i == 1000)
                    Debug.Log("rock");
                ArrayNull(ref box, 0);
                ExistRandom();
                Dispencers();
                RockRandomPass(1.2f);
                CheckFreeCells();
                CheckFreeUpCells();
                if (CheckArr(ref rock) >= CheckArr(ref exist) / 6 && freeCellsUpBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
            else if (level.PassedWithBox == true)
            {
                if (i == 1000)
                    Debug.Log("box");
                ArrayNull(ref rock, 0);
                ExistRandom();
                Dispencers();
                BoxRandomPass(1f);
                CheckFreeCells();
                CheckFreeUpCells();
                if (CheckArr(ref box) >= CheckArr(ref exist) / 6 && freeCellsUpBox[6] >= 8 && freeCellsBox[9] >= 1 && freeCellsUpNum >= 14)
                    break;
            }
            else if (level.PassedWithCrystal == true)
            {
                level.NeedCrystal = isKey ? Key() % CheckArr(ref exist) + 15 : Random.Range(0, 1000) % CheckArr(ref exist) + 15;
                level.NeedColor = isKey ? (CellInternalObject.InternalColor)(Key() % 5) : (CellInternalObject.InternalColor)(Random.Range(0, 1000) % 5);
                break;
            }
            else if (level.PassedWithEnemy == true || level.PassedWithScore)
            {
                break;
            }
            if (i == 1000)
                Debug.Log("oh no");
        }

        MainParametersCalc();

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

        level.ConvertTwoCellsToOneCells();

        /*Debug.Log($"{NumLevel} freeCellsUpNum = {freeCellsUpNum}");
        for(int i = 0; i < freeCellsBox.Length; i++)
        {
            Debug.Log($"{NumLevel} freeCellsBox[{i}] = {freeCellsBox[i]}");
        }*/

        return level;

        //выбираем цели уровня
        void PassRandom()
        {
            if(baseLevel != null && (baseLevel.PassedWithBox || baseLevel.PassedWithCrystal || baseLevel.PassedWithEnemy || baseLevel.PassedWithIce || baseLevel.PassedWithMold || baseLevel.PassedWithPanel || baseLevel.PassedWithRock || baseLevel.PassedWithScore))
            {
                level.PassedWithBox = baseLevel.PassedWithBox;
                level.PassedWithCrystal = baseLevel.PassedWithCrystal;
                level.PassedWithEnemy = baseLevel.PassedWithEnemy;
                level.PassedWithIce = baseLevel.PassedWithIce;
                level.PassedWithMold = baseLevel.PassedWithMold;
                level.PassedWithPanel = baseLevel.PassedWithPanel;
                level.PassedWithRock = baseLevel.PassedWithRock;
                level.PassedWithScore = baseLevel.PassedWithScore;
                return;
            }

            int rand = isKey ? Key() % 7 : Random.Range(0, 1000) % 7;
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

        //генерация ячеек
        void ExistRandom()
        {
            ArrayNull(ref exist, 0);
            ArrayRandomPerlin(existscaler, existchance, ref exist, 1);
        }

        //проверка количества ячеек вокруг которых нет отверстий
        void CheckFreeCells()
        {
            for (int i = 0; i <= 9; i++)
            {
                freeCellsBox[i] = 0;
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int freeCellsNum = CheckOneCell(x, y, false);
                    for (int i = 0; i <= 9; i++)
                    {
                        if (freeCellsNum >= i)
                            freeCellsBox[i]++;
                    }
                }
            }
        }

        //проверка количества свободных ячеек при старте уровня(сколько ячеек может заполнится)
        void CheckFreeUpCells()
        {
            for (int i = 0; i <= 9; i++)
            {
                freeCellsUpBox[i] = 0;
            }
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
                    if ((exist[y, x] == 1 && box[y, x] == 0 && rock[y, x] == 0) &&
                        (y == 0 || dispencers[y - 1, x] == 1 || free[y - 1, x] == true || (x != 0 && free[y - 1, x - 1] == true) || (x != Width - 1 && free[y - 1, x + 1] == true)))
                    {
                        free[y, x] = true;
                        Type[y, x] = 1;
                        freeCellsUpNum++;

                        int freeCellsNum = CheckOneCell(x, y, true);
                        for (int i = 0; i <= 9; i++)
                        {
                            if (freeCellsNum >= i)
                                freeCellsUpBox[i]++;
                        }
                    }
                    else
                    {
                        free[y, x] = false;

                        if (rock[y, x] == 0 && dispencers[y, x] == 0)
                            Type[y, x] = 0;
                        else
                            Type[y, x] = 1;
                    }
                }
            }
        }

        //циклы проверки одной ячейки(вокруг нее)
        int CheckOneCell(int x, int y, bool isFullFree)
        {
            int freeCellsNum = 0;
            for (int y2 = -1; y2 <= 1; y2++)
            {
                for (int x2 = -1; x2 <= 1; x2++)
                {
                    //проверка ячейки
                    if (isFullFree)
                    {
                        if ((y + y2 >= 0 && y + y2 < Height && x + x2 >= 0 && x + x2 < Width) && (exist[y + y2, x + x2] == 1) && (box[y + y2, x + x2] == 0) && (rock[y + y2, x + x2] == 0))
                            freeCellsNum++;
                    }
                    else
                    {
                        if ((y + y2 >= 0 && y + y2 < Height && x + x2 >= 0 && x + x2 < Width) && (exist[y + y2, x + x2] == 1))
                            freeCellsNum++;
                    }
                }
            }
            return freeCellsNum;
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

        //генерация раздатчиков в необходимых местах
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

        //генерация кристаллов
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
        //генерация ящиков
        void BoxRandomPass(float value)
        {
            ArrayNull(ref box, 0);
            ArrayRandomPerlin(boxScaler, boxchance / value, ref box, 5);
        }

        //генерация ящиков
        void BoxRandom()
        {
            ArrayNull(ref box, 0);
            ArrayRandomPerlin(boxScaler, boxchance, ref box, 5);
        }

        //генерация камней
        void RockRandomPass(float value)
        {
            ArrayNull(ref rock, 0);
            ArrayRandomPerlin(rockScaler, rockChance / value, ref rock, 1);
        }

        //генерация камней
        void RockRandom()
        {
            ArrayNull(ref rock, 0);
            ArrayRandomPerlin(rockScaler, rockChance, ref rock, 1);
        }

        //генерация льда
        void IceRandom()
        {
            ArrayNull(ref ice, 0);
            ArrayRandomPerlin(iceScaler, iceChance, ref ice, 5);
        }

        //генерация панели
        void PanelRandom()
        {
            ArrayNull(ref panel, 0);
            ArrayRandomPerlin(panelScaler, panelChance, ref panel, 1);
        }

        //генерация плесени
        void MoldRandom()
        {
            ArrayNull(ref mold, 0);
            ArrayRandomPerlin(moldScaler, moldChance, ref mold, 1);
        }

        void ArrayRandomPerlin(float scaler, float chance, ref int[,] arr, int maxValue)
        {
            float randomPositionX = isKey ? Key() * 1.1f : Random.Range(0, 1000f);
            float randomPositionY = isKey ? Key() * 1.1f : Random.Range(0, 1000f);
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

        //отзеркаливает массив по горизонтали
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

        void MainParametersCalc()
        {
            /*
            float coeficient;
            coeficient = freeCellsUpNum * 0.5f + freeCellsBox[7] * 1.5f + ((freeCellsBox[3] - freeCellsBox[4]) * -0.5f);
            int intCoef = (int)(coeficient / 5);
            //if(freeCellsUpNum < 16)
            //    intCoef--;
            intCoef = intCoef < 3 ? 3 : intCoef;
            intCoef = intCoef > 5 ? 5 : intCoef;
            Debug.Log($"{NumLevel} int {intCoef} float {coeficient} {freeCellsUpNum * 0.5f} {freeCellsBox[9] * 2} {(freeCellsBox[3] - freeCellsBox[4]) * -0.5f}  {freeCellsUpBox[6]}");
            level.NumColors = intCoef;
            */
            if ((freeCellsBox[9] > 12 || freeCellsBox[6] > 32) && (freeCellsUpBox[6] > 16 || freeCellsUpBox[4] > 32))
            {
                if (Chance(0.4f))
                {
                    level.NumColors = 5;
                }
                else if (Chance(0.4f))
                {
                    level.NumColors = 4;
                    level.TypeBlockerPercent = 100;
                }
                else
                {
                    level.NumColors = 4;
                    level.SuperColorPercent = 100;
                }
            }
            else if (((freeCellsBox[2] > 4 || freeCellsBox[3] > 6 || freeCellsBox[4] > 10) && freeCellsBox[6] < 14) || (freeCellsUpBox[6] < 10 && freeCellsUpBox[4] < 20))
            {
                level.NumColors = 3;
            }
            else
            {
                if (Chance(0.4f))
                {
                    level.NumColors = 4;
                }
                else if (Chance(0.4f))
                {
                    level.NumColors = 3;
                    level.TypeBlockerPercent = 100;
                }
                else
                {
                    level.NumColors = 3;
                    level.SuperColorPercent = 100;
                }
            }

            if (level.PassedWithEnemy == true)
            {
                level.Move = 10;
            }
            else if (level.PassedWithCrystal == true)
            {
                level.Move = level.NeedCrystal / 3 + (freeCellsBox[6] - freeCellsBox[7]) / 3 + (freeCellsBox[4] - freeCellsBox[1]);
            }

            level.Move = freeCellsBox[6] - freeCellsBox[7] + (freeCellsBox[1] - freeCellsBox[4]);

            level.NeedScore = 300 * level.Move * ((freeCellsBox[6] / 2 + 1) + (freeCellsBox[9] + 1));
        }

        //рандомный расчет шанса
        bool Chance(float i)
        {
            float random = isKey ? Key() / 1000 % 1 : Random.Range(0f, 1f); ;
            if (random < i)
                return true;
            else
                return false;
        }

        int Key()
        {
            Debug.Log($"key{key}");
            key = (int)((key * 1234.5678) % 1000);
            return key;
        }
    }
}
