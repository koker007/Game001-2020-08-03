using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// Скрипт кнопок уровней
/// </summary>
public class LevelButton : MonoBehaviour
{

    public Animator LevelAnim;

    private Collider2D butCollider;
    /// <summary>
    /// номер уровня
    /// </summary>
    public int NumLevel;

    private void Awake()
    {
        butCollider = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        //поворот кнопки к камере
        transform.LookAt(MainComponents.MainCamera.transform);
    }


    //отслеживание нажатий на кнопку
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
