using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CombatSystem
{
public class GridObject
{
    int x;
    int y;
    Grid<GridObject> grid;
    private bool walkable = true;
    public GameObject objectInTile;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;
    
    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    
    // checkes if this grid tile is walkable
    public bool isWalkable()
    {
        if(objectInTile != null)
        {
            return false;
        }
        return walkable;
    }

    public override string ToString()
    {
        return x.ToString() + " " + y.ToString();
    }

    public Vector3 GetCellPos()
    {
        return grid.GetWorldPos(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * .5f;
    }

    public void setIdleCellSprite(GameObject sprite)
    {
        idleCellSprite = sprite;
    }

    public void setActiveCellSprite(GameObject sprite)
    {
        activeCellSprite = sprite;
    }

    public GameObject getIdleCellSprite()
    {
        return idleCellSprite;
    }

    public GameObject getActiveCellSprite()
    {
        return activeCellSprite;
    }
}
}
