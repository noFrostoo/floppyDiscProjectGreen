using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.DataStructures;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class Pathfinding
{
    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGH_MOVE_COST = 10;
    private Grid<PathNode> grid;
    private PiorityQueueHeap<PathNode> piorityQueue;
    private List<PathNode> closedList;
    private List<PathNode> openedList;
    private PathNode currentNode;
    
    public Pathfinding(int rows, int colls, float cellSize ,Vector3 originPos )
    {
        grid = new Grid<PathNode>(rows, colls, cellSize ,originPos, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), false);
    }

    public List<PathNode> FindPath(Vector3 startWoldPos, Vector3 endPos)
    {
        return FindPath(grid.GetGridObject(startWoldPos), grid.GetGridObject(endPos));
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        return FindPath(grid.GetGridObject(startX, startY), grid.GetGridObject(endX, endY));
    }

    private List<PathNode> FindPath(PathNode startNode, PathNode endNode)
    {
        startNode.heurecticCostToTheEnd = CalculateDistance(startNode, endNode);
        startNode.costFromStartToTheCell = 0;
        startNode.CalculateTotalCost();
        startNode.previousNode = null;
        
        openedList = new List<PathNode>();
        openedList.Add(startNode);
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetCollumns(); x++)
            for(int y = 0; y < grid.GetRows(); y++)
                {
                    PathNode node = grid.GetGridObject(x, y);
                    node.costFromStartToTheCell = Int32.MaxValue;
                    node.previousNode = null;
                    node.CalculateTotalCost();
                }

        //piorityQueue = new PiorityQueueHeap<PathNode>(tempList);
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

    private int CalculateDistance(PathNode pn1, PathNode pn2)
    {
        int discX = Math.Abs(pn1.x - pn2.x);
        int discY = Math.Abs(pn1.y - pn2.y);
        int reamaing = Math.Abs(discX - discY);
        return DIAGONAL_MOVE_COST*Math.Min(discX, discY) + STRAIGH_MOVE_COST*reamaing;
    }

    private List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> exitList = new List<PathNode>();
        for(int i = -1; i <= 1; i++)
            for(int j = -1; j <= 1; j++)
            {
                PathNode currentNode = grid.GetGridObject(node.x + i, node.y + j);
                if( !( i == 0 && j == 0) && currentNode != null)
                {
                    exitList.Add(currentNode);
                }
            }
        return exitList;
    }

    private List<PathNode> GetPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>{endNode};
        currentNode = endNode.previousNode;
        
        while(currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private PathNode GetLowestTotalCostNode()
    {
        PathNode minTotalCostNode = openedList[0];
        foreach(var node in openedList)
            if(minTotalCostNode.totalcost > node.totalcost)
                minTotalCostNode = node;
        return minTotalCostNode;
    }
}

public class PathNode : IComparable<PathNode>
{
    public int x;
    public int y;
    private Grid<PathNode> grid;
    public int totalcost;
    public int costFromStartToTheCell;
    public int heurecticCostToTheEnd;

    public bool isWakalbe;
    public PathNode previousNode;

    public PathNode(Grid<PathNode> grid, int x, int y, bool walkable = true)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWakalbe = walkable;
    }

    public void CalculateTotalCost()
    {
        totalcost = costFromStartToTheCell + heurecticCostToTheEnd;
    }

    public void SetTotalCost(int newTotalCost)
    {
        totalcost = newTotalCost;
    }
    public int CompareTo(PathNode other)
    {
        if(other == null) return 1;
        return totalcost.CompareTo(other.totalcost);
    }

    public static bool operator > (PathNode operand1, PathNode operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }

    public static bool operator < (PathNode operand1, PathNode operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }

    public static bool operator >= (PathNode operand1, PathNode operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <= (PathNode operand1, PathNode operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }

    public override string ToString()
    {
        return x +" "+ y;
    }
}
}
}