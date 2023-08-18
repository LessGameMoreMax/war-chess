using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Tile
{
    public int character_id_;
    public Material material_;
    public Color neutral_color_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void InitializeBuilding(){
        material_ = GetComponent<MeshRenderer>().material;
        neutral_color_ = Color.black;
        SetNeutralColor();
    }

    public virtual void AttachToCharacter(int character_id){
        character_id_ = character_id;
        Task task = Task.GetInstance();
        if(character_id == -1){
            task.neutral_building_set_.Add(guid_);
            SetNeutralColor();
        }else{
            Character character = task.GetCharacter(character_id);
            character.AddBuilding(guid_);
            SetColor(character.faction_.color_);
        }
    }

    public virtual void DeleteFromCharacter(int character_id){
        if(character_id == -1){
            Task.GetInstance().neutral_building_set_.Remove(guid_);
        }else{
            Character character = Task.GetInstance().GetCharacter(character_id);
            character.RemoveBuilding(guid_);
        }
        SetNeutralColor();
    }

    public virtual void SetNeutralColor(){
        material_.color = neutral_color_;
    }

    public virtual void SetColor(Color color){
        material_.color = color;
    }
}
