using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
    [SerializeField] Button nextRoundButton;
    // Start is called before the first frame update
    void Start()
    {
        player.OnDoneMoving += UpdateMovmentPointsText;
        player.OnDoneMoving += UpdateAPText;
        player.OnDoneAttacking += UpdateAPText;
        player.OnReady += UpdateAll;
        gridCombatSystem.OnStateChange += UpdateButton;
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

    void UpdateButton(object sender, EventArgs e)
    {
        if(gridCombatSystem.GetState() == GridCombatSystem.State.playerRound)
        {
            nextRoundButton.gameObject.SetActive(true);
        }
        else if(gridCombatSystem.GetState() == GridCombatSystem.State.enemyRound)
        {
            nextRoundButton.gameObject.SetActive(false);
        }
    }
}
}
}