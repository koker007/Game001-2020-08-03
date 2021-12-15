using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������
//�����
//������ ���������� ������ ����� ���������� ������ ���� �������� ������
public class MarkerAnimationController : MonoBehaviour
{
    //private Animator animator;
    //public bool canPlayInAnim = false;
    //public bool canPlayOutAnim = false;
    //public bool isMarkerPlayingAnimation = false;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        startColor();
    }

    private void Update()
    {
        //PlayInAnim();
        //PlayOutAnim();

        updateColor();
    }

    /*
    private void PlayInAnim()
    {
        if (canPlayInAnim)
        {
            animator.Play("MarkerFrameAnimIn");
            canPlayInAnim = false;
        }
    }

    private void PlayOutAnim()
    {
        if (canPlayOutAnim)
        {
            animator.Play("MarkerFrameAnimOut");
            canPlayOutAnim = false;
        }
    }

    private void PlayIdleAnim()
    {
        animator.Play("MarkerFrameAnimIdle");      
    }
    */

    [SerializeField]
    Image image;
    
    //
    bool NeedPlus = true;
    float offsetSpeed = 1f;
    void updateColor() {

        Color color = image.color;

        //����������� ���� ���� ����
        if (NeedPlus)
        {
            color.a += offsetSpeed * Time.unscaledDeltaTime;

            if (color.a > 1)
                NeedPlus = false;
        }
        else {
            if (color.a > 0.1f)
                color.a -= offsetSpeed * Time.unscaledDeltaTime;
            else {
                color.a -= offsetSpeed * Time.unscaledDeltaTime * 0.1f;
            }
        }

        //���� ������� �� �������
        if (color.a < 0) {
            Destroy(gameObject);
        }

        //��������� ��������
        image.color = color;
    }

    void startColor() {
        Color color = image.color;
        color.a = 0;
        //��������� ��������
        image.color = color;
    }
}
