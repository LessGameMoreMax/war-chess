using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSolider : Unit
{
    HashSet<GameObject> troop_tiles_;
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

    public override bool SearchMoveCondition(Tile tile, HashSet<GameObject> move_tiles_set){
        if(base.SearchMoveCondition(tile, move_tiles_set)){
            Unit unit = tile.unit_;
            if(unit != null && unit.CanLoad(this)){
                if(!move_tiles_set.Contains(tile.gameObject)){
                    tile.gameObject.GetComponent<TileColor>().ShowMoveColor();
                    move_tiles_set.Add(tile.gameObject);
                    if(!troop_tiles_.Contains(tile.gameObject)) troop_tiles_.Add(tile.gameObject);
                }
            }
            return true;
        }
        return false;
    }

    public override bool HavePath(GameObject dist){
        return base.HavePath(dist) && !troop_tiles_.Contains(dist);
    }

    public override void SearchMove(){
        if(troop_tiles_ == null) troop_tiles_ = new HashSet<GameObject>();
        else troop_tiles_.Clear();
        base.SearchMove();
    }

    public override bool HaveLoad(){
        if(troop_tiles_.Contains(Task.GetInstance().map_.GetTile(x_ - 1, y_))) return true;
        if(troop_tiles_.Contains(Task.GetInstance().map_.GetTile(x_ + 1, y_))) return true;
        if(troop_tiles_.Contains(Task.GetInstance().map_.GetTile(x_, y_ - 1))) return true;
        if(troop_tiles_.Contains(Task.GetInstance().map_.GetTile(x_, y_ + 1))) return true;
        return false;
    }

    public override bool HaveLoadUnit(Unit other){
        return troop_tiles_.Contains(other.tile_.gameObject);
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
