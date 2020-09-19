using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class GridObject : IComparable<GridObject>
{
    int xPos;
    int yPos;
    Grid<GridObject> grid;
    private bool walkable = true;
    public GameObject objectInTile;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;
    
    public int totalcost;
    public int costFromStartToTheCell;
    public int heurecticCostToTheEnd;

    public GridObject previousNode;

    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.xPos = x;
        this.yPos = y;
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

    public void SetWalkable(bool newWalkable)
    {
        walkable = newWalkable;
    }

    public int x()
    {
        return xPos;
    }

    public int y()
    {
        return yPos;
    }

    public void SetObjectInTile(GameObject newObject)
    {
        objectInTile = newObject;
    }

    public void AttactObjectInTile(int damage)
    {
        if(objectInTile != null)
            objectInTile.GetComponent<GameCharacter>().TakeDamage(damage);
    }

    public override string ToString()
    {
        if(objectInTile) return objectInTile.name;
        if(walkable)
            return xPos.ToString() + " " + yPos.ToString();
        else
            return "X";
    }

    public Vector3 GetCellPos()
    {
        return grid.GetWorldPos(xPos, yPos) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * .5f;
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

    public void CalculateTotalCost()
    {
        totalcost = costFromStartToTheCell + heurecticCostToTheEnd;
    }

    public void SetTotalCost(int newTotalCost)
    {
        totalcost = newTotalCost;
    }
    
    public GameObject GetObjectInCell()
    {
        return objectInTile;
    }
    public int CompareTo(GridObject other)
    {
        if(other == null) return 1;
        return totalcost.CompareTo(other.totalcost);
    }

    public static bool operator > (GridObject operand1, GridObject operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }

    public static bool operator < (GridObject operand1, GridObject operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }

    public static bool operator >= (GridObject operand1, GridObject operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <= (GridObject operand1, GridObject operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
    
}
}
}