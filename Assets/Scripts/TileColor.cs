using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour
{
    public Material material_;
    public Color normal_color_;
    public Color move_color_;
    public Color path_color_;
    public Color attack_color_;
    // Start is called before the first frame update
    void Start()
    {
        material_ = GetComponent<MeshRenderer>().material;
        normal_color_ = material_.color;
        move_color_ = Color.white;
        path_color_ = Color.green;
        attack_color_ = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNormalColor(){
        material_.color = normal_color_;
    }

    public void ShowMoveColor(){
        material_.color = move_color_;
    }

    public void ShowPathColor(){
        material_.color = path_color_;
    }

    public void ShowAttackColor(){
        material_.color = attack_color_;
    }

    public void ChangeNormalColor(Color color){
        normal_color_ = color;
    }
}
