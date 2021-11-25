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
    Slider slider;
    [SerializeField]
    Text PlayerScore;
    [SerializeField]
    Text PayerLevel;
    [SerializeField]
    Text levelOpen;

    void Update()
    {
        UpdateProfile();
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
        levelOpen.text = System.Convert.ToString(PlayerProfile.main.ProfilelevelOpen);
    }

    public void ClickPlay()
    {
        GlobalMessage.LevelInfo(PlayerProfile.main.ProfilelevelOpen);
        MenuWorld.main.OpenMapPanel();
    }

    public void ClickSave()
    {
        GlobalMessage.ComingSoon();
    }

    public void UpdateProfile()
    {
        int level = PlayerProfile.main.ProfileLevel;
        int score = PlayerProfile.main.ProfileScore;
        int nextLevelScore = PlayerProfile.main.nextLevelPoint[level - 1];

        PayerLevel.text = level.ToString();
        PlayerScore.text = score.ToString() + "/" + nextLevelScore.ToString();

        slider.value = (float)score / nextLevelScore;
    }
}
