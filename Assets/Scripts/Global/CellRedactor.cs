using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellRedactor : MonoBehaviour
{
    [SerializeField] private Image _cellImageBack; //фон
    [SerializeField] private Image _cellImageBasic; //цвет, тип обьекта и тд
    [SerializeField] private Image _cellImageRock; //сетка/камень
    [SerializeField] private Image _cellImageAdditional; //порталы стены
    [SerializeField] private Text _additionalText; //жизни

    public void UpdateImages(Sprite cellImageBack, Sprite cellImageBasic, Sprite cellImageRock, Sprite cellImageAdditional, string AdditionalText)
    {
        UpdateImage(_cellImageBack, cellImageBack);
        UpdateImage(_cellImageBasic, cellImageBasic);
        UpdateImage(_cellImageRock, cellImageRock);
        UpdateImage(_cellImageAdditional, cellImageAdditional);

        _additionalText.text = AdditionalText;
    }
    private void UpdateImage(Image image, Sprite sprite)
    {
        image.gameObject.SetActive(true);
        if (sprite == null)
            image.gameObject.SetActive(false);
        else
            image.sprite = sprite;
    }
}
