using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using TMPro;
using System;

public class GameCharacter : MonoBehaviour
{
    private const int DIAGONAL_MOVE_COST = 14;
    private const int STRAIGH_MOVE_COST = 10;
    [SerializeField] private GridCombatSystem gridCS;
    [SerializeField] private int health = 100;
    private HealthSystem healthSystem;
    private TextMeshPro healthText;
    [SerializeField] public int actionPoints = 100;
    [SerializeField] public int movmentPointsPerRound = 50;
    [SerializeField] private Vector3 healthTextOffset = new Vector3(0, 0.65f, 0);
    [SerializeField] private int healthTextFontSize = 4;
    [SerializeField] private Color healthTextColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = new HealthSystem(health);
        healthSystem.OnDeath += Death;
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
    }
    
    public bool CellInRadious(GridObject cell)
    {
        if(cell == null) return false;
        return CalculateDistance(gridCS.GetGrid().GetGridObject(transform.position), cell) <= movmentPointsPerRound;
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

}
