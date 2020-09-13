using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class VisualiationHandler : MonoBehaviour
{
    [SerializeField] private GameCharacter player;
    GridComplete grid;
    [SerializeField] private GridCombatSystem gridCombatSystem;
    private bool mouseOutofRadious;
    public GameObject idleCellSprite;
    public GameObject activeCellSprite;

    private GameObject activeCellGameObject;

    private GameObject[] radiousVisualizationnPool;
    private GameObject[] pathVisualizationPool;
    // Start is called before the first frame update
    void Start()
    {
        SetUpPathVisualiation(20);
        SetUpRadiousVisualiation(100);
        gridCombatSystem.onGridReady += SetUp;
        gridCombatSystem.OnPathChanged += HandlePathVisualization;

        activeCellGameObject = Instantiate(activeCellSprite, new Vector3(0,0,0), activeCellSprite.transform.rotation);
        activeCellGameObject.SetActive(false);
    }
    void SetUp()
    {
        grid = gridCombatSystem.GetGrid();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayerAndGrid(GameCharacter player, GridComplete grid)
    {
        this.grid = grid;
        this.player = player;
    }

     void SetUpRadiousVisualiation(int amount) //!!
    {
        radiousVisualizationnPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            radiousVisualizationnPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            radiousVisualizationnPool[i].name = "RadiousCell";
            radiousVisualizationnPool[i].SetActive(false);
        }
    }

    void SetUpPathVisualiation(int amount)  //!!
    {
        pathVisualizationPool = new GameObject[amount];
        for(int i = 0; i < amount; i++)
        {
            pathVisualizationPool[i] = Instantiate(idleCellSprite, Vector3.zero, idleCellSprite.transform.rotation);
            pathVisualizationPool[i].name = "PathCell";
            pathVisualizationPool[i].SetActive(false);
        }
    }

    public void HandleRadiousVisualiation(GridObject currentActiveCell, GridObject playerCell)
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
}
}
}