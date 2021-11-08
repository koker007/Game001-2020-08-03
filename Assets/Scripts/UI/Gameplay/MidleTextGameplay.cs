using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������������ �������� ������������ ������
public class MidleTextGameplay: MonoBehaviour
{

    [SerializeField]
    Image fon;
    [SerializeField]
    Text text;

    [SerializeField]
    AudioClip audioClip;

    [SerializeField]
    Animator AnimMigleTextComponent;

    public void SetText(string testNew) {
        text.text = testNew;
    }

    public void SetColorFon(Color colorNew) {
        fon.color = colorNew;
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
