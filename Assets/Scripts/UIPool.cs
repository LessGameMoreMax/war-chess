using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPool : Singleton<UIPool>
{
    public GameObject action_ui_;
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
        action_ui_.SetActive(false);
        //...
    }

    public GameObject GetActionUI(){
        action_ui_.SetActive(true);
        return action_ui_;
    }

    public void CollectActionUI(){
        action_ui_.SetActive(false);
    }
}

public class UIInstantiate: MonoBehaviour{
    public static GameObject Generate(GameObject ui_prefab){
        return Instantiate(ui_prefab);
    }
}
