using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

public class GridCombatSystem : MonoBehaviour
{
    public Grid<CombatSystem.GridObject> grid;
    // Start is called before the first frame update
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;
    private CombatSystem.GridObject lastActiveCell;
    [SerializeField] private bool debug;
    void Start()
    {
        grid = new Grid<CombatSystem.GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<CombatSystem.GridObject> g, int x, int y) => new CombatSystem.GridObject(g, x, y), debug );

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

    void updateActiveCell(CombatSystem.GridObject currentActiveCell)
    {
        if(currentActiveCell == null) 
            activeCellGameObject.SetActive(false);
        else
        {
            activeCellGameObject.SetActive(true); //ToDO if there is a way to active state test if one if is faster then this
            activeCellGameObject.transform.position = currentActiveCell.GetCellPos();
            
        }
    }

    void updateIdleCell(CombatSystem.GridObject gridcell)
    {
        if(gridcell.getIdleCellSprite() == null) Destroy(gridcell.getIdleCellSprite());
        gridcell.setIdleCellSprite(Instantiate(idleCellSprite, gridcell.GetCellPos(), idleCellSprite.transform.rotation) as GameObject);
    }
}
