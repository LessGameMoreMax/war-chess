using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using LitJson;

public class UnitPrefabInfo{
    public int id_;
    public string name_;
    public string prefab_name_;
    public string script_name_;
    public int position_property_;
    public int[] attack_position_property_;
    public int move_property_;
    public int view_property_;
    public int[] attack_scope_property_;
    public int[] move_terrain_property_;
    public int unit_type_;
    public int max_health_;
}

public class UnitPrefabPool : Singleton<UnitPrefabPool>
{
    private List<GameObject> unit_prefab_list_;

    public void LoadUnitPrefabInfoFromFile(string file_name){
        string unit_prefab_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        List<UnitPrefabInfo> unit_prefab_info_list = JsonMapper.ToObject<List<UnitPrefabInfo>>(unit_prefab_config);
        CreateUnitPrefabFromInfo(unit_prefab_info_list);
    }

    private void CreateUnitPrefabFromInfo(List<UnitPrefabInfo> unit_prefab_info_list){
        unit_prefab_list_ = new List<GameObject>();
        for(int i = 0;i != unit_prefab_info_list.Count; ++i){
            UnitPrefabInfo unit_prefab_info = unit_prefab_info_list[i];
            GameObject unit_prefab = Resources.Load<GameObject>("Prefab/" + unit_prefab_info.prefab_name_);
            Type type = Type.GetType(unit_prefab_info.script_name_);
            Unit unit = unit_prefab.GetComponent(type) as Unit;
            if(unit == null){
                unit_prefab.AddComponent(type);
                unit = unit_prefab.GetComponent(type) as Unit;
            }
            AttachUnitPrefabInfoToUnit(unit_prefab_info, unit);
            unit_prefab_list_.Add(unit_prefab);
        }
    }

    private void AttachUnitPrefabInfoToUnit(UnitPrefabInfo unit_prefab_info, Unit unit){
        unit.id_ = unit_prefab_info.id_;
        unit.name_ = unit_prefab_info.name_;
        unit.position_property_ = unit_prefab_info.position_property_;
        unit.attack_position_property_ = new int[5];
        for(int i = 0;i != 5; ++i)
            unit.attack_position_property_[i] = unit_prefab_info.attack_position_property_[i];
        unit.move_property_ = unit_prefab_info.move_property_;
        unit.view_property_ = unit_prefab_info.view_property_;
        unit.attack_scope_property_ = new int[2];
        unit.attack_scope_property_[0] = unit_prefab_info.attack_scope_property_[0];
        unit.attack_scope_property_[1] = unit_prefab_info.attack_scope_property_[1];
        unit.move_terrain_property_ = new int[6];
        for(int i = 0;i != 6; ++i)
            unit.move_terrain_property_[i] = unit_prefab_info.move_terrain_property_[i];
        unit.unit_type_ = unit_prefab_info.unit_type_;
        unit.max_health_ = unit_prefab_info.max_health_;
        unit.health_ = unit_prefab_info.max_health_;
        unit.tile_ = null;
    }

    public GameObject GetUnitPrefab(int index){
        return unit_prefab_list_[index];
    }

    public int GetUnitPrefabCount(){
        return unit_prefab_list_.Count;
    }
}
