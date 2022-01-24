using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexanr
/// <summary>
/// управляет клеткой в редакторе уровней
/// </summary>
public class CellRedactor : MonoBehaviour
{
    [SerializeField] private Image _cellImageBack; //фон
    [SerializeField] private Image _cellImageBasic; //цвет, тип обьекта и тд
    [SerializeField] private Image _cellImageRock; //сетка/камень
    [SerializeField] private Image _cellImagePortal; //порталы
    [SerializeField] private Image _cellImageWalls; //порталы стены
    [SerializeField] private Text _additionalText; //жизни
    [SerializeField] private Vector2Int _cellPos; //позиция

    public void UpdateImages(Sprite cellImageBack, Color cellColorBack, Sprite cellImageBasic, Color cellColorBasic, Sprite cellImageRock, Sprite cellImagePortal, Color cellColorPortal, Sprite walls, string AdditionalText)
    {
        UpdateImage(_cellImageBack, cellImageBack, cellColorBack);
        UpdateImage(_cellImageBasic, cellImageBasic, cellColorBasic);
        UpdateImage(_cellImageRock, cellImageRock);
        UpdateImage(_cellImagePortal, cellImagePortal, cellColorPortal);
        UpdateImage(_cellImageWalls, walls);

        _additionalText.text = AdditionalText;
    }
    public void UpdateImages(Sprite cellImageBack, Sprite cellImageBasic)
    {
        UpdateImages(cellImageBack, Color.white, cellImageBasic, Color.white, null, null, Color.white, null, " ");
    }
    private void UpdateImage(Image image, Sprite sprite, Color color)
    {
        image.color = color;
        image.gameObject.SetActive(true);
        if (sprite == null)
            image.gameObject.SetActive(false);
        else
            image.sprite = sprite;
    }

    private void UpdateImage(Image image, Sprite sprite)
    {
        UpdateImage(image, sprite, Color.white);
    }

    public void UpdatePos(Vector2Int pos)
    {
        _cellPos = pos;
    }

    public void TouchCell()
    {
        LevelRedactor.main.SelectCell(_cellPos);
    }
}
