using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrystalObject : MonoBehaviour
{
    public GameObject parent;
    public void AnimationEnd()
    {
        Destroy(parent);
    }
}
