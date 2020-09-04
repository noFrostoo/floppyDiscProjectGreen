using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;

public class GridCombatSystem : MonoBehaviour
{
    public Grid<GridObject> grid;
    // Start is called before the first frame update
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;
    private GridObject lastActiveCell;
    [SerializeField] private bool debug;
    void Start()
    {
        grid = new Grid<GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), debug );

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
    }
    // Update is called once per frame
    void Update()
    {
        var currentActiveCell = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        updateActiveCell(currentActiveCell);     
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
