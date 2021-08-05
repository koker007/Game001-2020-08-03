using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Panels")]
    [SerializeField]
    GameObject Map;
    [SerializeField]
    GameObject Profile;
    [SerializeField]
    GameObject Arena;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        updateButtons();
    }

    void OnEnable()
    {
        startButtons();
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
        CloseAllPanels();
        Map.SetActive(true);
    }

    /// <summary>
    /// Открыть панель профиля игрока
    /// </summary>
    public void OpenProfilePanel() {
        CloseAllPanels();
        Profile.SetActive(true);
    }

    /// <summary>
    /// Открыть панель арены
    /// </summary>
    public void OpenArenaPanel() {
        CloseAllPanels();
        Arena.SetActive(true);
    }

    void updateButtons() {
        moving();

        void moving() {
            float downY = Down.pivot.y;

            downY += (0 - downY) * Time.unscaledDeltaTime * 4;

            Down.pivot = new Vector2(Down.pivot.x, downY);
        }
    }

    //Поставить панели в стартовое положение
    void startButtons() {
        Down.pivot = new Vector2(Down.pivot.x, 3);
    }
}
