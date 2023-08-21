using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    public int offset_x_;
    public int offset_y_;
    public bool is_show_;
    public Building building_;
    public Button solider_button_;
    public Button troop_button_;
    public Button missile_button_;
    public Button helicopter_button_;
    public Button tank_button_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(is_show_){
            Vector3 temp = transform.position - Camera.main.transform.position;
            temp.x = 0;
            transform.rotation = Quaternion.LookRotation(temp);
        }
    }

    public void Show(Building building){
        Vector3 target = building.transform.position + Vector3.up * offset_y_ + Vector3.right * offset_x_;
        transform.position = target;
        is_show_ = true;
        building_ = building;
        int current_cash = Task.GetInstance().GetCurrentCharacter().cash_;
        if(UnitPrefabPool.GetInstance().GetUnitPrefab(0).GetComponent<Unit>().cash_ <= current_cash) {
            solider_button_.interactable = true;
        }else {
            solider_button_.interactable = false;
        }

        if(UnitPrefabPool.GetInstance().GetUnitPrefab(1).GetComponent<Unit>().cash_ <= current_cash) {
            troop_button_.interactable = true;
        }else {
            troop_button_.interactable = false;
        }

        if(UnitPrefabPool.GetInstance().GetUnitPrefab(2).GetComponent<Unit>().cash_ <= current_cash) {
            missile_button_.interactable = true;
        }else {
            missile_button_.interactable = false;
        }

        if(UnitPrefabPool.GetInstance().GetUnitPrefab(3).GetComponent<Unit>().cash_ <= current_cash) {
            helicopter_button_.interactable = true;
        }else {
            helicopter_button_.interactable = false;
        }

        if(UnitPrefabPool.GetInstance().GetUnitPrefab(4).GetComponent<Unit>().cash_ <= current_cash) {
            tank_button_.interactable = true;
        }else {
            tank_button_.interactable = false;
        }
    }

    public void Hide(){
        is_show_ = false;
    }

    public void solider_button_click_event(){
        building_.CreateUnit(0).SetBide();
        FinishProduct();
    }

    public void troop_button_click_event(){
        building_.CreateUnit(1).SetBide();
        FinishProduct();
    }

    public void missile_button_click_event(){
        building_.CreateUnit(2).SetBide();
        FinishProduct();
    }

    public void helicopter_button_click_event(){
        building_.CreateUnit(3).SetBide();
        FinishProduct();
    }

    public void tank_button_click_event(){
        building_.CreateUnit(4).SetBide();
        FinishProduct();
    }

    private void FinishProduct(){
        building_.HideFactoryUI();
        Task.GetInstance().GetCurrentCharacter().SubtractCash(building_.unit_.cash_);
        SelectManager s = SelectManager.GetInstance();
        s.select_gameObject_.GetComponent<ContourColor>().CancleColor();
        // s.current_gameObject_.GetComponent<ContourColor>().CancleColor();
        s.select_gameObject_ = null;
        // s.current_gameObject_ = null;
        s.current_state_ = CurrentStateEnum.Idle;
    }
}
