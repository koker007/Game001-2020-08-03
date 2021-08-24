using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                    myCell.myField.CountBoxBlocker--;
                    Destroy(gameObject);
                }
            }
        }
    }
}
