using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//александр
//Семен
/// <summary>
/// контролирует анимации обьектов
/// </summary>
public class AnimatorCTRL : MonoBehaviour
{
    private Animator animator;
    public string parameterNameInt = "parameter";

    [System.Serializable]
    public class Animations
    {
        public int num;
        public string key;
    }

    public Animations[] animations = new Animations[1];

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string key)
    {
        if (animator == null)
            return;

        for (int i = 0; i < animations.Length; i++)
        {
            if (key == animations[i].key)
            {
                animator.SetInteger(parameterNameInt, animations[i].num);
            }
        }
    }

    public void SetFloat(string name, float value) {
        animator.SetFloat(name, value);
    }

    public void StopAnimations()
    {
        animator.SetInteger(parameterNameInt, 0);
    }
}
