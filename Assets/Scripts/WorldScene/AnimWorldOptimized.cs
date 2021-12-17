using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������������ �������� ����� �� �������������� ������ ����� ���� ������ ��������� ����� �������
//������ ������ ���� ���������� �� ��� �� ������� ��� � �������� ���� ������� �������� ����
public class AnimWorldOptimized : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    //������ ��������
    float scale = 1;

    // Start is called before the first frame update
    void Start()
    {
        iniAnimator();
    }

    void iniAnimator() {
        if (animator) return;

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //������� ����� ��������� ����� ����� �������� ��������
        Invoke("TestAnimation", Random.Range(0, 0.25f));
    }

    void TestAnimation()
    {

        if (Vector3.Distance(MainCamera.main.animatePos, gameObject.transform.position) < MainCamera.animateDist * scale)
        {
            if (animator.enabled)
                return;

            animator.enabled = true;
            animator.Rebind();
            animator.Update(0f);
        }
        else
        {
            if (!animator.enabled)
                return;

            animator.enabled = false;

        }
    }
}
