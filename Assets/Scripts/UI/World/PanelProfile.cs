using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// �������� �� ������ �������
/// </summary>
public class PanelProfile : MonoBehaviour
{

    [SerializeField]
    Image Fon;
    [SerializeField]
    GameObject PlayerIcon;

    [SerializeField]
    Text PremiumDateEnd;
    [SerializeField]
    GameObject PlayerRamkaGold;
    [SerializeField]
    GameObject PlayerRamkaBace;


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

        UpdateSubscription();
    }

    private void OnEnable()
    {
        Inicialize();
        SetActiveAll();

        Invoke("GetPlayerLVLGift", 2f);

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

        isSaveBasic = TranslateManager.main.GetText("ProfileSaveBasic");
        isSaveProcess = TranslateManager.main.GetText("ProfileSaveProcess");
        isSavedText = TranslateManager.main.GetText("ProfileSaved");

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



    //��������
    System.DateTime subscriptionDateEndOld = new System.DateTime(1, 2, 3);
    void UpdateSubscription() {

        if (subscriptionDateEndOld == PlayerProfile.main.subscriptionDateEnd)
            return;

        subscriptionDateEndOld = PlayerProfile.main.subscriptionDateEnd;

        //���� ����� ���� �������� ����������, �� ��� ��� ���������
        //��������� �� ������ ������ ��� ���������� � ��������

        //�������� ��� ���������
        if (PlayerProfile.main.subscriptionDateEnd > System.DateTime.Now)
        {
            PlayerRamkaGold.SetActive(true);
            PlayerRamkaBace.SetActive(false);

        }
        //�������� �����������
        else {
            PlayerRamkaGold.SetActive(false);
            PlayerRamkaBace.SetActive(true);
        }

        string strMonth = "";
        string strDay = "";

        //���� ����� �� ����������� ���������� ����
        if (PlayerProfile.main.subscriptionDateEnd.Month < 10) strMonth += "0";
        if (PlayerProfile.main.subscriptionDateEnd.Day < 10) strDay += "0";

        strMonth += PlayerProfile.main.subscriptionDateEnd.Month;
        strDay += PlayerProfile.main.subscriptionDateEnd.Day;

        //��������� ����� ��������� ��������
        PremiumDateEnd.text = PlayerProfile.main.subscriptionDateEnd.Year + "." + strMonth + "." + strDay;

    }

    void GetPlayerLVLGift() {
        //������� ���� ���� ������� ������ �� �������
        //������� ���� ����� �� ������ �������� �������
        //���� ��������� � ������� ��� ��������
        if (!PlayerProfile.main.CanPlusPlayerLVLGift() ||
            !gameObject.activeSelf ||
            MessageGetGiftNewProfileLevel.main != null
            ) return;

        //������� ��������� � ��������� ������� (�� �������������)
        GlobalMessage.GetGiftNewProfileLVL(false);

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

    public void ClickDeleteProfile() {
        GlobalMessage.DeleteProfile();
    }
}
