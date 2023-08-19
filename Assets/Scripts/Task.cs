using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task: Singleton<Task>
{
    public int id_;
    public string name_;
    public Map map_;
    public SortedDictionary<int, Character> id_character_dic_;
    public List<int> character_id_list_;
    public int current_character_;
    public Dictionary<int, HashSet<int>> character_friend_set_dic_;
    public HashSet<int> player_character_set_;
    public HashSet<int> neutral_building_set_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Character GetCharacter(int id){
        Debug.Assert(id_character_dic_.ContainsKey(id), " Task.cs---id_character_dic_ not contain character: " + id);
        return id_character_dic_[id];
    }

    public void RemoveUnit(int guid, int character_id){
        Character character = GetCharacter(character_id);
        character.RemoveUnit(guid);
    }
}
