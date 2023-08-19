using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UnitAndPlaceInfo{
    public int id_;
    public int x_;
    public int y_;
    public int character_id_;
}

public class BuildAndPlaceInfo{
    public int x_;
    public int y_;
    public int character_id_;
}

public class TaskInfo{
    public int id_;
    public string name_;
    public int map_id_;
    public List<List<int>> power_;
    public List<int> player_power_;
    public List<int> power_initial_money_;
    public List<UnitAndPlaceInfo> unit_list_;
    public List<BuildAndPlaceInfo> build_list_;
}

public class TaskLoader : Singleton<TaskLoader>
{
    private List<TaskInfo> task_info_list_;

    public void LoadTaskFromFile(string file_name){
        string task_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        task_info_list_ = JsonMapper.ToObject<List<TaskInfo>>(task_config);
    }

    public void GetTask(int index){
        if(index < 0 || index >= task_info_list_.Count) return;
        if(Task.GetInstance() != null){
            if(Task.GetInstance().id_ != index) Task.DeleteInstance();
            else return;
        }
        TaskInfo task_info = task_info_list_[index];
        Task task = Task.CreateInstance();
        task.id_ = task_info.id_;
        task.name_ = task_info.name_;
        task.map_ = MapPool.GetInstance().GetMap(task_info.map_id_);
        task.id_character_dic_ = new SortedDictionary<int, Character>();
        task.character_friend_set_dic_ = new Dictionary<int, HashSet<int>>();
        for(int i = 0;i != task_info.power_.Count; ++i){
            List<int> character_list = task_info.power_[i];
            HashSet<int> friend_set = new HashSet<int>();
            for(int j = 0;j != character_list.Count; ++j){
                int character_id = character_list[j];
                task.id_character_dic_.Add(character_id, CharacterLoader.GetInstance().GetCharacter(character_id));
                friend_set.Add(character_id);
            }
            foreach(int character_id in friend_set)
                task.character_friend_set_dic_.Add(character_id, friend_set);
        }
        task.character_id_list_ = new List<int>();
        foreach(int character_id in task.id_character_dic_.Keys){
            task.character_id_list_.Add(character_id);
        }
        task.current_character_ = task.character_id_list_[0];
        int count = 0;
        foreach(Character character in task.id_character_dic_.Values){
            character.money_ = task_info.power_initial_money_[count++];
        }
        task.player_character_set_ = new HashSet<int>();
        for(int i = 0;i != task_info.player_power_.Count; ++i)
            task.player_character_set_.Add(task_info.player_power_[i]);
        int guid = 0;
        for(int i = 0;i != task_info.unit_list_.Count; ++i){
            GameObject unit_prefab = UnitPrefabPool.GetInstance().GetUnitPrefab(task_info.unit_list_[i].id_);
            int x = task_info.unit_list_[i].x_;
            int y = task_info.unit_list_[i].y_;
            GameObject unit_prefab_copy = UnitInstantiate.Generate(unit_prefab, task.map_.GetTile(x, y).transform);
            Unit unit = unit_prefab_copy.GetComponent<Unit>();
            unit.guid_ = guid++;
            unit.x_ = x;
            unit.y_ = y;
            unit.attack_tiles_set_ = new HashSet<GameObject>();
            unit.attack_real_tiles_set_ = new HashSet<GameObject>();
            Tile tile = task.map_.GetTile(x, y).GetComponent<Tile>();
            unit.tile_ = tile;
            tile.unit_ = unit;
            int character_id = task_info.unit_list_[i].character_id_;
            unit.InitializeCharacter(character_id);
            unit.SetActive();
        }
        task.neutral_building_set_ = new HashSet<int>();
        guid = 0;
        for(int i = 0;i != task_info.build_list_.Count; ++i){
            BuildAndPlaceInfo build_and_place_info = task_info.build_list_[i];
            GameObject tile = task.map_.GetTile(build_and_place_info.x_, build_and_place_info.y_);
            Building building = tile.GetComponent<Building>();
            Debug.Assert(building != null, "TaskLoader.cs---building load error");
            building.InitializeBuilding();
            building.AttachToCharacter(build_and_place_info.character_id_);
        }
    }

    public int GetTaskCount(){
        return task_info_list_.Count;
    }
}

public class UnitInstantiate: MonoBehaviour{
    public static GameObject Generate(GameObject unit_prefab, Transform transform){
        return Instantiate(unit_prefab, new Vector3(transform.position.x,
            transform.position.y + 1.0f, transform.position.z), new Quaternion());
    }
}
