using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public int offset_x_;
    public int offset_y_;
    public TextMeshProUGUI text_;
    public Unit unit_;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(unit_ != null){
            Vector3 target = unit_.transform.position + Vector3.up * offset_y_ + Vector3.right * offset_x_;
            transform.position = target;
        }
        
    }

    public void SetText(int health){
        text_.text = health.ToString();
    }
}
