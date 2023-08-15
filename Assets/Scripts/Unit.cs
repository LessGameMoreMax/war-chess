using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    //Load from unit json
    public int id_;
    public string name_;
    public int position_property_;
    public int[] attack_position_property_;
    public int move_property_;
    public int view_property_;
    public int[] attack_scope_property_;
    public int[] move_terrain_property_;
    public int unit_type_;

    //Load from map json or the fact task
    public int guid_;
    public int x_;
    public int y_;
    public Tile tile_;

    public HashSet<GameObject> move_tiles_set_;
    public List<GameObject> path_tiles_list_;
    private Tile restore_tile_;
    private Vector3 restore_position_;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool CanAttack(int position_property){
        return attack_position_property_[position_property] == 1;
    }

    public virtual bool CanMove(int terrain_property_){
        return move_terrain_property_[terrain_property_] == 1;
    }

    public virtual void Restore(){
        tile_.unit_ = null;
        tile_ = restore_tile_;
        tile_.unit_ = this;
        transform.position = new Vector3(
            restore_position_.x,
            restore_position_.y,
            restore_position_.z
        );

        restore_tile_ = null;
        restore_position_ = new Vector3();
    }

    public virtual void SearchMove(){
        if(move_tiles_set_ == null) move_tiles_set_ = new HashSet<GameObject>();
        Task.GetInstance().map_.SearchMove(this, move_tiles_set_);
    }

    public virtual void ClearMove(){
        foreach(GameObject gameObject in move_tiles_set_){
            gameObject.GetComponent<TileColor>().ShowNormalColor();
        }
        move_tiles_set_.Clear();
    }

    public virtual void MoveTo(){
        StartCoroutine(Move());
    }

    public virtual bool HavePath(GameObject dist){
        return move_tiles_set_.Contains(dist);
    }

    public virtual void SearchPath(GameObject dist){
        if(path_tiles_list_ == null) path_tiles_list_ = new List<GameObject>();
        Task.GetInstance().map_.SearchPath(this, dist, path_tiles_list_);
    }

    public virtual void ClearPath(){
        foreach(GameObject gameObject in path_tiles_list_){
            gameObject.GetComponent<TileColor>().ShowMoveColor();
        }
        foreach(GameObject gameObject in move_tiles_set_){
            Tile tile = gameObject.GetComponent<Tile>();
            tile.g_ = 0;
            tile.f_ = 0;
            tile.parent_ = null;
        }
        path_tiles_list_.Clear();
    }

    private IEnumerator Move(){
        MoveBegin();
        while(path_tiles_list_.Count > 0){
            float work_time = 0;
            Vector3 origin_pos = transform.position;
            Vector3 dist_pos = path_tiles_list_[0].transform.position;
            while(true){
                work_time += Time.deltaTime;
                transform.position = Vector3.Lerp(origin_pos, dist_pos, work_time);
                transform.position = new Vector3(transform.position.x, origin_pos.y, transform.position.z);
                if(work_time >= 1.0f){
                    tile_.unit_ = null;
                    tile_ = path_tiles_list_[0].GetComponent<Tile>();
                    tile_.unit_ = this;
                    path_tiles_list_.RemoveAt(0);
                    break;
                }
                yield return null;
            }
        }
        MoveFinish();
    }

    private void MoveBegin(){
        restore_tile_ = path_tiles_list_[0].GetComponent<Tile>();
        restore_position_ = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );
        //...

        path_tiles_list_.RemoveAt(0);
    }

    private void MoveFinish(){
        ClearPath();
        ClearMove();
        SelectManager.GetInstance().current_state_ = CurrentStateEnum.Moved;
    }


}
