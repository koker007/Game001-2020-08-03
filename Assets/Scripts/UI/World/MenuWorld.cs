using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен / алексадр
/// <summary>
/// Контролирует UI главного меню
/// </summary>
public class MenuWorld : MonoBehaviour
{
    static public MenuWorld main;



    [Header("Buttons")]
    public RectTransform Up;
    public RectTransform Down;
    public RectTransform Left;

    [Header("Panels")]
    [SerializeField]
    GameObject Map;
    [SerializeField]
    GameObject Profile;
    [SerializeField]
    GameObject Arena;
    [SerializeField]
    Text Gold;
    [SerializeField]
    Text Ticket;
    [SerializeField]
    Text Health;

    [Header("World")]
    [SerializeField]
    GameObject World3D;

    [HideInInspector]
    public enum UIIsOpen
    {
        Map,
        Profile,
        Arena
    }

    public UIIsOpen isOpen;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        OpenMapPanel();
    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
        ExitGame();
    }

    void ExitGame()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GlobalMessage.ExitGame();
            }
        }
    }

    void OnEnable()
    {
        startButtons();
        SetText();

        if (!World3D.activeSelf) {
            World3D.SetActive(true);
        }
    }
    void OnDisable()
    {
        if (World3D.activeSelf) {
            World3D.SetActive(false);
        }
    }


    void CloseAllPanels() {
        Map.SetActive(false);
        Profile.SetActive(false);
        Arena.SetActive(false);
    }

    /// <summary>
    /// Открыть панель игровой карты
    /// </summary>
    public void OpenMapPanel() {
        if (!Map.activeSelf) {
            CloseAllPanels();
            Map.SetActive(true);
            isOpen = UIIsOpen.Map;
        }
    }

    /// <summary>
    /// Открыть панель профиля игрока
    /// </summary>
    public void OpenProfilePanel() {
        if (!Profile.activeSelf) {
            CloseAllPanels();
            Profile.SetActive(true);
            isOpen = UIIsOpen.Profile;
        }
    }

    /// <summary>
    /// Открыть панель арены
    /// </summary>
    public void OpenArenaPanel() {
        if (!Arena.activeSelf)
        {
            CloseAllPanels();
            Arena.SetActive(true);
            isOpen = UIIsOpen.Arena;
        }
    }

    void updateButtons() {
        moving();

        void moving() {
            float downY = Down.pivot.y;
            downY += (0 - downY) * Time.unscaledDeltaTime * 4;
            Down.pivot = new Vector2(Down.pivot.x, downY);

            float upY = Up.pivot.y;
            upY += (1 - upY) * Time.unscaledDeltaTime * 4;
            Up.pivot = new Vector2(Up.pivot.x, upY);
        }
    }

    //Поставить панели в стартовое положение
    void startButtons() {
        Down.pivot = new Vector2(Down.pivot.x, 3);
        Up.pivot = new Vector2(Up.pivot.x, -3);
    }

    public bool isOpenMap() {
        return Map.activeSelf;
    }


    public void ClickButtonHealth() {
        GlobalMessage.Health();
    }
    public void ClickButtonTicket() {
        GlobalMessage.Tickets();
    }

    public void ClickButtonShop()
    {
        GlobalMessage.Shop();
    }
    public void ClickButtonEvents()
    {
        GlobalMessage.Events();
    }
    public void ClickButtonMessages()
    {
        GlobalMessage.ComingSoon();
    }

    public void ClickButtonRotateForvard() {
        WorldGenerateScene.main.rotationNeed -= 45;
    }
    public void ClickBottonRotateBack() {
        WorldGenerateScene.main.rotationNeed += 45;
    }

    public void SetText()
    {
        try{
            Gold.text = PlayerProfile.main.GoldAmount.ToString();
            Ticket.text = PlayerProfile.main.Ticket.Amount.ToString();
            Health.text = PlayerProfile.main.Health.Amount.ToString();
        }
        catch { }
    }
}
