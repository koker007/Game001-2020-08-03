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

    CellCTRL myCell;

    int healthOld = 0;

    /// <summary>
    /// Инициализация обьекта сразу после создания
    /// </summary>
    public void Inicialize(CellCTRL cellNew) {
        myCell = cellNew;

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

            void testImage() {
                Texture textureNow;
                if (myCell.BlockingMove < textures.Length)
                {
                    textureNow = textures[myCell.BlockingMove];
                }
                else {
                    textureNow = textures[textures.Length-1];
                }

                image.texture = textureNow;
            }


            void testDestroy() {
                if (myCell.BlockingMove <= 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
