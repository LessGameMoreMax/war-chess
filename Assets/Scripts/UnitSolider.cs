using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSolider : Unit
{
    
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
}
