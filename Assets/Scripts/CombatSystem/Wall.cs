using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private int _chancePointsDecrease;
    [SerializeField] private int _damageDecrease;
    public int ChancePointsDecrease{ get => _chancePointsDecrease;}
    public int DamageDecrase {get => _damageDecrease;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
