using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentStateEnum{
    Idle,
    Selected,
    Moving,
    Moved
}

public class SelectManager : MonoBehaviour
{
    private static SelectManager instance_;
    public static SelectManager GetInstance(){
        return instance_;
    }

    public CurrentStateEnum current_state_;
    public GameObject current_gameObject_;
    public GameObject select_gameObject_;
    // Start is called before the first frame update
    void Start()
    {
        instance_ = this;
        current_state_ = CurrentStateEnum.Idle;
        current_gameObject_ = null;
        select_gameObject_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_state_){
            case CurrentStateEnum.Idle:
                MouseIdleDetect();
                MouseIdleInput();
                break;
            case CurrentStateEnum.Selected:
                MouseSelectedDetect();
                MouseSelectedInput();
                break;
            case CurrentStateEnum.Moving:
                MouseMovingDetect();
                MouseMovingInput();
                break;
            case CurrentStateEnum.Moved:
                MouseMovedDetect();
                MouseMovedInput();
                break;
            default:
                break;
        }

    }

    // every state MouseDetect!
    private void MouseIdleDetect(){
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if(Physics.Raycast(mouse_ray, out hit_info)){
            GameObject temp_gameObject = hit_info.transform.gameObject;
            if(current_gameObject_ != null && current_gameObject_ != temp_gameObject){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
            }
            current_gameObject_ = temp_gameObject;
            current_gameObject_.GetComponent<ContourColor>().ShowPreSelectColor();
        }else if(current_gameObject_ != null){
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_ = null;
        }
    }

    private void MouseIdleInput(){
        if(Input.GetMouseButtonDown(0) && current_gameObject_ != null){
            if(current_gameObject_.GetComponent<Unit>() != null){
                current_gameObject_.GetComponent<Unit>().SearchMove();
                current_gameObject_.GetComponent<ContourColor>().ShowSelectColor();
                select_gameObject_ = current_gameObject_;
                current_state_ = CurrentStateEnum.Selected;
            }
        }
    }

    private void MouseSelectedDetect(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if(Physics.Raycast(mouse_ray, out hit_info)){
            GameObject temp_gameObject = hit_info.transform.gameObject;
            if(current_gameObject_ != null && current_gameObject_ != temp_gameObject && current_gameObject_ != select_gameObject_){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
                unit.ClearPath();
            }
            current_gameObject_ = temp_gameObject;
            if(current_gameObject_ != select_gameObject_){
                current_gameObject_.GetComponent<ContourColor>().ShowPreSelectColor();
                if(unit.HavePath(current_gameObject_)){
                    unit.SearchPath(current_gameObject_);
                }
            }
        }else if(current_gameObject_ != null){
            if(current_gameObject_ != select_gameObject_){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
                unit.ClearPath();
            }
            current_gameObject_ = null;
        }
    }

    private void MouseSelectedInput(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        if(Input.GetMouseButtonDown(1)){
            unit.ClearPath();
            unit.ClearMove();
            select_gameObject_.GetComponent<ContourColor>().CancleColor();
            select_gameObject_ = null;
            current_state_ = CurrentStateEnum.Idle;
        }
        if(Input.GetMouseButtonDown(0) && current_gameObject_ != null){
            if(unit.HavePath(current_gameObject_)){
                current_state_ = CurrentStateEnum.Moving;
                unit.MoveTo();
                return;
            }
        }
    }

    private void MouseMovingDetect(){

    }

    private void MouseMovingInput(){

    }

    private void MouseMovedDetect(){

    }

    private void MouseMovedInput(){
        if(Input.GetMouseButtonDown(1)){
            Unit unit = select_gameObject_.GetComponent<Unit>();
            if(unit != null){
                unit.Restore();
                select_gameObject_.GetComponent<Unit>().SearchMove();
                current_state_ = CurrentStateEnum.Selected;
                return;
            }
        }

    }
}
