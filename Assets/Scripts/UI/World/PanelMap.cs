using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// �������� �� �����
/// </summary>
public class PanelMap : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    RectTransform Up;

    //����������
    [SerializeField]
    Text Health;
    [SerializeField]
    Text Ticket;
    [SerializeField]
    Text Gold;

    [SerializeField]
    Animator SpeedRotate;



    void Update()
    {
        updateButtons();

        UpdateText();

        updateRotate();
    }

    void UpdateText() {
        Health.text = System.Convert.ToString(PlayerProfile.main.Health.Amount);
        Ticket.text = System.Convert.ToString(PlayerProfile.main.Ticket.Amount);
        Gold.text = System.Convert.ToString(PlayerProfile.main.GoldAmount);
    }

    void OnEnable()
    {
        startButtons();
    }
    void updateButtons()
    {
        moving();

        void moving()
        {
            float upY = Up.pivot.y;

            upY += (1 - upY) * Time.unscaledDeltaTime * 4;

            Up.pivot = new Vector2(Up.pivot.x, upY);
        }
    }

    //��������� ������ � ��������� ���������
    void startButtons()
    {
        Up.pivot = new Vector2(Up.pivot.x, -3);
    }

    //������ ��������� ��������
    float angleTimeRot = 0;
    //����� ��������� �������� �������
    float angleTimeLastUpdate = 0;
    //������ � ���������� �����
    float oldTimeRot = 0;
    void updateRotate() {
        //���� ����� � ���������� ����� ������ ����� �����
        if (Time.unscaledTime - angleTimeLastUpdate > 10) {
            //�������� ������
            oldTimeRot = 0;
            SpeedRotate.SetBool("NeedOpen", false);
        }

        //���������� ����� ������� ��������
        angleTimeLastUpdate = Time.unscaledTime;

        //���������� � ����
        if (oldTimeRot != 0) {
            //������ ������� ����� ������� ��������� � ����������
            float raznica = Mathf.Abs(WorldGenerateScene.main.rotationNow - oldTimeRot);
            angleTimeRot += raznica;
            angleTimeRot -= Time.unscaledDeltaTime;
        }

        //���������� ���� ��� �� �������� � ����� ��������
        if (!SpeedRotate.GetBool("NeedOpen") && angleTimeRot > 30 && PlayerProfile.main.ProfilelevelOpen > 20) {
            SpeedRotate.SetBool("NeedOpen", true);
        }

        



        oldTimeRot = WorldGenerateScene.main.rotationNow;
    }
}
