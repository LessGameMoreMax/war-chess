using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可以创建一个信息池，保存同一种类建筑的共同信息，
//仅仅存储类型信息，使用时通过类型信息查询信息池即可，
//如cash, max_health等属性可通过此方法存储。
//可按角色来分类，便于实现角色数值特性,
//即每个角色分别维护unit，tile等数值表
public class Building : Tile
{
    public int character_id_;
    public Material material_;
    public Color neutral_color_;
    public int current_health_;
    
    public int max_health_;
    public int cash_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void InitializeBuilding(){
        character_id_ = -1;
        material_ = GetComponent<MeshRenderer>().material;
        neutral_color_ = Color.black;
        SetNeutralColor();
        SetMaxHealth();
    }

    public virtual void AttachToCharacter(int character_id){
        character_id_ = character_id;
        Task task = Task.GetInstance();
        if(character_id == -1){
            task.neutral_building_set_.Add(this);
            SetNeutralColor();
        }else{
            Character character = task.GetCharacter(character_id);
            character.AddBuilding(this);
            SetColor(character.faction_.color_);
        }
    }

    public virtual void DeleteFromCharacter(){
        if(character_id_ == -1){
            Task.GetInstance().neutral_building_set_.Remove(this);
        }else{
            Character character = Task.GetInstance().GetCharacter(character_id_);
            character.RemoveBuilding(this);
        }
        SetNeutralColor();
    }

    public virtual void SetNeutralColor(){
        GetComponent<TileColor>().ChangeNormalColor(neutral_color_);
        material_.color = neutral_color_;
    }

    public virtual void SetColor(Color color){
        GetComponent<TileColor>().ChangeNormalColor(color);
        material_.color = color;
    }

    public virtual bool HasDead(){
        return current_health_ <= 0;
    }

    public virtual void SubtractHealth(int health){
        current_health_ -= health;
    }

    public virtual void SetMaxHealth(){
        current_health_ = max_health_;
    }

    public virtual int GetCash(){
        return cash_;
    }

    public virtual void ShowFactoryUI(){
        GameObject temp = UIPool.GetInstance().GetFactoryUI();
        temp.GetComponent<FactoryUI>().Show(this);
    }

    public virtual void HideFactoryUI(){
        GameObject temp = UIPool.GetInstance().GetFactoryUI();
        temp.GetComponent<FactoryUI>().Hide();
        UIPool.GetInstance().CollectFactoryUI();
    }

    public virtual bool IsFactory(){
        return false;
    }

    public virtual bool IsCurrentCharacter(){
        return character_id_ == Task.GetInstance().CurrentCharacterId();
    }

    public virtual Unit CreateUnit(int id){
        GameObject unit_prefab = UnitPrefabPool.GetInstance().GetUnitPrefab(id);
        int x = x_;
        int y = y_;
        GameObject unit_prefab_copy = Instantiate(unit_prefab, new Vector3(transform.position.x,
            transform.position.y + 1.0f, transform.position.z), new Quaternion());
        Unit unit = unit_prefab_copy.GetComponent<Unit>();
        unit.guid_ = Task.GetInstance().unit_guid_++;
        unit.x_ = x;
        unit.y_ = y;
        unit.health_ = unit.max_health_;
        UIPool.GetInstance().GetHealthUI(unit);
        unit.attack_tiles_set_ = new HashSet<GameObject>();
        unit.attack_real_tiles_set_ = new HashSet<GameObject>();
        // Tile tile = task.map_.GetTile(x, y).GetComponent<Tile>();
        unit.tile_ = this;
        unit_ = unit;
        unit.InitializeCharacter(character_id_);
        unit.gameObject.GetComponent<ContourColor>().Initialize();
        return unit;
    }
}
