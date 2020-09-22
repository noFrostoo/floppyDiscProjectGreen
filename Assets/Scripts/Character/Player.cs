using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;
using FloppyDiscProjectGreen.Abilites;

public class Player : MonoBehaviour
{
    public static GameCharacter GameCharacter;
    public static AbilitesSystem Abilites;
    public static StatsSystem Stats;
    public static Player Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;    
        GameCharacter = GetComponent<GameCharacter>();
        Abilites = GetComponent<AbilitesSystem>();
        Stats = GetComponent<StatsSystem>();
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
