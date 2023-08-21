using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPool : Singleton<UIPool>
{
    public GameObject action_ui_;
    public GameObject health_ui_prefab_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeUIPool(){
        //ActionUI
        action_ui_ = UIInstantiate.Generate(Resources.Load<GameObject>("Prefab/ActionUI"));
        ActionUI temp = action_ui_.GetComponent<ActionUI>();
        temp.offset_x_ = 0;
        temp.offset_y_ = 1;
        temp.is_show_ = false;
        temp.bide_button_.onClick.AddListener(temp.bide_button_click_event);
        temp.attack_button_.onClick.AddListener(temp.attack_button_click_event);
        temp.load_button_.onClick.AddListener(temp.load_button_click_event);
        temp.unload_button_.onClick.AddListener(temp.unload_button_click_event);
        temp.occupy_button_.onClick.AddListener(temp.occupy_button_click_event);
        action_ui_.SetActive(false);
        //HealthUI
        health_ui_prefab_ = Resources.Load<GameObject>("Prefab/HealthUI");
        //...
    }

    public GameObject GetActionUI(){
        action_ui_.SetActive(true);
        return action_ui_;
    }

    public void CollectActionUI(){
        action_ui_.SetActive(false);
    }

    public void GetHealthUI(Unit unit){
        GameObject result = UIInstantiate.Generate(health_ui_prefab_);

        HealthUI temp = result.GetComponent<HealthUI>();
        temp.offset_x_ = 1;
        temp.offset_y_ = 1;
        temp.SetText(unit.health_);
        temp.unit_ = unit;
        unit.health_ui_ = temp;
    }
}

public class UIInstantiate: MonoBehaviour{
    public static GameObject Generate(GameObject ui_prefab){
        return Instantiate(ui_prefab);
    }
}
