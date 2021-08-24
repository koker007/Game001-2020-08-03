using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoldCTRL : MonoBehaviour
{
    CellCTRL myCell;
    RectTransform myRect;

    [SerializeField]
    RawImage image;


    bool isInicialize = false;
    public void inicialize(CellCTRL cellIni) {
        if (isInicialize) return;

        myCell = cellIni;
        IniRect();

        isInicialize = true;
    }

    void IniRect() {
        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);
    }


    int HealthOld = -1;
    void TestLife() {
        //Если здоровье не менялось
        if (HealthOld == myCell.mold)
            return;

        ChangeImage();

        DestroyMold();
        HealthOld = myCell.mold;

        void ChangeImage() {

        }

        //Уничтожить если жизни кончились
        void DestroyMold() {
            if (myCell.mold > 0) return;

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestLife();
    }
}
