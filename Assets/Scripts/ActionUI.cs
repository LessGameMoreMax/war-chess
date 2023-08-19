using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    public int offset_x_;
    public int offset_y_;
    public bool is_show_;
    public Unit unit_;
    public Button bide_button_;
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

    public void Show(Unit unit){
        Vector3 target = unit.transform.position + Vector3.up * offset_y_ + Vector3.right * offset_x_;
        transform.position = target;
        is_show_ = true;
        unit_ = unit;
        if(unit.HaveBide()) {
            bide_button_.interactable = true;
        }else {
            bide_button_.interactable = false;
        }
    }

    public void Hide(){
        is_show_ = false;
    }

    public void bide_button_click_event(){
        unit_.HideActionUI();
        unit_.SetBide();
        GameObject select_unit = SelectManager.GetInstance().select_gameObject_;
        select_unit.GetComponent<ContourColor>().CancleColor();
        SelectManager.GetInstance().select_gameObject_ = null;
        SelectManager.GetInstance().current_state_ = CurrentStateEnum.Idle;
    }
}
