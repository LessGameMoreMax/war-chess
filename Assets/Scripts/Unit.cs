using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitStateEnum{
    Active,
    Bide
}

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
    public int character_id_;
    public Material material_;
    public Color active_color_;
    public Color bide_color_;
    public UnitStateEnum current_unit_state_;
    
    //Use for move
    public HashSet<GameObject> move_tiles_set_;
    public List<GameObject> path_tiles_list_;
    private Tile restore_tile_;
    private Vector3 restore_position_;

    //Use for attack
    public HashSet<GameObject> attack_tiles_set_;
    public HashSet<GameObject> attack_real_tiles_set_;


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
                    x_ = tile_.x_;
                    y_ = tile_.y_;
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
        ShowActionUI();
    }

    public virtual void InitializeCharacter(int character_id){
        character_id_ = character_id;
        Character character = Task.GetInstance().GetCharacter(character_id_);
        character.AddUnit(guid_);
        material_ = GetComponent<MeshRenderer>().material;
        active_color_ = character.faction_.color_;
        bide_color_ = Color.black;
    }

    public virtual void SetActive(){
        material_.color = active_color_;
        current_unit_state_ = UnitStateEnum.Active;
    }

    public virtual void SetBide(){
        material_.color = bide_color_;
        current_unit_state_ = UnitStateEnum.Bide;
    }

    public virtual bool IsActive(){
        return current_unit_state_ == UnitStateEnum.Active;
    }

    public virtual bool IsBide(){
        return current_unit_state_ == UnitStateEnum.Bide;
    }

    public virtual void ShowActionUI(){
        GameObject temp = UIPool.GetInstance().GetActionUI();
        temp.GetComponent<ActionUI>().Show(this);
    }

    public virtual void HideActionUI(){
        GameObject temp = UIPool.GetInstance().GetActionUI();
        temp.GetComponent<ActionUI>().Hide();
        UIPool.GetInstance().CollectActionUI();
    }

    public virtual bool HaveBide(){
        return true;
    }

    public virtual void ShowAttackRange(){
        Task.GetInstance().map_.SearchAttack(this);
        foreach(GameObject temp in attack_tiles_set_)
            temp.GetComponent<TileColor>().ShowAttackColor();
    }

    public virtual void ClearAttackRange(){
        foreach(GameObject temp in attack_tiles_set_)
            temp.GetComponent<TileColor>().ShowNormalColor();
        attack_tiles_set_.Clear();
    }

    public virtual void SearchRealAttackRange(){
        FindAttackRange(tile_.gameObject, true);
        foreach(GameObject temp in attack_real_tiles_set_)
            temp.GetComponent<TileColor>().ShowAttackColor();
    }

    public virtual void ClearRealAttackRange(){
        foreach(GameObject temp in attack_real_tiles_set_)
            temp.GetComponent<TileColor>().ShowNormalColor();
        attack_real_tiles_set_.Clear();
    }

    public virtual void FindAttackRange(GameObject temp, bool is_add_to_real_set){
        int min_scope = attack_scope_property_[0];
        int max_scope = attack_scope_property_[1];
        Tile tile = temp.GetComponent<Tile>();
        int x = tile.x_;
        int y = tile.y_;
        HashSet<GameObject> black_set = new HashSet<GameObject>();
        HashSet<GameObject> set = new HashSet<GameObject>();
        black_set.Add(temp);
        RecursiveFindAttackRange(x - 1, y, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x + 1, y, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x, y - 1, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x, y + 1, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        if(is_add_to_real_set){
            foreach(GameObject obj in set){
                Tile temp_tile = obj.GetComponent<Tile>();
                if(!attack_real_tiles_set_.Contains(obj) && !black_set.Contains(obj) && temp_tile.unit_ != null && CanAttack(temp_tile.unit_.position_property_)) 
                    attack_real_tiles_set_.Add(obj);
            }
                
        }else{
            foreach(GameObject obj in set){
                if(!attack_tiles_set_.Contains(obj) && !black_set.Contains(obj))
                    attack_tiles_set_.Add(obj);
            }
        }
    }

    private void RecursiveFindAttackRange(int x, int y, int min_scope, int max_scope, HashSet<GameObject> black_set, HashSet<GameObject> set){
        Map map = Task.GetInstance().map_;
        GameObject temp = map.GetTile(x, y);
        if(temp == null || black_set.Contains(temp)) return;
        Tile tile = temp.GetComponent<Tile>();
        if(min_scope == 0){
            if(!set.Contains(temp))
                set.Add(temp);
        }else{
            black_set.Add(temp);
        }
        
        if(max_scope == 0) return;
        RecursiveFindAttackRange(x - 1, y, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x + 1, y, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x, y - 1, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
        RecursiveFindAttackRange(x, y + 1, Mathf.Max(0, min_scope - 1), max_scope - 1, black_set, set);
    }


}
