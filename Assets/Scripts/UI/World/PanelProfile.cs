using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Отвечает за панель профиля
/// </summary>
public class PanelProfile : MonoBehaviour
{

    [SerializeField]
    Image Fon;
    [SerializeField]
    GameObject PlayerIcon;
    [SerializeField]
    GameObject Achivements;

    [SerializeField]
    Text levelOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Inicialize();
        SetActiveAll();
    }

    private void OnDisable()
    {
        SetDisableAll();
    }

    void SetActiveAll() {
        PlayerIcon.SetActive(true);
        Achivements.SetActive(true);
    }

    void SetDisableAll() {
        PlayerIcon.SetActive(false);
        Achivements.SetActive(false);
    }

    void Inicialize() {
        //levelOpen.text = System.Convert.ToString(Gameplay.main.levelOpen);
    }

    public void ClickPlay()
    {
        GlobalMessage.LevelInfo(Gameplay.main.levelOpen);
        MenuWorld.main.OpenMapPanel();
    }

    public void ClickSave()
    {
        GlobalMessage.ComingSoon();
    }
}
