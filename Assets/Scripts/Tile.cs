using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    //Load from json
    public int id_;
    public string name_;
    public int terrain_property_;
    public int[] block_property_;

    //Load from map manager
    public int guid_;
    public Unit unit_;
    public int x_;
    public int y_;

    //Use for A star
    public int g_;
    public int f_;
    public GameObject parent_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual int GetBlockProperty(int unit_type){
        return block_property_[unit_type];
    }
}
