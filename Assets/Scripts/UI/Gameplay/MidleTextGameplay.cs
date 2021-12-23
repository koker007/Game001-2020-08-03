using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
///////////////////////////////////////////////////////////////////////////////////
//������������ ����������� ����� � �������� (���������� ����� ��������)
public class MidleTextGameplay: MonoBehaviour
{

    [SerializeField]
    Image fon;
    [SerializeField]
    Text text;

    [Header("Images")]
    [SerializeField]
    Image imageCombo;
    [SerializeField]
    Image imageCongratulations;
    [SerializeField]
    Image imageEnemyMove;
    [SerializeField]
    Image imageNotBad;
    [SerializeField]
    Image imageYourMove;

    [SerializeField]
    AudioClip audioClip;

    [SerializeField]
    Animator AnimMigleTextComponent;

    public void SetText(string testNew) {
        text.text = testNew;
    }

    public void SetColorFon(Color colorNew) {
        fon.color = colorNew;
    }

    public void SetTextCombo() {
        CloseAll();
        imageCombo.gameObject.SetActive(true);
    }
    public void SetTextCongratulations()
    {
        CloseAll();
        imageCongratulations.gameObject.SetActive(true);
    }

    public void CloseAll() {
        fon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        imageCombo.gameObject.SetActive(false);
        imageCongratulations.gameObject.SetActive(false);
        imageEnemyMove.gameObject.SetActive(false);
        imageNotBad.gameObject.SetActive(false);
        imageYourMove.gameObject.SetActive(false);

    }

    //��������� ����� ������� � ���������. ��������
    void Destroy() {
        EnemyController.canEnemyMove = true;

        GameFieldCTRL.main.canPassTurn = true;
        
        Destroy(gameObject);
    }
}
