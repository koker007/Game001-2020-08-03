using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен

/// <summary>
/// Контролирует префаб Коробки блокирующего движение
/// </summary>
public class BoxBlockCTRL : MonoBehaviour
{

    [SerializeField]
    Texture[] textures;
    [SerializeField]
    RawImage image;

    RectTransform myRect;

    CellCTRL myCell;

    int healthOld = 0;

    bool isInicialized = false;
    /// <summary>
    /// Инициализация обьекта сразу после создания
    /// </summary>
    public void Inicialize(CellCTRL cellNew) {
        if (isInicialized) return;

        myCell = cellNew;
        myCell.myField.CountBoxBlocker++;

        SetPos(-myCell.pos.x, -myCell.pos.y);

        myCell.myField.BoxBlockCTRLs[myCell.pos.x, myCell.pos.y] = this;
        ReCalcBoxCount();

        isInicialized = true;
    }

    void SetPos(int x, int y) {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(x,y);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        testLive();
    }

    //Проверка состояния обьекта
    void testLive() {
        if (healthOld != myCell.BlockingMove) {
            //Если жизней стало меньше
            if (healthOld > myCell.BlockingMove) {
                //Воспроизводим звук поломки
                SoundCTRL.main.SmartPlaySound(SoundCTRL.main.clipDamageAcne);
            }


            //Поменять изображение
            testImage();


            //Если жизни кончелись, самоуничтожаемся
            testDestroy();

            healthOld = myCell.BlockingMove;
            myCell.timeBoomOld = Time.unscaledTime;

            void testImage() {
                Texture textureNow;
                int array = myCell.BlockingMove - 1;
                if (myCell.BlockingMove < textures.Length && array >= 0)
                {
                    textureNow = textures[array];
                }
                else if(array < 0) {
                    textureNow = textures[0];
                }
                else
                {
                    textureNow = textures[textures.Length - 1];
                }

                image.texture = textureNow;
            }


            void testDestroy() {
                if (myCell.BlockingMove <= 0) {
                    //myCell.BoxBlock = null;


                    //myCell.myField.CountBoxBlocker--;

                    myCell.myField.BoxBlockCTRLs[myCell.pos.x, myCell.pos.y] = null;
                    ReCalcBoxCount();

                    Particle3dCTRL.CreateDestroyBox(myCell.myField.transform, myCell);

                    Destroy(gameObject);
                }
            }
        }
    }


    void ReCalcBoxCount()
    {

        int count = 0;

        //Перебираем все игровое поле
        for (int x = 0; x < myCell.myField.BoxBlockCTRLs.GetLength(0); x++)
        {
            for (int y = 0; y < myCell.myField.BoxBlockCTRLs.GetLength(1); y++)
            {
                if (myCell.myField.BoxBlockCTRLs[x, y])
                {
                    count++;
                }
            }
        }

        myCell.myField.CountBoxBlocker = count;
    }
}
