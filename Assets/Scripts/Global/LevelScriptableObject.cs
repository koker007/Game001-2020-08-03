using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObject/level", order = 1)]
public class LevelScriptableObject : ScriptableObject
{
    public List<LevelsScript.Level> levels = new List<LevelsScript.Level>();
}
