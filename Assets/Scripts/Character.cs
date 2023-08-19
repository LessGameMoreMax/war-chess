using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int id_;
    public string name_;
    public Faction faction_;
    public int money_;
    public HashSet<int> unit_set_;
    public HashSet<int> building_set_;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(int guid){
        unit_set_.Add(guid);
    }

    public void RemoveUnit(int guid){
        unit_set_.Remove(guid);
    }

    public void AddBuilding(int guid){
        building_set_.Add(guid);
    }

    public void RemoveBuilding(int guid){
        building_set_.Remove(guid);
    }

}
