using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloppyDiscProjectGreen.CombatSystem;

public class TestingGrid : MonoBehaviour
{
    public Grid<GridObject> grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<GridObject>(10, 20, 1f, new Vector3(-13, -6), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
