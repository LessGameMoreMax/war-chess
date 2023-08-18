using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class CharacterInfo{
    public int id_;
    public string name_;
    public int faction_id_;
}

public class CharacterLoader : Singleton<CharacterLoader>
{
    private List<CharacterInfo> character_info_list_;

    public void LoadCharacterFromFile(string file_name){
        string character_config = Resources.Load<TextAsset>("Config/" + file_name).text;
        character_info_list_ = JsonMapper.ToObject<List<CharacterInfo>>(character_config);
    }

    public Character GetCharacter(int index){
        if(index < 0 || index >= character_info_list_.Count) return null;
        CharacterInfo character_info = character_info_list_[index];
        Character character = new Character();
        character.id_ = character_info.id_;
        character.name_ = character_info.name_;
        character.faction_ = FactionLoader.GetInstance().GetFaction(character_info.faction_id_);
        character.unit_set_ = new HashSet<int>();
        character.building_set_ = new HashSet<int>();
        return character;
    }
}
