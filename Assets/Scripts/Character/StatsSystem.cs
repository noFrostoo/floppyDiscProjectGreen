using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] int _strength;
    [SerializeField] int _armour;
    [SerializeField] int _stealth;
    [SerializeField] int _charisma;
    [SerializeField] int _MeleeAttackRadious = 1; //in cells
    [SerializeField] int _MeleeDamage = 30;
    [SerializeField] int _health = 100;
    [SerializeField] int _actionPoints = 100;
    [SerializeField] int _movmentPointsPerRound = 50;
    
    public int Charisma { get => _charisma; set {
        if(value >= 0)
            _charisma = value;
    }}

    public int Strenght { get => _strength; set {
        if(value >= 0)
            _strength = value;
    }}

    public int Armour { get => _armour; set {
        if(value >= 0)
            _armour = value;
    }}

    public int Stealth { get => _stealth; set {
        if(value >= 0)
            _stealth = value;
    }}

    public int MeleeDamge{ get => _stealth + _MeleeDamage; set{
        if(value >= 0)
            _MeleeDamage = value;
    }}

    public int MeleeAttackRadious{ get => _MeleeAttackRadious;}

    public int Health{ get => _health; set {
        if(value >= 0)
            _health = value;
    }}

    public int ActionPoints{ get => _actionPoints; set{
        if(value >= 0)
            _actionPoints = value;
    }}

    public int MovmentPointsPerRound{ get => _movmentPointsPerRound; set{
        if(value >= 0)
            _movmentPointsPerRound = value;
    }}
}
