using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using TMPro;
using System;

public class GameCharacter : MonoBehaviour
{
    [SerializeField] private GridCombatSystem gridCS;
    [SerializeField] private int health = 100;
    private HealthSystem healthSystem;
    private TextMeshPro healthText;
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
        StartCoroutine(followPath(path));
    }

    IEnumerator followPath(List<GridObject> path)
    {
        foreach(var pathNode in path)
        {
            transform.position = pathNode.GetCellPos();
            yield return new WaitForSeconds(0.1f);
        }
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
