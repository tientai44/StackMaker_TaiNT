using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : GOSingleton<LevelManager>
{
    static private int currentLevel=1;
    private List<int> levelList;
    public List<int> LevelList { get => levelList; set => levelList = value; }

    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }


}
