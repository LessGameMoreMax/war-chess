using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map{
    public int id_;
    public string name_;
    public int width_;
    public int height_;
    public float interval_;
    public List<GameObject> tiles_;

    // Use for move
    List<GameObject> open_list_;
    List<GameObject> close_list_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetTile(int x, int y){
        if(x < 0 || y < 0 || x >= width_ || y >= height_) return null;
        int index = x + y * width_;
        if(index >= tiles_.Count) return null;
        return tiles_[index];
    }

    public void SearchMove(Unit unit, HashSet<GameObject> move_tiles_set){
        GameObject temp = GetTile(unit.x_, unit.y_);
        temp.GetComponent<TileColor>().ShowMoveColor();
        move_tiles_set.Add(temp);

        RecursiveSearchMove(unit.x_ - 1, unit.y_, unit, unit.move_property_, move_tiles_set);
        RecursiveSearchMove(unit.x_, unit.y_ - 1, unit, unit.move_property_, move_tiles_set);
        RecursiveSearchMove(unit.x_ + 1, unit.y_, unit, unit.move_property_, move_tiles_set);
        RecursiveSearchMove(unit.x_, unit.y_ + 1, unit, unit.move_property_, move_tiles_set);
    }

    private void RecursiveSearchMove(int x, int y, Unit unit, int move_property, HashSet<GameObject> move_tiles_set){
        GameObject temp = GetTile(x, y);
        if(temp == null) return;
        Tile tile = temp.GetComponent<Tile>();
        int block_property = tile.GetBlockProperty(unit.unit_type_);
        if(move_property < block_property || !unit.CanMove(tile)) return;
        unit.SearchMoveCondition(tile);
        move_property -= block_property;
        if(!move_tiles_set.Contains(temp)){
            temp.GetComponent<TileColor>().ShowMoveColor();
            move_tiles_set.Add(temp);
        }
        
        RecursiveSearchMove(x - 1, y, unit, move_property, move_tiles_set);
        RecursiveSearchMove(x, y - 1, unit, move_property, move_tiles_set);
        RecursiveSearchMove(x + 1, y, unit, move_property, move_tiles_set);
        RecursiveSearchMove(x, y + 1, unit, move_property, move_tiles_set);
    }

    public void SearchAttack(Unit unit){
        int attack_scope = unit.attack_scope_property_[1];
        if(attack_scope == 0) return;

        unit.FindAttackRange(unit.tile_.gameObject, false);
        RecursiveSearchAttack(unit.x_ - 1, unit.y_, unit, unit.move_property_);
        RecursiveSearchAttack(unit.x_, unit.y_ - 1, unit, unit.move_property_);
        RecursiveSearchAttack(unit.x_ + 1, unit.y_, unit, unit.move_property_);
        RecursiveSearchAttack(unit.x_, unit.y_ + 1, unit, unit.move_property_);
    }

    private void RecursiveSearchAttack(int x, int y, Unit unit, int move_property){
        int unit_type = unit.unit_type_;
        GameObject temp = GetTile(x, y);
        if(temp == null) return;
        Tile tile = temp.GetComponent<Tile>();
        int block_property = tile.GetBlockProperty(unit_type);
        if(move_property < block_property || !unit.CanMove(tile)) return;
        move_property -= block_property;
        if(tile.unit_ == null) unit.FindAttackRange(temp, false);
        
        RecursiveSearchAttack(x - 1, y, unit, move_property);
        RecursiveSearchAttack(x, y - 1, unit, move_property);
        RecursiveSearchAttack(x + 1, y, unit, move_property);
        RecursiveSearchAttack(x, y + 1, unit, move_property); 
    }

    public void SearchPath(Unit src, GameObject dist, List<GameObject> path_tiles_list){
        if(open_list_ == null) open_list_ = new List<GameObject>();
        if(close_list_ == null) close_list_ = new List<GameObject>();
        CaculatePath(src.tile_.gameObject, dist, path_tiles_list, src);
    }

    private void CaculatePath(GameObject from, GameObject to, List<GameObject> path_tiles_list, Unit unit){
        open_list_.Clear();
        close_list_.Clear();
        unit.ClearPath();

        open_list_.Add(from);
        while (open_list_.Count != 0){
            GameObject temp = open_list_[0];
            if (temp == to){
                Tile tile = null;
                for(GameObject gameObject = to; gameObject != null; gameObject = tile.parent_){
                    path_tiles_list.Insert(0, gameObject);
                    gameObject.GetComponent<TileColor>().ShowPathColor();
                    tile = gameObject.GetComponent<Tile>();
                }
                break;
            }else{
                open_list_.RemoveAt(0);
                close_list_.Add(temp);
                List<GameObject> neighbours = FindNeighbours(temp);
                for (int i = 0;i != neighbours.Count; ++i){
                    GameObject n = neighbours[i];
                    Tile temp_tile = temp.GetComponent<Tile>();
                    Tile n_tile = n.GetComponent<Tile>();
                    if (close_list_.Contains(n) || !unit.CanMove(n_tile))
                        continue;
                    int g = temp_tile.g_ + n_tile.GetBlockProperty(unit.unit_type_);
                    int h = CaculateH(n, to);
                    int f = g + h;
                    if (!open_list_.Contains(n)){
                        n_tile.parent_ = temp;
                        if (open_list_.Count == 0){
                            open_list_.Add(n);
                        }else{
                            if (f < open_list_[0].GetComponent<Tile>().f_)
                                open_list_.Insert(0, n);
                            else
                                open_list_.Add(n);
                        }
                    }else if (f < n_tile.f_){
                        n_tile.f_ = f;
                        n_tile.g_ = g;
                        n_tile.parent_ = temp;
                    }
                }
            }
        }
    }

    private List<GameObject> FindNeighbours(GameObject temp){
        List<GameObject> list = new List<GameObject>();
        int x = temp.GetComponent<Tile>().x_;
        int y = temp.GetComponent<Tile>().y_;

        GameObject tile = GetTile(x - 1, y);
        if(tile != null) list.Add(tile);

        tile = GetTile(x + 1, y);
        if(tile != null) list.Add(tile);

        tile = GetTile(x, y - 1);
        if(tile != null) list.Add(tile);

        tile = GetTile(x, y + 1);
        if(tile != null) list.Add(tile);

        return list;
    }

    private int CaculateH(GameObject from, GameObject to){
        int x = Mathf.Abs(from.GetComponent<Tile>().x_ - to.GetComponent<Tile>().x_);
        int y = Mathf.Abs(from.GetComponent<Tile>().y_ - to.GetComponent<Tile>().y_);
        return x + y;
    }
}
