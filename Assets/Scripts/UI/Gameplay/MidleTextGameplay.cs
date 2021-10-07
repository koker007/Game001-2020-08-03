using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// онтролирует движение всплывающего текста
public class MidleTextGameplay: MonoBehaviour
{

    [SerializeField]
    float WaitingTime = 1;
    [SerializeField]
    bool opening = false;
    [SerializeField]
    bool closing = false;

    [SerializeField]
    Image fon;
    [SerializeField]
    Text text;

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

    void SetText(string testNew) {
        text.text = testNew;
    }


    void Destroy() {
        Destroy(gameObject);
    }
}
