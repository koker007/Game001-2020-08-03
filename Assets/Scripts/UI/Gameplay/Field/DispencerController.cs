using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispencerController : MonoBehaviour
{
    private float primaryObjectSpawnChance = 30;        //Шанс выпадения основного предмета
    public CellCTRL myCell;                        //Клетка с раздатчиком
    public CellCTRL targetCell;                    //Клетка под раздатчиком

    private void Update()
    {
        SpawnInternal();
    }

    //Cнизу пусто + не камень + не стенка ==> спавним объект
    private void SpawnInternal()
    {
        if (GameFieldCTRL.main.CheckWallsToMoveDown(targetCell, myCell) && targetCell.cellInternal == null)
        {
            int currentChance = Random.Range(1, 101);
            //if (primaryObjectSpawnChance >= currentChance) //Проверяем шанс
            //{
            //GameObject internalObj = Instantiate(GameFieldCTRL.main.prefabInternal, GameFieldCTRL.main.parentOfInternals);
            Debug.Log("SPAWN!");
            //}
        }      
    } 
}
