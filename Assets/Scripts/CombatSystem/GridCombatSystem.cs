using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FloppyDiscProjectGreen.Abilites;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class GridCombatSystem : MonoBehaviour
{
    public static GridCombatSystem Instance;
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
    //List<GridObject> path = null;
    public bool gridRead = false;
    [SerializeField] State currentState;

    [SerializeField] private bool debug;
    [SerializeField] private  List<Vector2Int> unWalkableCells; 
    static public bool debugS;
    private GameObject[] charactersInFight;
    private GameObject[] enemies;
    private GameCharacter player;
    private AbilitesSystem playerAbilitiesSystem;
    private VisualiationHandler pathAndRadiousVisualiation;
    
    void Start()
    {
        Instance = this;
        debugS = debug;
        grid = new GridComplete(10, 20, 1f, new Vector3(-13, -6), debug);
        grid.SetUnwalkable(unWalkableCells);
        pathAndRadiousVisualiation = GetComponent<VisualiationHandler>();
        currentState = State.playerRound;
        OnStateChange += HandleStateChange;
        SetUp();
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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacter>();
        playerAbilitiesSystem = player.GetComponent<AbilitesSystem>();
        charactersInFight = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

    void Update()
    {
        if(debug) TriggerGridObjectChangeForWholeGrid();
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

    void updateIdleCell(GridObject gridcell)
    {
        if(gridcell.getIdleCellSprite() == null) Destroy(gridcell.getIdleCellSprite());
        gridcell.setIdleCellSprite(Instantiate(idleCellSprite, gridcell.GetCellPos(), idleCellSprite.transform.rotation) as GameObject);
    }
}
}
}