using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnUI : MonoBehaviour
{
    public Button next_turn_button_;
    public TextMeshProUGUI message_text_;
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

    public void SetMessageText(string character_name, int character_cash){
        message_text_.text = character_name + " : " + character_cash.ToString();
    }
}
