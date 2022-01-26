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
    Text PlayerLevel;
    [SerializeField]
    Text levelOpen;
    [SerializeField]
    Text levelGold;

    [SerializeField]
    Text SaveText;
    string isSavedText = "Saved";
    string isSaveProcess = "Saving..";
    string isSaveBasic = "Save";

    bool isClickedSave = false;

    void Update()
    {
        UpdateProfile();
        UpdateButtonSave();
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
        isClickedSave = false;

        levelOpen.text = PlayerProfile.main.ProfilelevelOpen.ToString();
        levelGold.text = PlayerProfile.main.LVLGoldCount.ToString();

        isSaveBasic = TranslateManager.main.GetText("ProfileSaveBasic", isSaveBasic);
        isSaveProcess = TranslateManager.main.GetText("ProfileSaveProcess", isSaveProcess);
        isSavedText = TranslateManager.main.GetText("ProfileSaved", isSavedText);

        PlayerLevel.text = PlayerProfile.main.ProfileLevel.ToString();
        PlayerScore.text = ((int)PlayerProfile.main.ProfileCurrentLVLScoreNow).ToString() + " / " + ((int)PlayerProfile.main.ProfileCurrentLVLScoreMax).ToString();
    }

    public void ClickPlay()
    {
        GlobalMessage.LevelInfo(PlayerProfile.main.ProfilelevelOpen);
        MenuWorld.main.OpenMapPanel();
    }

    void UpdateProfile()
    {
        if (slider.value > PlayerProfile.main.ProfileCurrentLVLScoreNow) {
            slider.value = 0;
        }

        slider.value += (PlayerProfile.main.ProfileCurrentLVLScoreNow - slider.value) * Time.unscaledDeltaTime;
        slider.maxValue = PlayerProfile.main.ProfileCurrentLVLScoreMax;

    }

    void UpdateButtonSave() {

        if (GooglePlay.main.isSavingProcessNow())
        {
            SaveText.text = isSaveProcess;
        }
        else if (isClickedSave) {
            SaveText.text = isSavedText;
        }
        else {
            SaveText.text = isSaveBasic;
        }
    }

    public void ClickSave() {
        isClickedSave = true;
        PlayerProfile.main.Save();
    }
    
}
