using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using System;

public class GridCombatSystem : MonoBehaviour
{
    public event EventHandler<OnGridReadArgs> onGridReady;
    public class OnGridReadArgs : EventArgs
    {
        public Action onGridReadyAction;
    }

    private GridComplete grid;
    private Pathfinding pFgrid;
    // Start is called before the first frame update
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;
    private GridObject lastActiveCell;
    
    public bool gridRead = false;

    [SerializeField] private bool debug;
    [SerializeField] private  List<Vector2Int> unWalkableCells; 
    private GameObject[] charactersInFight;
    private GameCharacter player;
    void Start()
    {
        //grid = new Grid<GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), debug );
        grid = new GridComplete(10, 20, 1f, new Vector3(-13, -6), debug);
        grid.SetUnwalkable(unWalkableCells);
        //pFgrid = new Pathfinding(10, 20, 1f, new Vector3(-13, -6));
        SetUp();

        if(onGridReady != null) onGridReady(this, new OnGridReadArgs{onGridReadyAction = OnGridReadyAction });
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

    public void OnGridReadyAction()
    {
        gridRead = true;
    }

    // Update is called once per frame
    void Update()
    {
        var currentActiveCell = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        updateActiveCell(currentActiveCell);   
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<GridObject> path = grid.FindPath(new Vector3(-13, -6), mousePos);
            foreach(var node in path)
            {
                Instantiate(idleCellSprite, grid.GetGridObject(node.x(), node.y()).GetCellPos(), idleCellSprite.transform.rotation);
            }
        }  
        if(Input.GetMouseButtonDown(1))
        {
            player.MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

    void updateIdleCell(GridObject gridcell)
    {
        if(gridcell.getIdleCellSprite() == null) Destroy(gridcell.getIdleCellSprite());
        gridcell.setIdleCellSprite(Instantiate(idleCellSprite, gridcell.GetCellPos(), idleCellSprite.transform.rotation) as GameObject);
    }
}
