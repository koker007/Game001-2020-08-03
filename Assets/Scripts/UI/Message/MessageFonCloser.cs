using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MessageFonCloser : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (MessageCTRL.BufferMessages.Count > 0) {
            MessageCTRL.BufferMessages[0].DeleteMessageBuffer();
        }
        //MessageCTRL.selected.ClickButtonClose();
    }
}
