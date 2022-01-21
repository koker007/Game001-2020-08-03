using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRedactor : MonoBehaviour
{
    public static LevelRedactor main;

    [SerializeField] private LevelScriptableObject _levelsObject;
    [Space]
    [SerializeField] private CellRedactor _cellPref;
    [SerializeField] private CellRedactor[,] _cells;
    [SerializeField] private RectTransform _gameFieldTransform;
    [SerializeField] private const int _heightOneCell = 100;
    [Space]
    [SerializeField] private InputField _levelNumIF;
    [SerializeField] private InputField _sizeXIF;
    [SerializeField] private InputField _sizeYIF;
    [SerializeField] private InputField _moveIF;
    [SerializeField] private InputField _needScoreIF;
    [SerializeField] private InputField _numColorsIF;
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

    [Space]
    [SerializeField] private Sprite TextureExist;

    [SerializeField] private Sprite TextureIce;
    [SerializeField] private Sprite TextureMold;
    [SerializeField] private Sprite TexturePanel;

    [SerializeField] private Sprite TextureRed;
    [SerializeField] private Sprite TextureGreen;
    [SerializeField] private Sprite TextureBlue;
    [SerializeField] private Sprite TextureYellow;
    [SerializeField] private Sprite TextureOrange;
    [SerializeField] private Sprite TextureColor5;
    [SerializeField] private Sprite TextureBomb;
    [SerializeField] private Sprite TextureFly;
    [SerializeField] private Sprite TextureBlocker;
    [SerializeField] private Sprite TextureRocketHorizontal;
    [SerializeField] private Sprite TextureRocketVertical;

    [SerializeField] private Sprite TextureDispencer;
    [SerializeField] private Sprite TextureBox;
    [SerializeField] private Sprite TexturePortal;
    [SerializeField] private Sprite TextureWall;
    [SerializeField] private Sprite TextureRock;

    private void Awake()
    {
        main = this;
        _levelNumIF.text = "0";
        LoadLevel();
    }

    public void SaveLevel()
    {
        SaveInZeroLevel();
        _levelsObject.levels[int.Parse(_levelNumIF.text)] = new LevelsScript.Level(_levelsObject.levels[0]);
    }

    public void LoadLevel()
    {
        _levelsObject.levels[0] = new LevelsScript.Level(_levelsObject.levels[int.Parse(_levelNumIF.text)]);
        SetValuesInText();
    }

    private void SetValuesInText()
    {
        _sizeXIF.text = _levelsObject.levels[0].Width.ToString();
        _sizeYIF.text = _levelsObject.levels[0].Height.ToString();
        _moveIF.text = _levelsObject.levels[0].Move.ToString();
        _needScoreIF.text = _levelsObject.levels[0].NeedScore.ToString();
        _numColorsIF.text = _levelsObject.levels[0].NumColors.ToString();

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

        UpdtaeField();
    }

    public void SaveInZeroLevel()
    {
        _levelsObject.levels[0].Width = int.Parse(_sizeXIF.text);
        _levelsObject.levels[0].Height = int.Parse(_sizeYIF.text);
        _levelsObject.levels[0].Move = int.Parse(_moveIF.text);
        _levelsObject.levels[0].NeedScore = int.Parse(_needScoreIF.text);
        _levelsObject.levels[0].NumColors = int.Parse(_numColorsIF.text);

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

        UpdtaeField();
    }

    public void UpdateCellsArray()
    {

    }

    public void SaveSizeInZeroLevel()
    {
        _levelsObject.levels[0].Height = int.Parse(_sizeYIF.text);
        _levelsObject.levels[0].Move = int.Parse(_moveIF.text);
    }

    private void UpdtaeField()
    {
        if(_cells != null && _cells.Length > 0)
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
                _cells[x,y] = Instantiate(_cellPref, _gameFieldTransform);

                Vector2 pos = new Vector2(startPos.x + x * _heightOneCell, startPos.y + y * _heightOneCell);
                _cells[x, y].GetComponent<RectTransform>().anchoredPosition = pos;

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
                _cells[x, y].UpdateImages(null, TextureDispencer, null, null, "");
            else
                _cells[x, y].UpdateImages(TextureExist, null, null, null, "");
            return;
        }

        Sprite  back = null, 
                obj = null, 
                rock = null, 
                add = null;
        string str = "";

        TypeObject();

        if (cell.HealthBox > 0)
        {
            obj = TextureBox;
            str += $"B{cell.HealthBox}";
        }
        if (cell.rock > 0)
        {
            rock = TextureRock;
        }
        if (cell.HealthIce > 0)
        {
            back = TextureIce;
            str += $"I{cell.HealthIce}";
        }
        if (cell.Panel > 0)
        {
            back = TexturePanel;
        }
        if (cell.HealthMold > 0)
        {
            back = TextureMold;
        }
        if (cell.teleport > 0)
        {
            add = TexturePortal;
            str += $"P{cell.teleport}";
        }
        if (cell.wall > 0)
        {
            add = TextureWall;
            str += $"W{cell.wall}";
        }

        _cells[x, y].UpdateImages(back, obj, rock, add, "");

        void TypeObject()
        {
            obj = TextureRed;
        }
    }
}
