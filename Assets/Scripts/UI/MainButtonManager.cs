using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// ��������� ��������
/// </summary>
public class MainButtonManager : MonoBehaviour
{
    /// <summary>
    /// �������� ��������� ������
    /// </summary>
    public void PressButtonSound()
    {
        SoundCTRL.main.PlaySound(SoundCTRL.main.clipPressButton);
    }

}
