using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallController : MonoBehaviour
{ 
    public void SetWalls(int wallID)
    {
        //���������� ����������� ���� �� ID
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Walls/wallsID" + wallID);
    }
}
