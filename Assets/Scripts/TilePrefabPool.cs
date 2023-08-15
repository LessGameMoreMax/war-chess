using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using LitJson;

public class TilePrefabInfo{
    public int id_;
    public string prefab_name_;
    public string script_name_;
    public string name_;
    public int terrain_property_;
    public int[] block_property_;
}

public class TilePrefabPool: Singleton<TilePrefabPool>{
    private List<GameObject> tile_prefab_list_;

    public void LoadTilePrefabInfoFromFile(string file_name){
        string tile_prefab_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        List<TilePrefabInfo> tile_prefab_info_list = JsonMapper.ToObject<List<TilePrefabInfo>>(tile_prefab_config);
        CreateTilePrefabFromInfo(tile_prefab_info_list);
    }

    private void CreateTilePrefabFromInfo(List<TilePrefabInfo> tile_prefab_info_list){
        tile_prefab_list_ = new List<GameObject>();
        for(int i = 0;i != tile_prefab_info_list.Count; ++i){
            TilePrefabInfo tile_prefab_info = tile_prefab_info_list[i];
            GameObject tile_prefab = Resources.Load<GameObject>("Prefab/" + tile_prefab_info.prefab_name_);
            Type type = Type.GetType(tile_prefab_info.script_name_);
            Tile tile = tile_prefab.GetComponent(type) as Tile;
            if(tile == null){
                tile_prefab.AddComponent(type);
                tile = tile_prefab.GetComponent(type) as Tile;
            }
            AttachTileInfoToTile(tile_prefab_info, tile);
            StartTile(tile);
            tile_prefab_list_.Add(tile_prefab);
        }
    }

    private void AttachTileInfoToTile(TilePrefabInfo tile_prefab_info, Tile tile){
        tile.id_ = tile_prefab_info.id_;
        tile.name_ = tile_prefab_info.name_;
        tile.terrain_property_ = tile_prefab_info.terrain_property_;
        tile.block_property_ = new int[4];
        for(int i = 0; i != 4; ++i){
            tile.block_property_[i] = tile_prefab_info.block_property_[i];
        }
    }

    private void StartTile(Tile tile){
        tile.unit_ = null;
        tile.g_ = 0;
        tile.f_ = 0;
        tile.parent_ = null;
    }

    public GameObject GetTilePrefab(int index){
        return tile_prefab_list_[index];
    }

    public int GetTilePrefabCount(){
        return tile_prefab_list_.Count;
    }

}
