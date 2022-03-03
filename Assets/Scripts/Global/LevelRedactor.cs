using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// редактор уровней
/// редщактируемый уровень хранится в нулевом уровне
/// </summary>
public class LevelRedactor : MonoBehaviour
{
    public static LevelRedactor main;
    [SerializeField] private LevelScriptableObject _levelsObject;
    [Space]
    [SerializeField] private CellRedactor _cellPref;
    [SerializeField] private RectTransform _gameFieldTransform;
    [SerializeField] private const int _heightOneCell = 100;
    [Space]
    //UI элементы для глобальной настройки уровня
    [SerializeField] private InputField _levelNumIF;
    [SerializeField] private InputField _sizeXIF;
    [SerializeField] private InputField _sizeYIF;
    [SerializeField] private InputField _moveIF;
    [SerializeField] private InputField _needScoreIF;
    [SerializeField] private InputField _numColorsIF;
    [SerializeField] private InputField _difficultIF;
    [Space]
    [SerializeField] private Slider _blockerSlider;
    [SerializeField] private Slider _superColorSlider;
    [Space]
    [SerializeField] private Toggle _passedWithScore;
    [SerializeField] private Toggle _passedWithCrystal;
    [SerializeField] private Toggle _passedWithBox;
    [SerializeField] private Toggle _passedWithMold;
    [SerializeField] private Toggle _passedWithIce;
    [SerializeField] private Toggle _passedWithPanel;
    [SerializeField] private Toggle _passedWithRock;
    [SerializeField] private Toggle _passedWithEnemy;
    [SerializeField] private Dropdown _crystalColorDropdown;
    [SerializeField] private InputField _crystalNumIF;

    //клетка
    [SerializeField] private CellRedactor[,] _cells; //хранит редакторы! клеток
    [SerializeField] private LevelsScript.CellInfo _cellBufer; //скопированная клетка (буфер)
    [SerializeField] private CellRedactor _cellsBuferImage; //отображение клетки в буфере
    [SerializeField] private Toggle _FastPast;
    private int _selectCellPos = 0;
    [Space]
    //UI элементы для настройки выбранной клетки
    [SerializeField] private Text _SelectedCellPosText;
    [SerializeField] private Toggle _ExistSelectedCell;
    [SerializeField] private Toggle _DispencerSelectedCell;
    [SerializeField] private Toggle _RockSelectedCell;
    [SerializeField] private Toggle _PanelSelectedCell;
    [SerializeField] private Toggle _MoldSelectedCell;
    [SerializeField] private Slider _BoxHealthSelectedCellSlider;
    [SerializeField] private Text _BoxHealthSelectedCellText;
    [SerializeField] private Slider _PortalSelectedCellSlider;
    [SerializeField] private Text _PortalSelectedCellText;
    [SerializeField] private Slider _WallsTypeSelectedCellSlider;
    [SerializeField] private Text _WallsTypeSelectedCellText;
    private string[] _WallsName = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
    [SerializeField] private Slider _IceHealthSelectedCellSlider;
    [SerializeField] private Text _IceHealthSelectedCellText;
    [SerializeField] private Slider _ColorSelectedCellSlider;
    [SerializeField] private Text _ColorSelectedCellText;
    private string[] _ColorsName = { "Red", "Green", "Blue", "Yellow", "Violet", "Ultimate" };
    [SerializeField] private Slider _TypeSelectedCellSlider;
    [SerializeField] private Text _TypeSelectedCellText;
    private string[] _TypesName = { "none", "color", "color5", "rocketHorizontal", "rocketVertical", "bomb", "airplane", "blocker" };
    [Space]
    [Space]
    [Space]
    //текстуры клеток
    [SerializeField] private Sprite TextureExist;

    [SerializeField] private Sprite[] TextureIce = new Sprite[5];
    [SerializeField] private Sprite TextureMold;
    [SerializeField] private Sprite TexturePanel;

    [SerializeField] private Sprite TextureRed;
    [SerializeField] private Sprite TextureGreen;
    [SerializeField] private Sprite TextureBlue;
    [SerializeField] private Sprite TextureYellow;
    [SerializeField] private Sprite TextureOrange;
    [SerializeField] private Sprite TextureUltimate;
    [SerializeField] private Sprite TextureColor5;
    [SerializeField] private Sprite TextureBomb;
    [SerializeField] private Sprite TextureFly;
    [SerializeField] private Sprite TextureBlocker;
    [SerializeField] private Sprite TextureRocketHorizontal;
    [SerializeField] private Sprite TextureRocketVertical;

    [SerializeField] private Sprite TextureDispencer;
    [SerializeField] private Sprite[] TextureBox = new Sprite[5];
    [SerializeField] private Sprite TexturePortal;
    [SerializeField] private Color[] PortalColors = new Color[5];
    [SerializeField] private Sprite[] TextureWall = new Sprite[5];
    [SerializeField] private Sprite TextureRock;

    private void Awake()
    {
        main = this;
        _levelNumIF.text = "0";
        LoadLevel();
    }

    #if UNITY_EDITOR
    public void Update()
    {
        CheckInput();
    }
    #endif

    public void SaveLevel()
    {
        SaveInZeroLevel();
        _levelsObject.levels[int.Parse(_levelNumIF.text)] = new LevelsScript.Level(_levelsObject.levels[0]);
    }

    public void LoadLevel()
    {
        _levelsObject.levels[0] = new LevelsScript.Level(_levelsObject.levels[int.Parse(_levelNumIF.text)]);
        SetValuesInText();
        SelectCell(new Vector2Int(0, 0));
    }

    public void GenerateLevel()
    {
        _levelsObject.levels[0] = new LevelsScript.Level(LevelGenerator.main.GenerateLevelV3(int.Parse(_levelNumIF.text), _levelsObject.levels[0]));
        SetValuesInText();
        SelectCell(new Vector2Int(0, 0));
    }

    private void SetValuesInText()
    {
        _sizeXIF.text = _levelsObject.levels[0].Width.ToString();
        _sizeYIF.text = _levelsObject.levels[0].Height.ToString();
        _moveIF.text = _levelsObject.levels[0].Move.ToString();
        _needScoreIF.text = _levelsObject.levels[0].NeedScore.ToString();
        _numColorsIF.text = _levelsObject.levels[0].NumColors.ToString();
        _difficultIF.text = _levelsObject.levels[0].Difficult.ToString();

        _passedWithScore.isOn = _levelsObject.levels[0].PassedWithScore;
        _passedWithCrystal.isOn = _levelsObject.levels[0].PassedWithCrystal;
        _passedWithBox.isOn = _levelsObject.levels[0].PassedWithBox;
        _passedWithMold.isOn = _levelsObject.levels[0].PassedWithMold;
        _passedWithIce.isOn = _levelsObject.levels[0].PassedWithIce;
        _passedWithPanel.isOn = _levelsObject.levels[0].PassedWithPanel;
        _passedWithRock.isOn = _levelsObject.levels[0].PassedWithRock;
        _passedWithEnemy.isOn = _levelsObject.levels[0].PassedWithEnemy;

        _crystalColorDropdown.value = (int)_levelsObject.levels[0].NeedColor;
        _crystalNumIF.text = _levelsObject.levels[0].NeedCrystal.ToString();

        _blockerSlider.value = _levelsObject.levels[0].TypeBlockerPercent;
        _superColorSlider.value = _levelsObject.levels[0].SuperColorPercent;

        UpdateField();
    }

    private void SaveInZeroLevel()
    {
        _levelsObject.levels[0].Move = int.Parse(_moveIF.text);
        _levelsObject.levels[0].NeedScore = int.Parse(_needScoreIF.text);
        _levelsObject.levels[0].NumColors = int.Parse(_numColorsIF.text);
        _levelsObject.levels[0].Difficult = int.Parse(_difficultIF.text);

        _levelsObject.levels[0].PassedWithScore = _passedWithScore.isOn;
        _levelsObject.levels[0].PassedWithCrystal = _passedWithCrystal.isOn;
        _levelsObject.levels[0].PassedWithBox = _passedWithBox.isOn;
        _levelsObject.levels[0].PassedWithMold = _passedWithMold.isOn;
        _levelsObject.levels[0].PassedWithIce = _passedWithIce.isOn;
        _levelsObject.levels[0].PassedWithPanel = _passedWithPanel.isOn;
        _levelsObject.levels[0].PassedWithRock = _passedWithRock.isOn;
        _levelsObject.levels[0].PassedWithEnemy = _passedWithEnemy.isOn;

        _levelsObject.levels[0].NeedColor = (CellInternalObject.InternalColor)_crystalColorDropdown.value;
        _levelsObject.levels[0].NeedCrystal = int.Parse(_crystalNumIF.text);

        _levelsObject.levels[0].TypeBlockerPercent = (int)_blockerSlider.value;
        _levelsObject.levels[0].SuperColorPercent = (int)_superColorSlider.value;
    }

    public void SaveSizeInZeroLevel()
    {
        Vector2Int oldSize = new Vector2Int(_levelsObject.levels[0].Width, _levelsObject.levels[0].Height);

        _levelsObject.levels[0].Height = int.Parse(_sizeYIF.text);
        _levelsObject.levels[0].Width = int.Parse(_sizeXIF.text);
        UpdateFieldInZeroLevel(oldSize);
    }
    private void UpdateFieldInZeroLevel(Vector2Int oldSize)
    {
        LevelsScript.CellInfo[] oldCells = new LevelsScript.CellInfo[_levelsObject.levels[0].cellsArray.Length];
        for (int i = 0; i < oldCells.Length; i++)
        {
            oldCells[i] = new LevelsScript.CellInfo(_levelsObject.levels[0].cellsArray[i]);
        }


        if (_levelsObject.levels[0].cellsArray.Length != _levelsObject.levels[0].Width * _levelsObject.levels[0].Height)
        {
            _levelsObject.levels[0].cellsArray = new LevelsScript.CellInfo[_levelsObject.levels[0].Width * _levelsObject.levels[0].Height];
        }

        for (int x = 0; x < oldSize.x; x++)
        {
            for (int y = 0; y < oldSize.y; y++)
            {
                try {
                    if (_levelsObject.levels[0].cellsArray[x + y * _levelsObject.levels[0].Width] == null)
                        _levelsObject.levels[0].cellsArray[x + y * _levelsObject.levels[0].Width] = new LevelsScript.CellInfo(oldCells[x + y * oldSize.x]);
                }
                catch { }
            }
        }

        for (int i = 0; i < _levelsObject.levels[0].cellsArray.Length; i++)
        {
            if (_levelsObject.levels[0].cellsArray[i] == null)
                _levelsObject.levels[0].cellsArray[i] = new LevelsScript.CellInfo();
        }

        UpdateField();
    }

    private void UpdateField()
    {
        if (_cells != null && _cells.Length > 0)
        {
            foreach (CellRedactor cell in _cells)
            {
                Destroy(cell.gameObject);
            }
        }

        _gameFieldTransform.sizeDelta = new Vector2(_heightOneCell * _levelsObject.levels[0].Width, _heightOneCell * _levelsObject.levels[0].Height);

        //_levelsObject.levels[0].cellsArray = new LevelsScript.CellInfo[_levelsObject.levels[0].Width * _levelsObject.levels[0].Height];
        _cells = new CellRedactor[_levelsObject.levels[0].Width, _levelsObject.levels[0].Height];

        Vector2 startPos = -_gameFieldTransform.sizeDelta / 2;
        startPos = new Vector2(startPos.x + _heightOneCell / 2, startPos.y + _heightOneCell / 2);
        for (int x = 0; x < _levelsObject.levels[0].Width; x++)
        {
            for (int y = 0; y < _levelsObject.levels[0].Height; y++)
            {
                _cells[x, y] = Instantiate(_cellPref, _gameFieldTransform);

                Vector2 pos = new Vector2(startPos.x + x * _heightOneCell, startPos.y + y * _heightOneCell);
                _cells[x, y].GetComponent<RectTransform>().anchoredPosition = pos;
                _cells[x, y].UpdatePos(new Vector2Int(x, y));

                UpdateCell(x, y);
            }
        }
    }

    private void UpdateCell(int x, int y)
    {
        LevelsScript.CellInfo cell = new LevelsScript.CellInfo(_levelsObject.levels[0].cellsArray[x + y * _levelsObject.levels[0].Width]);
        if (cell.Exist == 0)
        {
            if (cell.dispencer)
                _cells[x, y].UpdateImages(TextureExist, TextureDispencer);
            else if (cell.teleport > 0)
                _cells[x, y].UpdateImages(TextureExist, Color.white, null, Color.white, null, TexturePortal, PortalColors[cell.teleport - 1], null, " ");
            else
                _cells[x, y].UpdateImages(TextureExist, null);
            return;
        }

        Sprite back = null,
                obj = null,
                rock = null,
                port = null,
                walls = null;
        Color cBack = Color.white,
                cObj = Color.white,
                cPort = Color.white;
        string str = "";

        TypeObject();

        if (cell.HealthBox > 0)
        {
            obj = TextureBox[cell.HealthBox - 1];
            cObj = Color.white;
            str += $"B{cell.HealthBox}";
        }
        if (cell.rock > 0)
        {
            rock = TextureRock;
        }
        if (cell.HealthIce > 0)
        {
            back = TextureIce[cell.HealthIce - 1];
            str += $"I{cell.HealthIce}";
        }
        if (cell.Panel > 0)
        {
            back = TexturePanel;
        }
        if (cell.HealthMold > 0)
        {
            back = TextureMold;
            cBack = Color.red;
        }
        if (cell.wall > 0)
        {
            walls = TextureWall[cell.wall - 1];
            str += $"W{cell.wall}";
        }
        if (cell.teleport > 0)
        {
            port = TexturePortal;
            cPort = PortalColors[cell.teleport - 1];
            str += $"P{cell.teleport}";
        }

        _cells[x, y].UpdateImages(back, cBack, obj, cObj, rock, port, cPort, walls, str);

        void TypeObject()
        {
            if (cell.typeCell == CellInternalObject.Type.color)
            {
                obj = SpriteColorObject();
            }
            else
            {
                cObj = ColorObject();
                switch (cell.typeCell)
                {
                    case CellInternalObject.Type.color5:
                        obj = TextureColor5;
                        cObj = Color.white;
                        break;
                    case CellInternalObject.Type.rocketHorizontal:
                        obj = TextureRocketHorizontal;
                        break;
                    case CellInternalObject.Type.rocketVertical:
                        obj = TextureRocketVertical;
                        break;
                    case CellInternalObject.Type.bomb:
                        obj = TextureBomb;
                        break;
                    case CellInternalObject.Type.blocker:
                        obj = TextureBlocker;
                        cObj = Color.white;
                        break;
                    case CellInternalObject.Type.airplane:
                        obj = TextureFly;

                        break;
                }
            }


        }

        Color ColorObject()
        {
            switch (cell.colorCell)
            {
                case CellInternalObject.InternalColor.Blue:
                    return Color.blue;

                case CellInternalObject.InternalColor.Green:
                    return Color.green;

                case CellInternalObject.InternalColor.Red:
                    return Color.red;

                case CellInternalObject.InternalColor.Yellow:
                    return Color.yellow;

                default:
                    return Color.white;
            }
        }

        Sprite SpriteColorObject()
        {
            switch (cell.colorCell)
            {
                case CellInternalObject.InternalColor.Blue:
                    return TextureBlue;

                case CellInternalObject.InternalColor.Green:
                    return TextureGreen;

                case CellInternalObject.InternalColor.Red:
                    return TextureRed;

                case CellInternalObject.InternalColor.Yellow:
                    return TextureYellow;

                case CellInternalObject.InternalColor.Ultimate:
                    return TextureUltimate;

                case CellInternalObject.InternalColor.Violet:
                    return TextureOrange;

                default:
                    return TextureRed;
            }
        }
    }

    public void SelectCell(Vector2Int pos)
    {
        _selectCellPos = pos.x + pos.y * _levelsObject.levels[0].Width;
        _SelectedCellPosText.text = $"{pos.x} : {pos.y}";
        SetValuesInCellText();
    }

    private void SetValuesInCellText()
    {
        if (_levelsObject.levels[0].cellsArray[_selectCellPos].Exist > 0)
            _ExistSelectedCell.isOn = true;
        else
            _ExistSelectedCell.isOn = false;

        _DispencerSelectedCell.isOn = _levelsObject.levels[0].cellsArray[_selectCellPos].dispencer;

        if (_levelsObject.levels[0].cellsArray[_selectCellPos].rock > 0)
            _RockSelectedCell.isOn = true;
        else
            _RockSelectedCell.isOn = false;

        if (_levelsObject.levels[0].cellsArray[_selectCellPos].Panel > 0)
            _PanelSelectedCell.isOn = true;
        else
            _PanelSelectedCell.isOn = false;

        if (_levelsObject.levels[0].cellsArray[_selectCellPos].HealthMold > 0)
            _MoldSelectedCell.isOn = true;
        else
            _MoldSelectedCell.isOn = false;

        _BoxHealthSelectedCellSlider.value = _levelsObject.levels[0].cellsArray[_selectCellPos].HealthBox;
        _PortalSelectedCellSlider.value = _levelsObject.levels[0].cellsArray[_selectCellPos].teleport;
        _WallsTypeSelectedCellSlider.value = _levelsObject.levels[0].cellsArray[_selectCellPos].wall;
        _IceHealthSelectedCellSlider.value = _levelsObject.levels[0].cellsArray[_selectCellPos].HealthIce;
        _TypeSelectedCellSlider.value = (int)_levelsObject.levels[0].cellsArray[_selectCellPos].typeCell;
        _ColorSelectedCellSlider.value = (int)_levelsObject.levels[0].cellsArray[_selectCellPos].colorCell;

        UpdateSliderValuesInCellText();
    }

    public void UpdateSliderValuesInCellText()
    {
        _BoxHealthSelectedCellText.text = $"Box health:{_BoxHealthSelectedCellSlider.value}";
        _PortalSelectedCellText.text = $"Portal num:{_PortalSelectedCellSlider.value}";
        _WallsTypeSelectedCellText.text = $"Wall type:{_WallsName[(int)_WallsTypeSelectedCellSlider.value]}";
        _IceHealthSelectedCellText.text = $"Ice health:{_IceHealthSelectedCellSlider.value}";
        _TypeSelectedCellText.text = $"Type:{_TypesName[(int)_TypeSelectedCellSlider.value]}";
        _ColorSelectedCellText.text = $"Color:{_ColorsName[(int)_ColorSelectedCellSlider.value]}";
    }
    public void SaveCellValuesInZeroLevel()
    {

        if (_ExistSelectedCell.isOn)
            _levelsObject.levels[0].cellsArray[_selectCellPos].Exist = 1;
        else
            _levelsObject.levels[0].cellsArray[_selectCellPos].Exist = 0;

        _levelsObject.levels[0].cellsArray[_selectCellPos].dispencer = _DispencerSelectedCell.isOn;

        if (_RockSelectedCell.isOn)
            _levelsObject.levels[0].cellsArray[_selectCellPos].rock = 1;
        else
            _levelsObject.levels[0].cellsArray[_selectCellPos].rock = 0;

        if (_PanelSelectedCell.isOn)
            _levelsObject.levels[0].cellsArray[_selectCellPos].Panel = 1;
        else
            _levelsObject.levels[0].cellsArray[_selectCellPos].Panel = 0;

        if (_MoldSelectedCell.isOn)
            _levelsObject.levels[0].cellsArray[_selectCellPos].HealthMold = 1;
        else
            _levelsObject.levels[0].cellsArray[_selectCellPos].HealthMold = 0;

        _levelsObject.levels[0].cellsArray[_selectCellPos].HealthBox = (int)_BoxHealthSelectedCellSlider.value;

        _levelsObject.levels[0].cellsArray[_selectCellPos].teleport = (int)_PortalSelectedCellSlider.value;

        _levelsObject.levels[0].cellsArray[_selectCellPos].wall = (int)_WallsTypeSelectedCellSlider.value;

        _levelsObject.levels[0].cellsArray[_selectCellPos].HealthIce = (int)_IceHealthSelectedCellSlider.value;

        _levelsObject.levels[0].cellsArray[_selectCellPos].typeCell = (CellInternalObject.Type)_TypeSelectedCellSlider.value;

        _levelsObject.levels[0].cellsArray[_selectCellPos].colorCell = (CellInternalObject.InternalColor)_ColorSelectedCellSlider.value;

        UpdateCell(_selectCellPos % _levelsObject.levels[0].Width, _selectCellPos / _levelsObject.levels[0].Width);
    }

    public void CopyCell()
    {
        _cellsBuferImage.gameObject.SetActive(true);
        _cellBufer = new LevelsScript.CellInfo(_levelsObject.levels[0].cellsArray[_selectCellPos]);
        _cellsBuferImage.newCellRedactor(_cells[_selectCellPos % _levelsObject.levels[0].Width, _selectCellPos / _levelsObject.levels[0].Width]);
    }

    public void PastCell()
    {
        _levelsObject.levels[0].cellsArray[_selectCellPos] = new LevelsScript.CellInfo(_cellBufer);
        UpdateCell(_selectCellPos % _levelsObject.levels[0].Width, _selectCellPos / _levelsObject.levels[0].Width);
    }

    public void FastPastCell(Vector2Int pos)
    {
        if (_FastPast.isOn && Input.GetMouseButton(0))
        {
            _levelsObject.levels[0].cellsArray[pos.x + pos.y * _levelsObject.levels[0].Width] = new LevelsScript.CellInfo(_cellBufer);
            UpdateCell(pos.x, pos.y);
        }
    }

    public void MoveFieldUp()
    {
        MoveField(new Vector2Int(0, 1));
    }
    public void MoveFieldRight()
    {
        MoveField(new Vector2Int(1, 0));
    }
    public void MoveFieldLeft()
    {
        MoveField(new Vector2Int(-1, 0));
    }
    public void MoveFieldDown()
    {
        MoveField(new Vector2Int(0, -1));
    }

    private void MoveField(Vector2Int plusSize)
    {

        LevelsScript.CellInfo[] cells = new LevelsScript.CellInfo[_levelsObject.levels[0].cellsArray.Length];
        for (int i = 0; i < _levelsObject.levels[0].cellsArray.Length; i++)
        {
            cells[i] = new LevelsScript.CellInfo(_levelsObject.levels[0].cellsArray[i]);
        }

        for (int x = 0; x < _levelsObject.levels[0].Width; x++)
        {
            for (int y = 0; y < _levelsObject.levels[0].Height; y++)
            {
                int posX = x + plusSize.x;
                int posY = y + plusSize.y;

                if (posX >= _levelsObject.levels[0].Width || posX < 0)
                    posX = x + plusSize.x - _levelsObject.levels[0].Width * (plusSize.x / Mathf.Abs(plusSize.x));

                if (posY >= _levelsObject.levels[0].Height || posY < 0)
                    posY = y + plusSize.y - _levelsObject.levels[0].Height * (plusSize.y / Mathf.Abs(plusSize.y));

                _levelsObject.levels[0].cellsArray[posX + posY * _levelsObject.levels[0].Width] = cells[x + y * _levelsObject.levels[0].Width];
            }
        }

        UpdateField();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveFieldLeft();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveFieldUp();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveFieldDown();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveFieldRight();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _FastPast.isOn = !_FastPast.isOn;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CopyCell();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PastCell();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _levelsObject.Save();
        }
    }
}