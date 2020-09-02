using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;
public class TestingGrid : MonoBehaviour
{
    public Grid<CombatSystem.GridObject> grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<CombatSystem.GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<CombatSystem.GridObject> g, int x, int y) => new CombatSystem.GridObject(g, x, y) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
