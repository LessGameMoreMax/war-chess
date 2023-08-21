using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentStateEnum{
    Idle,
    Selected,
    Moving,
    Moved,
    ShowAttackRange,
    Attack,
    Load,
    Unload
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
            case CurrentStateEnum.ShowAttackRange:
                MouseShowAttackRangeDetect();
                MouseShowAttackRangeInput();
                break;
            case CurrentStateEnum.Attack:
                MouseAttackDetect();
                MouseAttackInput();
                break;
            case CurrentStateEnum.Load:
                MouseLoadDetect();
                MouseLoadInput();
                break;
            case CurrentStateEnum.Unload:
                MouseUnloadDetect();
                MouseUnloadInput();
                break;
            default:
                break;
        }

    }

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
            Unit unit = current_gameObject_.GetComponent<Unit>();
            if(unit != null){
                unit.SearchMove();
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
            if(unit.HavePath(current_gameObject_) && Task.GetInstance().CurrentCharacterId() == unit.character_id_  && unit.IsActive()){
                current_state_ = CurrentStateEnum.Moving;
                unit.MoveTo();
                return;
            }
            if(current_gameObject_ == select_gameObject_){
                unit.ClearPath();
                unit.ClearMove();
                unit.ShowAttackRange();
                current_state_ = CurrentStateEnum.ShowAttackRange;
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
                unit.HideActionUI();
                unit.Restore();
                unit.SearchMove();
                current_state_ = CurrentStateEnum.Selected;
                return;
            }
        }

    }

    private void MouseShowAttackRangeDetect(){

    }

    private void MouseShowAttackRangeInput(){
        if(Input.GetMouseButtonDown(1)){
            Unit unit = select_gameObject_.GetComponent<Unit>();
            if(unit != null){
                unit.ClearAttackRange();
                unit.SearchMove();
                current_state_ = CurrentStateEnum.Selected;
                return;
            }
        }
    }

    private void MouseAttackDetect(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if(Physics.Raycast(mouse_ray, out hit_info)){
            GameObject temp_gameObject = hit_info.transform.gameObject;
            Unit attack_unit = temp_gameObject.GetComponent<Unit>();
            if(current_gameObject_ != null && current_gameObject_ != temp_gameObject){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
                current_gameObject_ = null;
            }
            if(attack_unit == null) return;
            if(unit.HaveAttackUnit(attack_unit)){
                current_gameObject_ = temp_gameObject;
                current_gameObject_.GetComponent<ContourColor>().ShowPreSelectColor();
            }
            
        }else if(current_gameObject_ != null){
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_ = null;
        }
    }

    private void MouseAttackInput(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        if(Input.GetMouseButtonDown(1)){
            if(unit != null){
                unit.ShowActionUI();
                unit.ClearRealAttackRange();
                current_state_ = CurrentStateEnum.Moved;
            }
        }
        if(Input.GetMouseButtonDown(0) && current_gameObject_ != null){
            Unit enemy = current_gameObject_.GetComponent<Unit>();
            unit.Attack(enemy);
            if(!unit.HasDead() && !enemy.HasDead()) enemy.Attack(unit);
            unit.ClearRealAttackRange();
            select_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            if(unit.HasDead()) unit.Die();
            else unit.SetBide();
            if(enemy.HasDead()) enemy.Die();
            select_gameObject_ = null;
            current_gameObject_ = null;
            current_state_ = CurrentStateEnum.Idle;
        }
    }

    private void MouseLoadDetect(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if(Physics.Raycast(mouse_ray, out hit_info)){
            GameObject temp_gameObject = hit_info.transform.gameObject;
            Unit load_unit = temp_gameObject.GetComponent<Unit>();
            if(current_gameObject_ != null && current_gameObject_ != temp_gameObject){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
                current_gameObject_ = null;
            }
            if(load_unit == null) return;
            if(unit.HaveLoadUnit(load_unit)){
                current_gameObject_ = temp_gameObject;
                current_gameObject_.GetComponent<ContourColor>().ShowPreSelectColor();
            }
        }else if(current_gameObject_ != null){
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_ = null;
        }
    }

    private void MouseLoadInput(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        if(Input.GetMouseButtonDown(1)){
            if(unit != null){
                unit.ShowActionUI();
                current_state_ = CurrentStateEnum.Moved;
            }
        }
        if(Input.GetMouseButtonDown(0) && current_gameObject_ != null){
            select_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            Unit loader = current_gameObject_.GetComponent<Unit>();
            loader.Load(unit);
            select_gameObject_ = null;
            current_gameObject_ = null;
            current_state_ = CurrentStateEnum.Idle;
        }
    }

    private void MouseUnloadDetect(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;
        if(Physics.Raycast(mouse_ray, out hit_info)){
            GameObject temp_gameObject = hit_info.transform.gameObject;
            Tile tile = temp_gameObject.GetComponent<Tile>();
            if(current_gameObject_ != null && current_gameObject_ != temp_gameObject){
                current_gameObject_.GetComponent<ContourColor>().CancleColor();
                current_gameObject_ = null;
            }
            if(tile == null) return;
            if(unit.CanUnload(temp_gameObject)){
                current_gameObject_ = temp_gameObject;
                current_gameObject_.GetComponent<ContourColor>().ShowPreSelectColor();
            }
        }else if(current_gameObject_ != null){
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_ = null;
        }
    }

    private void MouseUnloadInput(){
        Unit unit = select_gameObject_.GetComponent<Unit>();
        if(Input.GetMouseButtonDown(1)){
            if(unit != null){
                unit.ShowActionUI();
                current_state_ = CurrentStateEnum.Moved;
            }
        }
        if(Input.GetMouseButtonDown(0) && current_gameObject_ != null){
            select_gameObject_.GetComponent<ContourColor>().CancleColor();
            current_gameObject_.GetComponent<ContourColor>().CancleColor();
            unit.UnloadToTile(current_gameObject_);
            unit.SetBide();
            select_gameObject_ = null;
            current_gameObject_ = null;
            current_state_ = CurrentStateEnum.Idle;
        }
    }
}
