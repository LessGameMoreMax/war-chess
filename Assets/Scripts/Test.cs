using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIPool.CreateInstance();
        UIPool.GetInstance().InitializeUIPool();
        TilePrefabPool.CreateInstance();
        TilePrefabPool.GetInstance().LoadTilePrefabInfoFromFile("tile");
        MapPool.CreateInstance();
        MapPool.GetInstance().LoadMapFromFile("map");
        UnitPrefabPool.CreateInstance();
        UnitPrefabPool.GetInstance().LoadUnitPrefabInfoFromFile("unit");
        FactionLoader.CreateInstance();
        FactionLoader.GetInstance().LoadFactionFromFile("faction");
        CharacterLoader.CreateInstance();
        CharacterLoader.GetInstance().LoadCharacterFromFile("character");
        TaskLoader.CreateInstance();
        TaskLoader.GetInstance().LoadTaskFromFile("task");
        TaskLoader.GetInstance().GetTask(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
