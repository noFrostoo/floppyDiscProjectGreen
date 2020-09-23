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
    [SerializeField] private GridCombatSystem gridCombatSystem;
    private RangeCombat rangeCombatPlayer;
    
    private GridObject lastActiveCell;
    private GridObject lastPlayerCell;

    private Action actionToTakeOnMouseClik = null;
    private bool ablityiVisualation = false; // to help with wit
    Vector3 mousePos;
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
        rangeCombatPlayer = player.rangeCombat;
    }

 void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        DebugInput();
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
            if(Input.GetMouseButtonDown(0))
            {
                if(actionToTakeOnMouseClik != null)
                {
                    actionToTakeOnMouseClik();
                    actionToTakeOnMouseClik = null;   
                }   else
                {
                    Debug.Log("NO ACTION SELECTED");
                }
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

    public void SetMeleeAttack()
    {
        actionToTakeOnMouseClik = MeleeAttack;
    }
    void MeleeAttack()
    {
        player.MeleeAttack(grid.GetGridObject(mousePos));
    }

    public void SetRangeAttack()
    {
        actionToTakeOnMouseClik = RangeAttack;
    }

    void RangeAttack()
    {
        rangeCombatPlayer.Fire(grid.GetGridObject(mousePos).GetObjectInCell().GetComponent<GameCharacter>());
    }

    void SetAbility(Action action, AbilitesCode code)
    {
        actionToTakeOnMouseClik = action;
        playerAbilitiesSystem.VisualizeAbility(grid.GetGridObject(mousePos), code);
    }

    public void SetAbilityA()
    {
        SetAbility(AbilityA, AbilitesCode.A);
    }

    public void SetAbilityB()
    {
        SetAbility(AbilityB, AbilitesCode.B);
    }

    public void SetAbilityC()
    {
        SetAbility(AbilityC, AbilitesCode.C);
    }

    public void SetAbilityD()
    {
        SetAbility(AbilityD, AbilitesCode.D);
    }

    public void SetAbilityE()
    {
        SetAbility(AbilityE, AbilitesCode.E);
    }

    void AbilityA()
    {
        playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.A, () => {});
    }

    void AbilityB()
    {
        playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.B, () => {});
    }

    void AbilityC()
    {
        playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.C, () => {});
    }

    void AbilityD()
    {
        playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.D, () => {});
    }

    void AbilityE()
    {
        playerAbilitiesSystem.TrigerAbility(grid.GetGridObject(mousePos), AbilitesCode.E, () => {});
    }

    public void CycleFireMode()
    {
        switch(rangeCombatPlayer.GetCurrentWeapon().GetFireMode())
        {
            case FireMode.Semi_Automatic:
                rangeCombatPlayer.ChangeFireMode(FireMode.Burst);
                break;
            case FireMode.Burst:
                rangeCombatPlayer.ChangeFireMode(FireMode.Automatic);
                break;
            case FireMode.Automatic:
                rangeCombatPlayer.ChangeFireMode(FireMode.Semi_Automatic);
                break;
            case FireMode.Custom:
                // to do
                break;
            default:
                //toDO
                break;
        }
    }

    void DebugInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerAbilitiesSystem.EquipAbility<EmpAbility>();
            playerAbilitiesSystem.EquipAbility<BurnWiringAbility>(AbilitesCode.B);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            gridCombatSystem.TriggerStateChange();
        }
    }
}
