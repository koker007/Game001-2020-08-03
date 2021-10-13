using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTicket : MonoBehaviour
{
    [SerializeField]
    Text ticketAmount;

    public void OnEnable()
    {
        SetText();
    }
    public void BuyTicket()
    {
        PlayerProfile.main.isPurchaseItem(ref PlayerProfile.main.Ticket);
        SetText();
    }

    public void SetText()
    {
        ticketAmount.text = PlayerProfile.main.Ticket.Amount.ToString();
    }
}
