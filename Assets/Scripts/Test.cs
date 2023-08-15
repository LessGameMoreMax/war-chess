using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // GameObject tile_;
    // Map map_;
    // GameObject unit_;
    // Start is called before the first frame update
    void Start()
    {
        // TilePrefabPool.CreateInstance();
        // TilePrefabPool.GetInstance().LoadTilePrefabInfoFromFile("tile");
        // tile_ = Instantiate(TilePrefabPool.GetInstance().GetTilePrefab(2), transform);
        // Tile script = tile_.GetComponent<Tile>();
        // Debug.Log(script.id_);
        // Debug.Log(script.name_);
        // Debug.Log(script.position_property_);
        // Debug.Log(script.block_property_);

        // MapPool.CreateInstance();
        // MapPool.GetInstance().LoadMapFromFile("map");
        // map_ = MapPool.GetInstance().GetMap(0);
        // UnitPrefabPool.CreateInstance();
        // UnitPrefabPool.GetInstance().LoadUnitPrefabInfoFromFile("unit");
        // unit_ = Instantiate(UnitPrefabPool.GetInstance().GetUnitPrefab(1), transform);
        TilePrefabPool.CreateInstance();
        TilePrefabPool.GetInstance().LoadTilePrefabInfoFromFile("tile");
        MapPool.CreateInstance();
        MapPool.GetInstance().LoadMapFromFile("map");
        UnitPrefabPool.CreateInstance();
        UnitPrefabPool.GetInstance().LoadUnitPrefabInfoFromFile("unit");
        TaskLoader.CreateInstance();
        TaskLoader.GetInstance().LoadTaskFromFile("task");
        TaskLoader.GetInstance().GetTask(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
