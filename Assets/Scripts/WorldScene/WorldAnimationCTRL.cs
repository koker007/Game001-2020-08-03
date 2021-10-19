using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAnimationCTRL : MonoBehaviour
{

    [SerializeField]
    float timeAnimationPlay = 0;
    [SerializeField]
    Animation animation;

    [SerializeField]
    GameObject[] models;

    [SerializeField]
    float[] veroatnost;


    //��������������� ����������� ������
    public void RandomModel() {
        //������� ��� ������
        foreach (GameObject model in models) {
            model.SetActive(false);
        }

        for (int numTest = 0; numTest < 10; numTest++) {
            //�������� ������ ���� ����� �� �������
            if (SelectModel()) {
                break;
            }
        }
        

        bool SelectModel() {
            bool selected = false;

            //�������� �������� ������
            int modelNum = Random.Range(0, models.Length);
            float veroatnostNow = veroatnost[modelNum];

            //��������� ������� ���� ���������
            if (veroatnostNow < Random.Range(0, 100)) {
                models[modelNum].SetActive(true);
                selected = true;
            }

            return selected;
        }
    }


    private void OnEnable()
    {
        AnimationStart();
        RandomModel();
    }

    void AnimationStart() {
        foreach (AnimationState state in animation) {
            state.time = timeAnimationPlay;
        }

        animation.Play();
    }

    public void AnimationRestart() {
        foreach (AnimationState state in animation)
        {
            state.time = 0;
        }

        animation.Play();
    }
}
