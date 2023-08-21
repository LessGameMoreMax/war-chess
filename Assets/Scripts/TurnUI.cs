using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    public Button next_turn_button_;
    // Start is called before the first frame update
    void Start()
    {
        next_turn_button_.onClick.AddListener(next_turn_button_click_event);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next_turn_button_click_event(){
        Task.GetInstance().NextTurn();
    }
}
