using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageGetGiftNewProfileLevel : MonoBehaviour
{
    [SerializeField]
    public static MessageGetGiftNewProfileLevel main;

    [SerializeField]
    public MenuGameplay.SuperHitType typeGift;
    
    [SerializeField]
    public int countGift = 0;

    [Header("Textures")]
    [SerializeField]
    GameObject giftInternal;
    [SerializeField]
    GameObject giftRocket;
    [SerializeField]
    GameObject giftBomb;
    [SerializeField]
    GameObject giftMixed;
    [SerializeField]
    GameObject giftCorona;

    [Header("Components")]
    [SerializeField]
    public Text textLevel;
    [SerializeField]
    public Image imageType;
    [SerializeField]
    public Image imageGiftOpen;
    [SerializeField]
    public Image imageGiftClose;
    [SerializeField]
    public Text textCountGift;

    // Start is called before the first frame update
    void Start()
    {
        main = this;

        inicialize();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void inicialize() {
        //������������ � ����� �������
        typeGift = GetTypeGift();
        //������������ � �����������
        countGift = GetCountGift(typeGift);

        textLevel.text = PlayerProfile.main.ProfileLevelGetGift.ToString();

        int countNow = 0;
        if (typeGift == MenuGameplay.SuperHitType.internalObj) countNow = PlayerProfile.main.ShopInternal.Amount;
        else if (typeGift == MenuGameplay.SuperHitType.rosket2x) countNow = PlayerProfile.main.ShopRocket.Amount;
        else if (typeGift == MenuGameplay.SuperHitType.bomb) countNow = PlayerProfile.main.ShopBomb.Amount;
        else if (typeGift == MenuGameplay.SuperHitType.Color5) countNow = PlayerProfile.main.ShopColor5.Amount;
        else if (typeGift == MenuGameplay.SuperHitType.mixed) countNow = PlayerProfile.main.ShopMixed.Amount;

        if (countNow <= 0) {
            textCountGift.text =  "+" + countGift;
        }
        else {
            textCountGift.text = countNow + " +" + countGift;
        }

        openImageType();

        void closeAllImageType() {
            giftInternal.SetActive(false);
            giftRocket.SetActive(false); 
            giftBomb.SetActive(false);
            giftCorona.SetActive(false);
            giftMixed.SetActive(false);
        }
        void openImageType() {
            closeAllImageType();

            if (typeGift == MenuGameplay.SuperHitType.internalObj) giftInternal.SetActive(true);
            else if (typeGift == MenuGameplay.SuperHitType.rosket2x) giftRocket.SetActive(true);
            else if (typeGift == MenuGameplay.SuperHitType.bomb) giftBomb.SetActive(true);
            else if (typeGift == MenuGameplay.SuperHitType.Color5) giftCorona.SetActive(true);
            else if (typeGift == MenuGameplay.SuperHitType.mixed) giftMixed.SetActive(true);
        }
    }

    //�������� �������
    public void ClickButtonGetGift() {
        //��������� ����� �� �������� �������
        if (PlayerProfile.main.CanPlusPlayerLVLGift()) {

            //�������� �������
            GetGift();

            //�������� ��� �������� �������
            PlayerProfile.main.PlusPlayerLVLGift();
        }

        //��������� ��������� ��������� � �������
        MessageCTRL messageCTRL = MessageCTRL.selected;

        //���� ����� ��������� �������� ������� ��� ��� ����� �������� ���������
        if (PlayerProfile.main.CanPlusPlayerLVLGift()) {
            Destroy(messageCTRL.gameObject);

            //����� �������� ���������� �������� ��������� ������� (�������������)
            GlobalMessage.GetGiftNewProfileLVL(true);

        }
        //��������� ��������� ������
        else {
            messageCTRL.ClickButtonClose();
        }

        //�������� �������
        void GetGift()
        {
            //������
            if (typeGift == MenuGameplay.SuperHitType.internalObj) {
                PlayerProfile.main.ShopInternal.Amount += countGift;
            }
            else if (typeGift == MenuGameplay.SuperHitType.rosket2x)
            {
                PlayerProfile.main.ShopRocket.Amount += countGift;
            }
            else if (typeGift == MenuGameplay.SuperHitType.bomb) {
                PlayerProfile.main.ShopBomb.Amount += countGift;
            }
            else if (typeGift == MenuGameplay.SuperHitType.Color5) {
                PlayerProfile.main.ShopColor5.Amount += countGift;
            }
            else if (typeGift == MenuGameplay.SuperHitType.mixed) {
                PlayerProfile.main.ShopMixed.Amount += countGift;
            }



            //��������� ���������
            PlayerProfile.main.Save();

        }
    }

    //�������� ��� �������
    public MenuGameplay.SuperHitType GetTypeGift() {
        //������������ � ����� �������
        MenuGameplay.SuperHitType typeGift = MenuGameplay.SuperHitType.internalObj;

        //��������� ���� �� ���������������� ������� ��� �������� ������
        //���� ������� ������ ��� �������������� ��������
        if (PlayerProfile.main.ProfileLevelGetGift < PlayerProfile.main.ListGiftProfileLVL.Length)
        {
            typeGift = PlayerProfile.main.ListGiftProfileLVL[PlayerProfile.main.ProfileLevelGetGift];
        }
        //����� ����������� ������� ����
        else
        {
            typeGift = (MenuGameplay.SuperHitType)Random.Range(1, 6);
        }

        return typeGift;
    }


    //�������� ���������� ������� �� ������ ��� ����
    public int GetCountGift(MenuGameplay.SuperHitType typeGift) {

        float count = 1;
        if (typeGift == MenuGameplay.SuperHitType.internalObj) count = 2f;
        else if (typeGift == MenuGameplay.SuperHitType.rosket2x) count = 1.5f;
        else if (typeGift == MenuGameplay.SuperHitType.bomb) count = 1.5f;
        else if (typeGift == MenuGameplay.SuperHitType.Color5) count = 1f;
        else if (typeGift == MenuGameplay.SuperHitType.mixed) count = 3f;

        count += count * (0.3f * PlayerProfile.main.ProfileLevelGetGift);

        return (int)count;
    }
    public int GetCountGift()
    {
        return  GetCountGift(GetTypeGift());
    }
}
