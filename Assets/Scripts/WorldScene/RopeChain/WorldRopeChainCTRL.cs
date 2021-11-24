using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Контроллер генерации дорожки соединяющий все уровни в одну линию
/// </summary>
public class WorldRopeChainCTRL : MonoBehaviour
{
    static public WorldRopeChainCTRL main;

    [SerializeField]
    public RopeChain PrefabRopeChain;
    [SerializeField]
    public GameObject PrefabSphere;

    // Start is called before the first frame update
    void Start()
    {
        //ссылаемся на самого себя
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
