using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//alexandr
/// <summary>
/// ������ ������ �������
/// </summary>
public class LevelButton : MonoBehaviour
{

    public Animator LevelAnim;
    public Text text;

    private Collider2D butCollider;
    /// <summary>
    /// ����� ������
    /// </summary>
    public int NumLevel = 0;

    private void Awake()
    {
        butCollider = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        //������� ������ � ������
        transform.LookAt(MainComponents.MainCamera.transform);
    }


    //������������ ������� �� ������
    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.tag == "LevelButton")
            {
                LevelAnim.SetBool("Touch", true);
            }
        }
    }

    private void OnMouseExit()
    {
        LevelAnim.SetBool("Touch", false);
    }

    private void OnMouseUpAsButton()
    {
        LevelAnim.SetBool("Touch", false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.transform.tag == "LevelButton")
            {
                GlobalMessage.LevelInfo(NumLevel);
                Debug.Log("klick");
            }
        }
    }

    public void LevelUpdate(int lev)
    {
        NumLevel = lev;
        text.text = lev.ToString();
    }
}
