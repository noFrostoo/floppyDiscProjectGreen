using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using TMPro;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class GameCharacter : MonoBehaviour
{
     public enum State
    {
        Moving,
        Attacking,
        Idle,
    }
    public event EventHandler OnDoneMoving;
    public event EventHandler OnDoneAttacking;
    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGH_MOVE_COST = 10;
    [SerializeField] private GridCombatSystem gridCS;
    [SerializeField] private int health = 100;
    private HealthSystem healthSystem;
    private TextMeshPro healthText;
    public bool moving;
    [SerializeField] State currentState;
    [SerializeField] private int MeleeAttackRadious = 1; //in cells
    [SerializeField] private int MeleeDamage = 30;
    [SerializeField] public int actionPoints = 100;
    [SerializeField] public int movmentPointsPerRound = 50;
    [SerializeField] private Vector3 healthTextOffset = new Vector3(0, 0.65f, 0);
    [SerializeField] private int healthTextFontSize = 4;
    [SerializeField] private Color healthTextColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        healthSystem = new HealthSystem(health);
        healthSystem.OnDeath += Death;
        OnDoneMoving += DoneMoving;
        gridCS.onGridReady += SnapToCell;
        SetUpHealtText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthText();
    }

    void SetUpHealtText()
    {
        GameObject healthTextHolder = new GameObject("HealthText" + name, typeof(GameObject)) as GameObject;
        healthTextHolder.transform.position = transform.position + healthTextOffset;
        healthTextHolder.AddComponent<TextMeshPro>();
        healthText = healthTextHolder.GetComponent<TextMeshPro>();
        healthText.SetText(healthSystem.GetHealth().ToString());
        healthText.fontSize = healthTextFontSize;
        healthText.color = healthTextColor;
        healthText.alignment = TextAlignmentOptions.Center;
        healthText.GetComponent<MeshRenderer>().sortingOrder = 5000;
    }

    void UpdateHealthText()
    {
        healthText.SetText(healthSystem.GetHealth().ToString());
        healthText.transform.position = transform.position + healthTextOffset;
    }

    void SnapToCell()
    {
        transform.position = gridCS.GetGrid().GetGridObject(transform.position).GetCellPos();
        gridCS.GetGrid().GetGridObject(transform.position).SetObjectInTile(gameObject);
    }

    public void MoveTo(Vector3 moveToPosition)
    {
        List<GridObject> path = gridCS.GetGrid().FindPath(transform.position, moveToPosition);
        MoveTo(path);
    }

    public void MoveTo(List<GridObject> path)
    {
        if(path == null) return;
        int distanceToMove =  CalculateDistance(path[0], path[path.Count-1]);
        if( distanceToMove <= movmentPointsPerRound)
        {
            actionPoints -= movmentPointsPerRound;
            currentState = State.Moving;
            StartCoroutine(followPath(path));
        }
    }

    private int CalculateDistance(GridObject pn1, GridObject pn2)
    {
        int discX = Math.Abs(pn1.x() - pn2.x());
        int discY = Math.Abs(pn1.y() - pn2.y());
        int reamaing = Math.Abs(discX - discY);
        return DIAGONAL_MOVE_COST*Math.Min(discX, discY) + STRAIGH_MOVE_COST*reamaing;
    }

    IEnumerator followPath(List<GridObject> path)
    {
        foreach(var pathNode in path)
        {
            transform.position = pathNode.GetCellPos();
            yield return new WaitForSeconds(0.1f);
        }
        OnDoneMoving?.Invoke(this, EventArgs.Empty);
    }
    
    public bool CellInRadious(GridObject cell)
    {
        if(cell == null) return false;
        return CalculateDistance(gridCS.GetGrid().GetGridObject(transform.position), cell) <= movmentPointsPerRound;
    }

    public bool InRadious(Vector3 position)
    {
        if(position == null) return false;
        GridObject cell =  gridCS.GetGrid().GetGridObject(position);
        GridObject playerCell = gridCS.GetGrid().GetGridObject(transform.position);
        return CalculateDistance(playerCell, cell) <= movmentPointsPerRound;
    }
    
    public bool InMeleeAttackRadious(Vector3 position)
    {
        GridObject cell = gridCS.GetGrid().GetGridObject(position);
        GridObject playerCell = gridCS.GetGrid().GetGridObject(transform.position);
        return InMeleeAttackRadious(playerCell, cell);
    }

    public bool InMeleeAttackRadious(GridObject playerCell, GridObject cell)
    {
        int discX = Math.Abs(cell.x() - playerCell.x());
        int discY = Math.Abs(cell.y() - playerCell.y());
        if(discX <= MeleeAttackRadious && discY <= MeleeAttackRadious) return true;
        if(discX == discY && discX <= MeleeAttackRadious) return true;
        return false;
    }
    public int MaxCellMovmentRadious()
    {
        return movmentPointsPerRound/STRAIGH_MOVE_COST;
    }

    public void TakeDamage(int amount)
    {
        healthSystem.TakeDamage(amount);
    }

    private void Death(object sender, EventArgs e)
    {
        Debug.Log("The " + name + "has died");
    }

    public void MeleeAttack(GameCharacter target)
    {
        if(target == null) return;
        if(target.name == name) return;
        Debug.Log(target.name);
        if(InMeleeAttackRadious(target.transform.position))
            target.TakeDamage(MeleeDamage);
        else
            Debug.Log("Out of Range");
    }

    public void MeleeAttack(GridObject targetCell)
    {
        if(targetCell == null) return;
        if(targetCell.GetObjectInCell() == null) return;
        if(targetCell.GetObjectInCell().name == name) return;
        Debug.Log(targetCell.GetObjectInCell().name);
        if(InMeleeAttackRadious(gridCS.GetGrid().GetGridObject(transform.position), targetCell))
            targetCell.AttactObjectInTile(MeleeDamage);
        else
            Debug.Log("Out of Range");
    }

    public void RangeAttack(GameCharacter target)
    {

    }

    public State GetState()
    {
        return currentState;
    }

    private void DoneMoving(object sender, EventArgs e)
    {
        currentState = State.Idle;
    }
}
}
}