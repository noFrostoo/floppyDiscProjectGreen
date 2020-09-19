using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] int _strength;
    [SerializeField] int _armour;
    [SerializeField] int _stealth;
    [SerializeField] int _charisma;

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


}
