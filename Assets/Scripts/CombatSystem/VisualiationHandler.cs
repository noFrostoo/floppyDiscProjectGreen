using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class VisualiationHandler : MonoBehaviour
{
    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGH_MOVE_COST = 10;
    public static VisualiationHandler Instance;
    [SerializeField] private GameCharacter player;
    GridComplete grid;
    [SerializeField] private GridCombatSystem gridCombatSystem;
    private bool mouseOutofRadious;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;

    private GameObject visualizeCell;

    // pool of objects for showing radious, path and ablites
    private GameObject[] radiousVisualizationnPool;
    private GameObject[] pathVisualizationPool;
    private GameObject[] abilitiesVisualizationPool;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SetUpVisualiation(20, "Path Cell", ref pathVisualizationPool);
        SetUpVisualiation(100, "Radious Cell", ref radiousVisualizationnPool);
        SetUpVisualiation(30, "Abilities Cell" ,ref abilitiesVisualizationPool);
        gridCombatSystem.onGridReady += SetUp;
        gridCombatSystem.OnPathChanged += HandlePathVisualization;

        visualizeCell = Instantiate(activeCellSprite, Vector3.zero, activeCellSprite.transform.rotation);
        visualizeCell.SetActive(false);

        activeCellGameObject = Instantiate(activeCellSprite, new Vector3(0,0,0), activeCellSprite.transform.rotation);
        activeCellGameObject.SetActive(false);
    }
    void SetUp()
    {
        grid = gridCombatSystem.GetGrid();
        
    }

    public void SetPlayerAndGrid(GameCharacter player, GridComplete grid)
    {
        this.grid = grid;
        this.player = player;
    }

     void SetUpVisualiation(int amount, string name, ref GameObject[] arr) //!!
    {
        arr = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            arr[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            arr[i].name = name;
            arr[i].SetActive(false);
        }
    }

    public void HandlePlayerRadiousVisualiation(GridObject currentActiveCell, GridObject playerCell)
    {
        if(!player.CellInRadious(currentActiveCell) && !mouseOutofRadious)
        { 
            VisualizeRadious(playerCell);  
            mouseOutofRadious = true;
        }
        else if(player.CellInRadious(currentActiveCell) && mouseOutofRadious)
        {
            ClearVisalizeRadious();
            mouseOutofRadious = false;
        }
    }


    void VisualizeRadious(GridObject playerCell)
    {
        int poolingCount = 0;
        int maxCellOffset = player.MaxCellMovmentRadious();
        for(int i = -maxCellOffset-1; i <= maxCellOffset; i++)
            for(int j = -maxCellOffset-1; j <= maxCellOffset; j++)
            {
                GridObject cell = grid.GetGridObject(playerCell.x() + i, playerCell.y() + j);
                if(cell != null && cell.isWalkable() && player.CellInRadious(cell))
                {
                    radiousVisualizationnPool[poolingCount].transform.position = cell.GetCellPos();
                    radiousVisualizationnPool[poolingCount].SetActive(true);
                    poolingCount++;
                }
            }
    }

    void ClearVisalizeRadious()
    {
        for(int i = 0; i < radiousVisualizationnPool.Length; i++)
        {
            radiousVisualizationnPool[i].SetActive(false);
        }
    }

    void VisualizePath(List<GridObject> path)
    {
        if(path == null) return;
        int index = 0;
        foreach(var cell in path)
        {
            pathVisualizationPool[index].SetActive(true);
            pathVisualizationPool[index].transform.position = cell.GetCellPos();
            index++;
        }
    }

    void ClearVisualizePath()
    {
        for(int i = 0; i < pathVisualizationPool.Length; i++)
        {
            pathVisualizationPool[i].SetActive(false);
        }
    }

    void HandlePathVisualization(object sender, GridCombatSystem.OnPathChangedArgs e)
    {
        ClearVisualizePath();
        VisualizePath(e.newPath);
    }

    public void ClearVisualization()
    {
        ClearVisalizeRadious();
        ClearVisualizePath();
    }

    public void ActiveCellVisualization(GridObject currentActiveCell)
    {
        if(currentActiveCell == null) 
            activeCellGameObject.SetActive(false);
        else
        {
            activeCellGameObject.SetActive(true); //ToDO if there is a way to active state test if one if is faster then this
            activeCellGameObject.transform.position = currentActiveCell.GetCellPos();
            
        }
    }
    public void VisualizeArea(Vector3 pos, int radious)
    {
        VisualizeArea(grid.GetGridObject(pos), radious);
    }
    
    public void VisualizeArea(GridObject center, int radious)
    {
        int x = center.x();
        int y = center.y();
        int radiousCost = radious * STRAIGH_MOVE_COST;
        int count = 0;
        for (int i = -radious-1; i <= radious; i++)
            for (int j = -radious-1 ; j <= radious; j++)
            {
                GridObject cell = grid.GetGridObject(x + i, y + i);
                if(cell != null && cell.isWalkable() && CalculateDistance(center, cell) <= radiousCost)
                {
                    abilitiesVisualizationPool[count].transform.position = cell.GetCellPos();
                    abilitiesVisualizationPool[count].SetActive(true);
                    count++;
                }
            }
    }

    public void ClearVisualizeArea()
    {
        for (int i = 0; i < abilitiesVisualizationPool.Length; i++)
        {
            abilitiesVisualizationPool[i].SetActive(false);
        }
    }

    public void VisualiseCell(GridObject cell)
    {
        visualizeCell.SetActive(true);
        visualizeCell.transform.position = cell.GetCellPos();
    }

    public void ClearVisualiseCell()
    {
        visualizeCell.SetActive(false);
    }

    private int CalculateDistance(GridObject pn1, GridObject pn2)
    {
        int discX = Math.Abs(pn1.x() - pn2.x());
        int discY = Math.Abs(pn1.y() - pn2.y());
        int reamaing = Math.Abs(discX - discY);
        return DIAGONAL_MOVE_COST*Math.Min(discX, discY) + STRAIGH_MOVE_COST*reamaing;
    }
}
}
}