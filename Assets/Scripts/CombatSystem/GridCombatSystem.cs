using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class GridCombatSystem : MonoBehaviour
{
    public enum State
    {
        playerRound,
        enemyRound,

    }
    public delegate void OnGridReadEventHandler();
    public event OnGridReadEventHandler onGridReady;
    public event EventHandler OnStateChange;
    public event EventHandler OnPlayerRound;
    public event EventHandler OnEnemyRound;

    private GridComplete grid;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;
    private GridObject lastActiveCell;
    private GridObject lastPlayerCell;
    List<GridObject> path = null;
    public bool gridRead = false;

    [SerializeField] State currentState;

    private bool mouseOutofRadious;
    private GameObject[] radiousVisualizationnPool;
    private GameObject[] pathVisualizationPool;

    [SerializeField] private bool debug;
    [SerializeField] private  List<Vector2Int> unWalkableCells; 
    private GameObject[] charactersInFight;
    private GameObject[] enemies;
    private GameCharacter player;

    void Start()
    {
        grid = new GridComplete(10, 20, 1f, new Vector3(-13, -6), debug);
        grid.SetUnwalkable(unWalkableCells);
        currentState = State.playerRound;
        OnStateChange += HandleStateChange;
        SetUp();
        SetUpRadiousVisualiation(100);
        SetUpPathVisualiation(15);

        UpdateObjectsInCellRefrences();
        SetUpOnGridReady();
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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void SetUpRadiousVisualiation(int amount) //!!
    {
        radiousVisualizationnPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            radiousVisualizationnPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            radiousVisualizationnPool[i].name = "PathCell";
            radiousVisualizationnPool[i].SetActive(false);
        }
    }

    void SetUpPathVisualiation(int amount)  //!!
    {
        pathVisualizationPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            pathVisualizationPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            pathVisualizationPool[i].SetActive(false);
        }
    }

    public void UpdateObjectsInCellRefrences()
    {
        grid.GetGridObject(player.transform.position).SetObjectInTile(player.gameObject);
        grid.TriggerGridObjectChange(grid.GetGridObject(player.transform.position).x(), grid.GetGridObject(player.transform.position).y());
        foreach(var character in charactersInFight)
        {
            grid.GetGridObject(character.transform.position).SetObjectInTile(character);
            grid.TriggerGridObjectChange(grid.GetGridObject(character.transform.position).x(), grid.GetGridObject(character.transform.position).y());
        }
    }

    void SetUpOnGridReady()
    {
        onGridReady += OnGridReadyAction;
        if(onGridReady != null) onGridReady();

    }
    
    public void OnGridReadyAction()
    {
        gridRead = true;
    }

    public GridComplete GetGrid()
    {
        return grid;
    }

    public State GetState()
    {
        return currentState;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var currentActiveCell = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var playerCell = grid.GetGridObject(player.transform.position);

        if(lastActiveCell == null) lastActiveCell = currentActiveCell;
        if(lastPlayerCell == null) lastPlayerCell = playerCell;

        updateActiveCell(currentActiveCell); 
        if(currentState == State.playerRound)
        {
            HandleRadiousVisualiation(currentActiveCell, playerCell);
            HandlePathFindingAndVisualiation(currentActiveCell, playerCell);
            HandleMouseInput(mousePos);
        }
        else
        {
            ClearVisalizeRadious();
            ClearVisualizePath();
        }
        if(debug) TriggerGridObjectChangeForWholeGrid();

    }

    void HandleRadiousVisualiation(GridObject currentActiveCell, GridObject playerCell)
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

    void HandlePathFindingAndVisualiation(GridObject currentActiveCell, GridObject playerCell)
    {
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
    }

    void HandleMouseInput(Vector3 mousePos)
    {
        if(currentState == State.playerRound)
        {
            if(Input.GetMouseButtonDown(0))
            {
                player.RangeAttack(grid.GetGridObject(mousePos).GetObjectInCell().GetComponent<GameCharacter>());
            }  
            if(Input.GetMouseButtonDown(1))
            {
                if(path != null && player.GetState() == GameCharacter.State.Idle)
                {
                    grid.GetGridObject(player.transform.position).SetObjectInTile(null);
                    player.MoveTo(path);
                    grid.GetGridObject(path[path.Count-1].GetCellPos()).SetObjectInTile(player.gameObject);
                }
            }
        }
    }

    private void ChangeState()
    {
        if(currentState == State.playerRound)
        {
            currentState = State.enemyRound;
        }else if(currentState == State.enemyRound)
        {
            currentState = State.playerRound;
        }
    } 

    public void TriggerGridObjectChangeForWholeGrid()
    {
        for(int x = 0; x < grid.GetCollumns(); x++)
            for(int y = 0; y < grid.GetRows(); y++)
            {
                grid.TriggerGridObjectChange(x, y);
            }
    }
  
    public void TriggerStateChange()
    {
        ChangeState();
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    
    void HandleStateChange(object sender, EventArgs e)
    {
        if(currentState == State.enemyRound)
            EnemyRound();
        if(currentState == State.playerRound)
            PlayerRound();
    }

    void EnemyRound()
    {
        OnEnemyRound?.Invoke(this, EventArgs.Empty);
    }

    void PlayerRound()
    {
        OnPlayerRound?.Invoke(this, EventArgs.Empty);
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

    void updateIdleCell(GridObject gridcell)
    {
        if(gridcell.getIdleCellSprite() == null) Destroy(gridcell.getIdleCellSprite());
        gridcell.setIdleCellSprite(Instantiate(idleCellSprite, gridcell.GetCellPos(), idleCellSprite.transform.rotation) as GameObject);
    }
}
}
}