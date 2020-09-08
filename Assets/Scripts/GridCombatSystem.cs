using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using System;

public class GridCombatSystem : MonoBehaviour
{
    public delegate void OnGridReadEventHandler();
    public event OnGridReadEventHandler onGridReady;

    private GridComplete grid;
    private Pathfinding pFgrid;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;
    private GridObject lastActiveCell;
    private GridObject lastPlayerCell;
    List<GridObject> path = null;
    public bool gridRead = false;

    private bool mouseOutofRadious;
    private GameObject[] radiousVisualizationnPool;
    private GameObject[] pathVisualizationPool;

    [SerializeField] private bool debug;
    [SerializeField] private  List<Vector2Int> unWalkableCells; 
    private GameObject[] charactersInFight;
    private GameCharacter player;

    void Start()
    {
        //grid = new Grid<GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), debug );
        //pFgrid = new Pathfinding(10, 20, 1f, new Vector3(-13, -6));
        grid = new GridComplete(10, 20, 1f, new Vector3(-13, -6), debug);
        grid.SetUnwalkable(unWalkableCells);

        SetUp();
        SetUpRadiousVisualiation(100);
        SetUpPathVisualiation(15);
        SetUpOnGridReady();
        UpdateObjectsInCellRefrences();
    }

    void SetUp()
    {
        foreach(var gridcell in grid.InOrderIteration())
        {
            //updateIdleCell(gridcell);
            //gridcell.setActiveCellSprite(activeCellSprite);
        }
        activeCellGameObject = Instantiate(activeCellSprite, new Vector3(0,0,0), activeCellSprite.transform.rotation);
        activeCellGameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacter>();
        charactersInFight = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void SetUpRadiousVisualiation(int amount)
    {
        radiousVisualizationnPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            radiousVisualizationnPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            radiousVisualizationnPool[i].name = "PathCell";
            radiousVisualizationnPool[i].SetActive(false);
        }
    }

    void SetUpPathVisualiation(int amount)
    {
        pathVisualizationPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            pathVisualizationPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            pathVisualizationPool[i].SetActive(false);
        }
    }
    public void OnGridReadyAction()
    {
        gridRead = true;
    }

    void SetUpOnGridReady()
    {
        onGridReady += OnGridReadyAction;
        if(onGridReady != null) onGridReady();
    }

    public void UpdateObjectsInCellRefrences()
    {
        grid.GetGridObject(player.transform.position).SetObjectInTile(player.gameObject);
        foreach(var character in charactersInFight)
        {
            grid.GetGridObject(character.transform.position).SetObjectInTile(character);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        var currentActiveCell = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var playerCell = grid.GetGridObject(player.transform.position);

        if(lastActiveCell == null) lastActiveCell = currentActiveCell;
        if(lastPlayerCell == null) lastPlayerCell = playerCell;

        updateActiveCell(currentActiveCell); 
        if(!player.CellInRadious(currentActiveCell) && !mouseOutofRadious)
        { 
            VisualizeRadious(playerCell);  
            mouseOutofRadious = true;
        }
        else if(player.CellInRadious(currentActiveCell) && mouseOutofRadious)
        {
            EndVisalizeRadious(playerCell);
            mouseOutofRadious = false;
        }
   
        if(lastActiveCell != currentActiveCell || lastPlayerCell != playerCell)
        {  
            lastActiveCell = currentActiveCell;
            lastPlayerCell = playerCell;
            if(player.CellInRadious(currentActiveCell))
            {
                ClearVisualizePath();
                path = grid.FindPath(playerCell, currentActiveCell);
                VisualizePath(path);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition)).AttactObjectInTile(30);
        }  
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(path != null)
                player.MoveTo(path);
            grid.GetGridObject(mousePos).SetObjectInTile(player.gameObject);
        }
    }


    public GridComplete GetGrid()
    {
        return grid;
    }

    void updateActiveCell(GridObject currentActiveCell)
    {
        if(currentActiveCell == null) 
            activeCellGameObject.SetActive(false);
        else
        {
            activeCellGameObject.SetActive(true); //ToDO if there is a way to active state test if one if is faster then this
            activeCellGameObject.transform.position = currentActiveCell.GetCellPos();
            
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

    void EndVisalizeRadious(GridObject playerCell)
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

    void updateIdleCell(GridObject gridcell)
    {
        if(gridcell.getIdleCellSprite() == null) Destroy(gridcell.getIdleCellSprite());
        gridcell.setIdleCellSprite(Instantiate(idleCellSprite, gridcell.GetCellPos(), idleCellSprite.transform.rotation) as GameObject);
    }
}
