using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// онтролирует движение всплывающего текста
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(string testNew) {
        text.text = testNew;
    }

    public void SetColorFon(Color colorNew) {
        fon.color = colorNew;
    }

    public void SetAudio() {
        
    }


    void Destroy() {
        Destroy(gameObject);
    }
}
