using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDeleteProfile : MonoBehaviour
{

    [SerializeField]
    AnimatorCTRL animatorButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(string AnumationName) {
        animatorButton.PlayAnimation(AnumationName);
    }


    public void DeleteProfile() {
        PlayerProfile.main.DeleteProfile();
    }
}
