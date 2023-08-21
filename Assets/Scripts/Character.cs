using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可以成为拥有各种不同特性或技能的角色基类
public class Character
{
    public int id_;
    public string name_;
    public Faction faction_;
    public int cash_;
    public HashSet<Unit> unit_set_;
    public HashSet<Building> building_set_;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(Unit unit){
        unit_set_.Add(unit);
    }

    public void RemoveUnit(Unit unit){
        unit_set_.Remove(unit);
    }

    public void AddBuilding(Building building){
        building_set_.Add(building);
    }

    public void RemoveBuilding(Building building){
        building_set_.Remove(building);
    }

    public virtual void InitializeTurn(){
        InitializeUnit();
        InitializeBuilding();
    }

    protected virtual void InitializeUnit(){
        foreach(Unit unit in unit_set_){
            unit.InitializeTurn();
        }
    }

    protected virtual void InitializeBuilding(){
        foreach(Building building in building_set_){
            cash_ += building.GetCash();
            Unit unit = building.unit_;
            if(unit != null && unit.character_id_ == id_){
                unit.Healed((int)(unit.max_health_ * 0.2));
            }
                
        }

    }

    public virtual void FinishTurn(){
        FinishUnit();
        FinishBuilding();
    }

    protected virtual void FinishUnit(){
        foreach(Unit unit in unit_set_){
            unit.FinishTurn();
        }
    }

    protected virtual void FinishBuilding(){

    }

}
