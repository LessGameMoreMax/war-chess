using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSolider : Unit
{
    Building is_occupied_building_;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack(Unit unit){
        unit.health_ -= 60;
        base.Attack(unit);
    }

    public override bool HaveLoad(){
        Map map = Task.GetInstance().map_;

        GameObject temp = map.GetTile(x_ - 1, y_);
        Tile tile = null;
        if(temp != null){
            tile = temp.GetComponent<Tile>();
            if(tile.unit_ != null && tile.unit_.CanLoad(this)) return true;
        }

        temp = map.GetTile(x_ + 1, y_);
        if(temp != null){
            tile = temp.GetComponent<Tile>();
            if(tile.unit_ != null && tile.unit_.CanLoad(this)) return true;
        }
        

        temp = map.GetTile(x_, y_ - 1);
        if(temp != null){
            tile = temp.GetComponent<Tile>();
            if(tile.unit_ != null && tile.unit_.CanLoad(this)) return true;
        }

        temp = map.GetTile(x_, y_ + 1);
        if(temp != null){
            tile = temp.GetComponent<Tile>();
            if(tile.unit_ != null && tile.unit_.CanLoad(this)) return true;
        }
        return false;
    }

    public override bool IsOccupiedState(){
        return is_occupied_building_ != null;
    }

    public override bool HaveOccupy(){
        Building building = tile_.GetComponent<Building>();
        return building != null && !IsFriendBuilding(building);
    }

    public override void Occupy(){
        Building building = tile_.gameObject.GetComponent<Building>();
        is_occupied_building_ = building;
        building.SubtractHealth(health_);
        if(building.HasDead()){
            building.DeleteFromCharacter();
            building.AttachToCharacter(character_id_);
            building.SetMaxHealth();
            is_occupied_building_ = null;
        }
    }

    public override void CancleOccupy(){
        if(is_occupied_building_ == null) return;
        is_occupied_building_.SetMaxHealth();
        is_occupied_building_ = null;
    }

    public override void Hide(){
        CancleOccupy();
        base.Hide();
    }

    public override void Die(){
        CancleOccupy();
        base.Die();
    }

    public override void SetBide(){
        if(is_occupied_building_ != null && tile_.gameObject != is_occupied_building_.gameObject)
            CancleOccupy();
        base.SetBide();
    }


}
