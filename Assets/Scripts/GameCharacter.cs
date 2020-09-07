using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using TMPro;
public class GameCharacter : MonoBehaviour
{
    [SerializeField] private GridCombatSystem gridCS;
    [SerializeField] private int health = 100;
    
    private TextMeshPro healthText;
    [SerializeField] private Vector3 healthTextOffset = new Vector3(0, 0.65f, 0);
    [SerializeField] private int healthTextFontSize = 4;
    [SerializeField] private Color healthTextColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        gridCS.onGridReady += Grid_SnapToCell;
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
        healthText.SetText(health.ToString());
        healthText.fontSize = healthTextFontSize;
        healthText.color = healthTextColor;
        healthText.alignment = TextAlignmentOptions.Center;
        healthText.GetComponent<MeshRenderer>().sortingOrder = 5000;
    }

    void UpdateHealthText()
    {
        healthText.SetText(health.ToString());
        healthText.transform.position = transform.position + healthTextOffset;
    }

    void Grid_SnapToCell(object sender, GridCombatSystem.OnGridReadArgs eventArgs)
    {
        SnapToCell();
    }

    void SnapToCell()
    {
        transform.position = gridCS.GetGrid().GetGridObject(transform.position).GetCellPos();
    }

    public void MoveTo(Vector3 moveToPosition)
    {
        GridObject cell = gridCS.GetGrid().GetGridObject(moveToPosition);
        if(cell.isWalkable())
            transform.position = cell.GetCellPos();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
