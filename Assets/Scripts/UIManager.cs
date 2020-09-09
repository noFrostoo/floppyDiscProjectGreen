using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class UIManager : MonoBehaviour
{
    [SerializeField] GridCombatSystem gridCombatSystem;
    [SerializeField] GameCharacter player;
    [SerializeField] TextMeshProUGUI APText;
    [SerializeField] TextMeshProUGUI MovementPointText;
    // Start is called before the first frame update
    void Start()
    {
        player.OnDoneMoving += UpdateMovmentPointsText;
        player.OnDoneMoving += UpdateAPText;
        player.OnDoneAttacking += UpdateAPText;
        player.OnReady += UpdateAll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateAPText(object sender, EventArgs e)
    {
        APText.SetText("ActionPoints: " + player.GetCurrentActionPoints());
    }
    void UpdateMovmentPointsText(object sender, EventArgs e)
    {
        MovementPointText.SetText("MovmentPoint: " + player.GetCurrentActionPoints());
    }

    void UpdateAll(object sender, EventArgs e)
    {
        UpdateAPText(this, EventArgs.Empty);
        UpdateMovmentPointsText(this, EventArgs.Empty);
    }
}
}
}