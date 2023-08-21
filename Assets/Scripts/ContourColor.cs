using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourColor : MonoBehaviour
{

    private Outline outline_;
    [SerializeField]
    public Color preselect_color_;
    [SerializeField]
    public Color selected_color_;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize(){
        outline_ = GetComponent<Outline>(); 
        outline_.enabled = false;
        preselect_color_ = Color.white;
        selected_color_ = Color.red;
    }

    public void ShowPreSelectColor(){
        outline_.enabled = true;
        outline_.OutlineColor = preselect_color_;
    }

    public void ShowSelectColor(){
        outline_.enabled = true;
        outline_.OutlineColor = selected_color_;
    }

    public void CancleColor(){
        outline_.enabled = false;        
    }
}
