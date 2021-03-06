using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//?????

/// <summary>
/// ????????? ?????????? ?????????????????? ?????? (????)
/// </summary>
public class PanelSpreadCTRL : MonoBehaviour
{
    static float offsetY = 0;
    static float offsetYTimeOld = 0;

    CellCTRL myCell;
    [SerializeField]
    RawImage image;

    [SerializeField]
    Animator myAnimator;

    RectTransform myRect;

    public void inicialize(CellCTRL cell) {
        myCell = cell;

        myCell.myField.CountPanelSpread++;

        myRect = GetComponent<RectTransform>();
        myRect.pivot = new Vector2(-myCell.pos.x, -myCell.pos.y);

    }

    void Update()
    {
        UpdateOffSet();
    }

    //???? ?????? ?? ?????-?? ??????? ????? ???????, ????????? ???????
    ~PanelSpreadCTRL() {
        myCell.myField.CountPanelSpread--;
    }

    static public void TestOffSet() {
        if (offsetYTimeOld != Time.unscaledTime) {
            offsetYTimeOld = Time.unscaledTime;

            offsetY += Time.unscaledDeltaTime * 0.1f;
            
        }
    }

    void UpdateOffSet() {
        image.uvRect = new Rect(0, offsetY, image.uvRect.width, image.uvRect.height);
    }

    public void EffectCreate() {
        Particle3dCTRL.CreateCreateMaz(myCell.myField.transform, myCell);
    }


    public void DestoroyAnimator() {
        Destroy(myAnimator);
    }
}
