using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellRedactor : MonoBehaviour
{
    [SerializeField] private Image _cellImageBack; //���
    [SerializeField] private Image _cellImageBasic; //����, ��� ������� � ��
    [SerializeField] private Image _cellImageRock; //�����/������
    [SerializeField] private Image _cellImageAdditional; //������� �����
    [SerializeField] private Text _additionalText; //�����

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
