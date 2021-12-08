using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//алкександр
//Семен
/// <summary>
/// Вращение мира прикосновением. И проверка на клик по кнопке
/// </summary>
public class WorldSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static WorldSlider main;

    private Vector2 StartTouchPosition;

    private const float SpeedRotation = 0.05f;
    private float Srotation;
    public float StartRotation = -115;

    private bool isDown;

    LevelButton3D ButtonStart;

    //нажатие мыши на обьект
    public void OnPointerDown(PointerEventData eventData)
    {
        //Запомнить позицию нажатия
        StartTouchPosition = Input.mousePosition;
        Srotation = WorldGenerateScene.main.rotationNeed;
        this.isDown = true;

        
        //Пускаем луч из камеры
        Ray ray = MainCamera.main.myCamera.ScreenPointToRay(Input.mousePosition*MainCamera.main.percent);
        RaycastHit hitInfo;

        //Если произошло столкновение и столкновение с кнопкой
        if (Physics.Raycast(ray, out hitInfo, 40))
        {
            //Запоминаем компонент кнопки
            ButtonStart = hitInfo.collider.GetComponent<LevelButton3D>();
            if(ButtonStart != null)
                ButtonStart.AnimStartPress();
        }
        else {
            ButtonStart = null;
        }
    }

    //отпускание мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;

        if (!ButtonStart) return;

        //Получить позицию отпускания
        //Если разница позиций не большая
        if (Vector3.Distance(StartTouchPosition, Input.mousePosition) < Screen.width * 0.1f)
        {
            //Пускаем луч из камеры
            Ray ray = MainCamera.main.myCamera.ScreenPointToRay(Input.mousePosition * MainCamera.main.percent);
            RaycastHit hitInfo;

            //Если произошло столкновение и столкновение с кнопкой
            if (Physics.Raycast(ray, out hitInfo, 40) && hitInfo.collider.gameObject == ButtonStart.gameObject)
            {
                if (PlayerProfile.main.ProfilelevelOpen >= ButtonStart.NumLevel)
                {
                    ButtonStart.ClickLevel();
                }
                else { 
                
                }
            }
        }

        ButtonStart.AnimEndPress();
        ButtonStart = null;
    }

    private void Start()
    {
        main = this;
        WorldGenerateScene.main.rotationNeed = StartRotation - (PlayerProfile.main.ProfilelevelOpen * 5 - 5);
    }

    private void Update()
    {
        if (isDown)
        {
            //изменение переменной поворота
            WorldGenerateScene.main.rotationNeed = Srotation - (StartTouchPosition.y - Input.mousePosition.y) * SpeedRotation;
        }
    }


}
