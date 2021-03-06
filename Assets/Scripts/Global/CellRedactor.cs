using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//alexanr
/// <summary>
/// ????????? ??????? ? ????????? ???????
/// </summary>
public class CellRedactor : MonoBehaviour
{
    [SerializeField] private Image _cellImageBack; //???
    [SerializeField] private Image _cellImageBasic; //????, ??? ??????? ? ??
    [SerializeField] private Image _cellImageRock; //?????/??????
    [SerializeField] private Image _cellImagePortal; //???????
    [SerializeField] private Image _cellImageWalls; //??????? ?????
    [SerializeField] private Text _additionalText; //?????
    [SerializeField] private Vector2Int _cellPos; //???????

    public void newCellRedactor(CellRedactor cell)
    {
        ReplacementImage(_cellImageBack, cell._cellImageBack);
        ReplacementImage(_cellImageBasic, cell._cellImageBasic);
        ReplacementImage(_cellImageRock, cell._cellImageRock);
        ReplacementImage(_cellImagePortal, cell._cellImagePortal);
        ReplacementImage(_cellImageWalls, cell._cellImageWalls);
        _additionalText.text = cell._additionalText.text;
    }

    public void ReplacementImage(Image im1, Image im2)
    {
        if (im2.gameObject.activeSelf)
            UpdateImage(im1, im2.sprite, im2.color);
        else
            im1.gameObject.SetActive(false);
    }

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
    public void FastPast()
    {
        LevelRedactor.main.FastPastCell(_cellPos);
    }
}
