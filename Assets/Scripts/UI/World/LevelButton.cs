using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// ������ ������ �������
/// </summary>
public class LevelButton : MonoBehaviour
{

    public Animator LevelAnim;

    private Collider2D butCollider;
    /// <summary>
    /// ����� ������
    /// </summary>
    public int NumLevel;

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
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == butCollider)
            {
                LevelAnim.SetBool("Touch", true);
            }
        }*/
        LevelAnim.SetBool("Touch", true);
    }

    private void OnMouseExit()
    {
        LevelAnim.SetBool("Touch", false);
    }

    private void OnMouseUpAsButton()
    {
        LevelAnim.SetBool("Touch", false);
        Debug.Log("klick");
    }
}
