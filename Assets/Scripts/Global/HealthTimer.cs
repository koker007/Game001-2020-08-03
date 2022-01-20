using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������� ����� ��� �������������� ������
/// </summary>
public class HealthTimer : MonoBehaviour
{
    public static HealthTimer main;

    [SerializeField] private Text _TimeRegenerationText;

    private bool _healthRegenerateStart;
    private int _TimeStartRegeneration;

    [SerializeField] private int _SystemTimeStartRegeneration;
    private const string _SystemTimeStartRegenerationID = "SystemTimeStartRegeneration";
    private const int _TimeForRegenerate = 60*5; //second
    private const int _maxLive = 5;
    private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc); //������ ������� �������
    
    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        HealthRegenerate();
    }


    //��� ������� ���� ��������� ������� �� ���� ������������
    public void HealthRegenerateRealTime()
    {
        _SystemTimeStartRegeneration = PlayerPrefs.GetInt(_SystemTimeStartRegenerationID, (int)(System.DateTime.UtcNow - epochStart).TotalSeconds);

        int inactiveGameTime = (int)((System.DateTime.UtcNow - epochStart).TotalSeconds - _SystemTimeStartRegeneration);
        int plusHealth = inactiveGameTime / _TimeForRegenerate;

        if (PlayerProfile.main.Health.Amount > _maxLive)
        {
            return;
        }
        if (plusHealth + PlayerProfile.main.Health.Amount > _maxLive)
        {
            PlayerProfile.main.SetHealth(_maxLive);
        }
        else
        {
            PlayerProfile.main.SetHealth(PlayerProfile.main.Health.Amount += plusHealth);
            TimerStart((int)Time.time - inactiveGameTime % _TimeForRegenerate, (int)(System.DateTime.UtcNow - epochStart).TotalSeconds - inactiveGameTime % _TimeForRegenerate);
        }
    }

    //�������� ����� �� �������������� �� � ������ �������
    private void HealthRegenerate()
    {
        if (PlayerProfile.main == null) return;

        if (PlayerProfile.main.Health.Amount >= _maxLive)
        {
            _TimeRegenerationText.text = "";
            _healthRegenerateStart = false;
            return;
        }

        if (_healthRegenerateStart)
        {
            if (_TimeForRegenerate <= (int)Time.time - _TimeStartRegeneration)
            {
                PlayerProfile.main.SetHealth(PlayerProfile.main.Health.Amount + 1);
                _TimeRegenerationText.text = "";
                _healthRegenerateStart = false;
            }
            else
            {
                int timeForRegenerate = _TimeForRegenerate - ((int)Time.time - _TimeStartRegeneration);
                int second = timeForRegenerate % 60;
                int minute = timeForRegenerate / 60;
                _TimeRegenerationText.text = $"{minute}:{second}";
            }
        }
        else
        {
            TimerStart((int)Time.time, (int)(System.DateTime.UtcNow - epochStart).TotalSeconds);
        }
    }
    //������������� �������� ������ �������
    private void TimerStart(int timerValue, int systemTimerValue)
    {
        _healthRegenerateStart = true;
        _SystemTimeStartRegeneration = systemTimerValue;
        PlayerPrefs.SetInt(_SystemTimeStartRegenerationID, _SystemTimeStartRegeneration);
        _TimeStartRegeneration = timerValue;
    }

}
