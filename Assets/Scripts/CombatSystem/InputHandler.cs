using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using FloppyDiscProjectGreen.Abilites;
using System;

public class InputHandler : MonoBehaviour
{
    public event EventHandler<OnPathChangedArgs> OnPlayerPathChanged;
    public class OnPathChangedArgs : EventArgs
    {
        public List<GridObject> newPath;
    }
    private GridComplete grid;
    [SerializeField] private GameCharacter player;
    [SerializeField] private AbilitesSystem  playerAbilitiesSystem;
    [SerializeField] private VisualiationHandler pathAndRadiousVisualiation;
    [SerializeField]private GridCombatSystem gridCombatSystem;

    private GridObject lastActiveCell;
    private GridObject lastPlayerCell;

    private bool ablityiVisualation = false; // to help with wit

    List<GridObject> playerPath;
    // Start is called before the first frame update
    void Start()
    {
        gridCombatSystem.onGridReady += GetGrid;
    }

    // Update is called once per frame

    void GetGrid()
    {
        grid = gridCombatSystem.GetGrid();
        lastActiveCell = grid.GetGridObject(0, 0);
        lastPlayerCell = grid.GetGridObject(player.transform.position);
    }

 void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var currentActiveCell = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var playerCell = grid.GetGridObject(player.transform.position);

        pathAndRadiousVisualiation.ActiveCellVisualization(currentActiveCell);
        if(gridCombatSystem.GetState() == GridCombatSystem.State.playerRound)
        {
            pathAndRadiousVisualiation.HandlePlayerRadiousVisualiation(currentActiveCell, playerCell);
            HandlePathFindingAndVisualiation(currentActiveCell, playerCell);
            HandleMouseInput(mousePos);
        }
        else
        {
            pathAndRadiousVisualiation.ClearVisualization();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerAbilitiesSystem.EquipAbility<EmpAbility>();
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
                playerPath = grid.FindPath(playerCell, currentActiveCell);
                OnPlayerPathChanged?.Invoke(this, new OnPathChangedArgs{newPath = playerPath});
            }
        }
    }

    void HandleMouseInput(Vector3 mousePos)
    {
        if(gridCombatSystem.GetState() == GridCombatSystem.State.playerRound)
        {
            if(Input.GetMouseButtonDown(0) && !ablityiVisualation)
            {
                playerAbilitiesSystem.VisualizeAbility(grid.GetGridObject(mousePos), AbilitesCode.A);
                ablityiVisualation = true;
                Debug.Log("1");
            }  
            else if(Input.GetMouseButtonDown(0) && ablityiVisualation)
            {
                playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.A, () => {});
                ablityiVisualation = false;
                Debug.Log("2");
            }  
            if(Input.GetMouseButtonDown(1))
            {
                if(playerPath != null && player.GetState() == GameCharacter.State.Idle)
                {
                    grid.GetGridObject(player.transform.position).SetObjectInTile(null);
                    player.MoveTo(playerPath);
                    grid.GetGridObject(playerPath[playerPath.Count-1].GetCellPos()).SetObjectInTile(player.gameObject);
                }
            }
        }
    }
}
