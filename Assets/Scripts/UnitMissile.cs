using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMissile : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool HaveAttack(){
        return base.HaveAttack() && tile_ == restore_tile_;
    }

    public override void ShowAttackRange(){
        FindAttackRange(tile_.gameObject, false);
        foreach(GameObject temp in attack_tiles_set_)
            temp.GetComponent<TileColor>().ShowAttackColor();
    }
}
