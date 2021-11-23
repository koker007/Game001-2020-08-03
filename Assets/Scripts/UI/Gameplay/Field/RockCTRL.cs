using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockCTRL : MonoBehaviour
{

    [SerializeField]
    AnimatorCTRL animator;

    CellCTRL myCell;
    RectTransform myRect;

    [SerializeField]
    RawImage image;

    bool isInicialize = false;
    /// <summary>
    /// ���������������� ������
    /// </summary>
    public void inicialize(CellCTRL cellIni)
    {
        if (isInicialize) return;

        myCell = cellIni;

        //��������� � ������ ��� �������
        myCell.myField.rockCTRLs[cellIni.pos.x, cellIni.pos.y] = this;
        ReCalcRockCount();
        IniRect();

        isInicialize = true;
    }

    void IniRect()
    {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }

    int HealthOld = -1;
    void UpdateLife()
    {
        //���� �������� �� ��������
        if (HealthOld == myCell.rock)
            return;

        //���� �������� �����������, ������� ������
        if (HealthOld > myCell.rock) {
            Particle3dCTRL.CreateDestroyRock(myCell.myField.transform, myCell);
        }

        DestroyRock();
        HealthOld = myCell.rock;

        //���������� ���� ����� ���������
        void DestroyRock()
        {
            if (myCell.rock > 0) return;

            animator.PlayAnimation("Destroy");

            //���� ���������� �����
            SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipDamageRock, 0.5f, Random.Range(0.9f, 1.1f));
        }
    }

    //���������� ���� ����� ���������
    void Destroy()
    {
        myCell.myField.rockCTRLs[myCell.pos.x, myCell.pos.y] = null;
        ReCalcRockCount();

        Destroy(gameObject);

        //����������� ������ ������� �������� ��������������
    }

    void Update()
    {
        UpdateLife();
    }

    void ReCalcRockCount() {

        int count = 0;

        //���������� ��� ������� ����
        for (int x = 0; x < myCell.myField.rockCTRLs.GetLength(0); x++) {
            for (int y = 0; y < myCell.myField.rockCTRLs.GetLength(1); y++) {
                if (myCell.myField.rockCTRLs[x,y]) {
                    count++;
                }
            }
        }

        myCell.myField.CountRockBlocker = count;
    }

}
