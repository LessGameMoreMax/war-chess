using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task: Singleton<Task>
{
    public int id_;
    public string name_;
    public Map map_;
    public Dictionary<int, Character> id_character_dic_;
    public List<int> character_id_list_;
    public int current_character_index_;
    public Dictionary<int, HashSet<int>> character_friend_set_dic_;
    public HashSet<int> player_character_set_;
    public HashSet<Building> neutral_building_set_;

    public TurnUI turn_ui_;

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

    public void RemoveUnit(Unit unit, int character_id){
        Character character = GetCharacter(character_id);
        character.RemoveUnit(unit);
    }

    public void NextTurn(){
        if(SelectManager.GetInstance().current_state_ != CurrentStateEnum.Idle) return;
        Character current_character = GetCurrentCharacter();
        current_character.FinishTurn();
        current_character_index_ = (current_character_index_ + 1) % character_id_list_.Count;
        Initialize();
    }

    public int CurrentCharacterId(){
        return character_id_list_[current_character_index_];
    }

    public Character GetCurrentCharacter(){
        return id_character_dic_[CurrentCharacterId()];
    }

    public void SetUI(){
        Character current_character = GetCurrentCharacter();
        if(turn_ui_ == null)
            turn_ui_ = GameObject.Find("Canvas").GetComponent<TurnUI>();
        turn_ui_.SetMessageText(current_character.name_, current_character.cash_);
    }

    public void Initialize(){
        Character current_character = GetCurrentCharacter();
        current_character.InitializeTurn();
        SetUI();
    }
}
