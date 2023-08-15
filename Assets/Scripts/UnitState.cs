using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UnitStateEnum{
    Active,
    Bide
}

public class UnitState : MonoBehaviour
{
    Material material_;
    public UnitStateEnum current_unit_state_;
    public Color active_state_color_;
    public Color bide_state_color_;

    // Start is called before the first frame update
    void Start()
    {
        current_unit_state_ = UnitStateEnum.Active;
        material_ = GetComponent<MeshRenderer>().material;
        active_state_color_ = material_.color;
        bide_state_color_ = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUnitStateActive(){
        current_unit_state_ = UnitStateEnum.Active;
        material_.color = active_state_color_;
    }

    public void SetUnitStateBide(){
        current_unit_state_ = UnitStateEnum.Bide;
        material_.color = bide_state_color_;
    }
}
