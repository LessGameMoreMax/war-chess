using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class MapInfo{
    public int id_;
    public string name_;
    public int width_;
    public int height_;
    public double interval_;
    public List<TileInfo> tile_;
}

public class TileInfo{
    public int id_;
}

public class MapPool: Singleton<MapPool>
{
    private List<MapInfo> map_info_list_;

    public void LoadMapFromFile(string file_name){
        string map_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        map_info_list_ = JsonMapper.ToObject<List<MapInfo>>(map_config);
    }

    public Map GetMap(int index){
        if(index < 0 || index >= map_info_list_.Count){
            return null;
        }
        Map map = new Map();
        map.id_ = map_info_list_[index].id_;
        map.name_ = map_info_list_[index].name_;
        map.interval_ = (float)map_info_list_[index].interval_;
        map.width_ = map_info_list_[index].width_;
        map.height_ = map_info_list_[index].height_;
        map.tiles_ = new List<GameObject>();
        AttachTileToMap(map_info_list_[index].tile_, map);
        return map;
    }

    public int GetMapCount(){
        return map_info_list_.Count;
    }

    private void AttachTileToMap(List<TileInfo> tile_info, Map map){
        int width = map.width_;
        float interval = map.interval_;
        List<GameObject> map_tiles = map.tiles_;

        for(int i = 0;i != tile_info.Count; ++i){
            int w_index = i % width;
            int h_index = i / width;
            Vector3 position = new Vector3(
                w_index * (1.0f + interval), 
                0,
                h_index * (1.0f + interval));
            GameObject tile_prefab = TileInstantiate.Generate(tile_info[i].id_, position);
            Tile tile = tile_prefab.GetComponent<Tile>();
            tile.guid_ = i;
            tile.x_ = w_index;
            tile.y_ = h_index;
            map_tiles.Add(tile_prefab);
        }
    }

}

public class TileInstantiate: MonoBehaviour{
    public static GameObject Generate(int id, Vector3 position){
        return Instantiate(TilePrefabPool.GetInstance().GetTilePrefab(id), position, new Quaternion());
    }
}
