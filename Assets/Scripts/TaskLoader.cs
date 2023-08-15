using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UnitAndPlaceInfo{
    public int id_;
    public int x_;
    public int y_;
}

public class TaskInfo{
    public int id_;
    public string name_;
    public int map_id_;
    public List<string> faction_list_;
    public List<List<UnitAndPlaceInfo>> unit_list_;
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
        task.map_.faction_units_ = new Dictionary<string, List<GameObject>>();
        int guid = 0;
        for(int i = 0;i != task_info.faction_list_.Count; ++i){
            string faction_name = task_info.faction_list_[i];
            task.map_.faction_units_.Add(faction_name, new List<GameObject>());
            for(int j = 0;j != task_info.unit_list_[i].Count; ++j){
                GameObject unit_prefab = UnitPrefabPool.GetInstance().GetUnitPrefab(task_info.unit_list_[i][j].id_);
                int x = task_info.unit_list_[i][j].x_;
                int y = task_info.unit_list_[i][j].y_;
                GameObject unit_prefab_copy = UnitInstantiate.Generate(unit_prefab, task.map_.GetTile(x, y).transform);
                Unit unit = unit_prefab_copy.GetComponent<Unit>();
                unit.guid_ = guid++;
                unit.x_ = x;
                unit.y_ = y;
                Tile tile = task.map_.GetTile(x, y).GetComponent<Tile>();
                unit.tile_ = tile;
                tile.unit_ = unit;
                task.map_.faction_units_[faction_name].Add(unit_prefab_copy);
            }            
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
