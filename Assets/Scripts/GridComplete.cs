using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class GridComplete : Grid<GridObject>
{


    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGH_MOVE_COST = 10;

    private List<GridObject> closedList;
    private List<GridObject> openedList;
    private GridObject currentNode;

    public GridComplete(int rows, int colls, float cellSize, Vector3 originPosition, bool debug = true, List<Vector2Int> unwalakbleList = null) : base(rows, colls, cellSize, originPosition, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), debug)
    {
        if (unwalakbleList != null)
            SetUnwalkable(unwalakbleList);
        
        
    }

    public void SetUnwalkable(List<Vector2Int> unWalkableList)
    {
        foreach(var cellXY in unWalkableList)
            SetUnwalkable(cellXY.x, cellXY.y);
    }

    public void SetUnwalkable(int x, int y)
    {
        GetGridObject(x, y).SetWalkable(false);
        TriggerGridObjectChange(x, y);
    }

    public List<GridObject> FindPath(Vector3 startWoldPos, Vector3 endPos)
    {
        return FindPath(GetGridObject(startWoldPos), GetGridObject(endPos));
    }

    public List<GridObject> FindPath(int startX, int startY, int endX, int endY)
    {
        return FindPath(GetGridObject(startX, startY), GetGridObject(endX, endY));
    }

    public List<GridObject> FindPath(GridObject startNode, GridObject endNode)
    {
        if(startNode == null || endNode == null) 
            return null;
        startNode.heurecticCostToTheEnd = CalculateDistance(startNode, endNode);
        startNode.costFromStartToTheCell = 0;
        startNode.CalculateTotalCost();
        startNode.previousNode = null;
        
        openedList = new List<GridObject>();
        openedList.Add(startNode);
        closedList = new List<GridObject>();

        for(int x = 0; x < GetCollumns(); x++)
            for(int y = 0; y < GetRows(); y++)
                {
                    GridObject node = GetGridObject(x, y);
                    node.costFromStartToTheCell = Int32.MaxValue;
                    node.previousNode = null;
                    node.CalculateTotalCost();
                }

        //piorityQueue = new PiorityQueueHeap<GridObject>(tempList);
        while(openedList.Count > 0) 
        {
            currentNode = GetLowestTotalCostNode();
            if (currentNode == endNode)
            {
                return GetPath(currentNode);
            }
            openedList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(var neighbour in GetNeighbours(currentNode)) 
            {
                if(closedList.Contains(neighbour)) continue;

                int estimateGcost = currentNode.costFromStartToTheCell + CalculateDistance(currentNode, neighbour);
                
                if(estimateGcost < neighbour.costFromStartToTheCell)
                {
                    neighbour.previousNode = currentNode;
                    neighbour.costFromStartToTheCell = estimateGcost;
                    neighbour.heurecticCostToTheEnd = CalculateDistance(neighbour, endNode);
                    neighbour.CalculateTotalCost();
                    
                    if(!openedList.Contains(neighbour))
                        openedList.Add(neighbour);
                }
            }
        }

        
        return null;
    }

    private int CalculateDistance(GridObject pn1, GridObject pn2)
    {
        int discX = Math.Abs(pn1.x() - pn2.x());
        int discY = Math.Abs(pn1.y() - pn2.y());
        int reamaing = Math.Abs(discX - discY);
        return DIAGONAL_MOVE_COST*Math.Min(discX, discY) + STRAIGH_MOVE_COST*reamaing;
    }

    private List<GridObject> GetNeighbours(GridObject node)
    {
        List<GridObject> exitList = new List<GridObject>();
        for(int i = -1; i <= 1; i++)
            for(int j = -1; j <= 1; j++)
            {
                GridObject currentNode = GetGridObject(node.x() + i, node.y() + j);
                if( !( i == 0 && j == 0) && currentNode != null && currentNode.isWalkable())
                {
                    exitList.Add(currentNode);
                }
            }
        return exitList;
    }

    private List<GridObject> GetPath(GridObject endNode)
    {
        List<GridObject> path = new List<GridObject>{endNode};
        currentNode = endNode.previousNode;
        
        while(currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private GridObject GetLowestTotalCostNode()
    {
        GridObject minTotalCostNode = openedList[0];
        foreach(var node in openedList)
            if(minTotalCostNode.totalcost > node.totalcost)
                minTotalCostNode = node;
        return minTotalCostNode;
    }


}
}
}