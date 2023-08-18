using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class FactionInfo{
    public int id_;
    public string name_;
    public string color_;
}

public class FactionLoader : Singleton<FactionLoader>
{
    private List<FactionInfo> faction_info_list_;
    
    public void LoadFactionFromFile(string file_name){
        string faction_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        faction_info_list_ = JsonMapper.ToObject<List<FactionInfo>>(faction_config);  
    }

    public Faction GetFaction(int index){
        if(index < 0 || index >= faction_info_list_.Count) return null;
        FactionInfo faction_info = faction_info_list_[index];
        Faction faction = new Faction();
        faction.id_ = faction_info.id_;
        faction.name_ = faction_info.name_;
        AddColorToFaction(faction, faction_info);
        return faction;
    }

    private void AddColorToFaction(Faction faction, FactionInfo info){
        if(info.color_ == "white") faction.color_ = Color.white;
        else if (info.color_ == "red") faction.color_ = Color.red;
        else if (info.color_ == "green") faction.color_ = Color.green;
        else if (info.color_ == "blue") faction.color_= Color.blue;
        else if (info.color_ == "yellow") faction.color_ = Color.yellow;
        else if (info.color_ == "gray") faction.color_ = Color.gray;
        else if (info.color_ == "cyan") faction.color_ = Color.cyan;
    }
}
