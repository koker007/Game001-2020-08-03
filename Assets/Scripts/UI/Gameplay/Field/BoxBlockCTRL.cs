using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxBlockCTRL : MonoBehaviour
{

    [SerializeField]
    Texture[] Textures;
    [SerializeField]
    Image image;

    CellCTRL myCell;

    int healthOld = 0;
    void Inicialize(CellCTRL cellNew) {
        myCell = cellNew;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void testImage() {
        if (image) {
        
        }
    }
}
