using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSolider : Unit
{
    HashSet<GameObject> troop_tiles_;
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
}
