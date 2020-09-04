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
    private PathNode currentNode;
    public Pathfinding(int rows, int colls, Vector3 originPos )
    {
        grid = new Grid<PathNode>(rows, colls, 1f ,originPos, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y), false);
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
        startNode.previousNode = null;
        List<PathNode> tempList = new List<PathNode>{startNode};
        piorityQueue = new PiorityQueueHeap<PathNode>(tempList);
        while(currentNode != endNode) 
        {
            currentNode = piorityQueue.Dequeue();
            foreach(var neighbour in GetNeighbours(currentNode)) 
            {
                neighbour.costFromStartToTheCell = CalculateDistance(startNode, neighbour);
                neighbour.heurecticCostToTheEnd = CalculateDistance(neighbour, endNode);
                neighbour.CalculateTotalCost();

            }
        }
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
        for(int i = -1; i < 1; i++)
            for(int j = -1; j < 1; j++)
            {
                PathNode currentNode = grid.GetGridObject(node.x + i, node.y + j);
                if( i != 0 && j != 0 && currentNode != null && currentNode.isWakalbe)
                    exitList.Add(currentNode);
            }
        return exitList;
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

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
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
}
}
}