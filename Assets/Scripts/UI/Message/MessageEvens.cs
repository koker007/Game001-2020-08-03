using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageEvens : MonoBehaviour
{
    public void OkClick()
    {
        MainComponents.WorldUI.GetComponent<MenuWorld>().OpenArenaPanel();
    }
}
