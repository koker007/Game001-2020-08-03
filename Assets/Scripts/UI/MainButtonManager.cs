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
        SoundManager.main.PlaySound(SoundManager.main.PressButton);
    }

}
