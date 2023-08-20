using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTroopCarriter : Unit
{
    Unit carried_unit_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CanLoad(Unit unit){
        return carried_unit_ == null && IsFriend(unit) && unit.id_ == 0;
    }

    public override void Load(Unit unit){
        unit.Hide();
        unit.tile_.unit_ = null;
        unit.tile_ = null;
        carried_unit_ = unit;
    }

    public override void Die(){
        if(carried_unit_ != null){
            Task.GetInstance().RemoveUnit(carried_unit_.guid_, carried_unit_.character_id_);
            Destroy(carried_unit_.health_ui_.gameObject);
            Destroy(carried_unit_.gameObject);
        }
        base.Die();
    }

    public override bool HaveUnload(){
        if(carried_unit_ == null) return false;
        GameObject temp = Task.GetInstance().map_.GetTile(x_ - 1, y_);
        if(CanUnload(temp)) return true;

        temp = Task.GetInstance().map_.GetTile(x_ + 1, y_);
        if(CanUnload(temp)) return true;

        temp = Task.GetInstance().map_.GetTile(x_, y_ - 1);
        if(CanUnload(temp)) return true;

        temp = Task.GetInstance().map_.GetTile(x_, y_ + 1);
        if(CanUnload(temp)) return true;

        return false;
    }
    public override bool CanUnload(GameObject temp){
        Tile tile = temp.GetComponent<Tile>();
        if(Mathf.Abs(tile.x_ - x_) + Mathf.Abs(tile.y_ - y_) > 1) return false;
        return tile != null && carried_unit_.CanMove(tile.terrain_property_) && tile.unit_ == null;
    }

    public override void UnloadToTile(GameObject temp){
        Tile tile = temp.GetComponent<Tile>();
        tile.unit_ = carried_unit_;
        carried_unit_.tile_ = tile;
        carried_unit_.x_ = tile.x_;
        carried_unit_.y_ = tile.y_;
        carried_unit_.transform.position = new Vector3(tile.transform.position.x,
            tile.transform.position.y + 1, tile.transform.position.z);
        carried_unit_.CancelHide();
        carried_unit_.SetBide();
        carried_unit_ = null;
    }
}
