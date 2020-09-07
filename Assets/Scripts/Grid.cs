//Code heavily based on CodeMonkey gird tutorial

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace FloppyDiscProjectGreen
{
namespace CombatSystem
{
public class Grid<TypeGridObject>
{
    public event EventHandler<OnValueChangeArgs> OnValueChange;
    public class OnValueChangeArgs : EventArgs{
        public int x;
        public int y;
    }
   private int rows;
   private int colls;
   private float cellSize;
   private Vector3 originPosition;
   private TypeGridObject[,] grid;
   private int debugFontSize = 3;
   public Grid(int rows, int colls, float cellSize, Vector3 originPosition, Func<Grid<TypeGridObject> ,int, int, TypeGridObject> createGridObject, bool debug = true)
   {
       this.colls = colls;
       this.rows = rows;
       this.originPosition = originPosition;
       this.cellSize = cellSize;

       grid = new TypeGridObject[colls, rows];
        for (int x = 0; x < colls; x++)
            for (int y = 0; y < rows; y++)
            {
                grid[x, y] = createGridObject(this, x, y);
            }

        if(debug)
        {
        TextMeshPro[,] debugText = new TextMeshPro[colls, rows];

        for (int x = 0; x < colls; x++)
            for (int y = 0; y < rows; y++)
            {
                debugText[x, y] = CreateDebugText(grid[x,y]?.ToString(), GetWorldPos(x, y) + new Vector3(cellSize, cellSize) * .5f, debugFontSize, Color.white);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 200f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 200f);
            }
        Debug.DrawLine(GetWorldPos(0, rows), GetWorldPos(colls, rows), Color.white, 200f);
        Debug.DrawLine(GetWorldPos(colls, rows), GetWorldPos(colls, 0), Color.white, 200f);
        
        OnValueChange += (object sender, OnValueChangeArgs eventArgs) => {debugText[eventArgs.x, eventArgs.y].text = grid[eventArgs.x, eventArgs.y].ToString();};
        }


   }

    public int GetRows()
    {
        return rows;
    }

    public int GetCollumns()
    {
        return colls;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void SetDebugFontSize(int size)
    {
        if(size > 0)
            debugFontSize = size;
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private Vector2Int GetXY(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - originPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }
    public void SetGridObject(int x, int y, TypeGridObject value)
     {
        if( x >= 0 && y >= 0 && x < colls && y < rows)
        {
            grid[x, y] = value;
            if(OnValueChange != null) OnValueChange(this, new OnValueChangeArgs{x = x, y = y}); 
        }
     }

     public void SetGridObject(Vector3 woldPos, TypeGridObject value)
     {
        Vector2Int xy = GetXY(woldPos);
        SetGridObject(xy.x, xy.y , value); 
     }
     
    public TypeGridObject GetGridObject(int x, int y)
     {
        if( x >= 0 && y >= 0 && x < colls && y < rows)
        {
            return grid[x, y];
        }else
            return default(TypeGridObject);
     }

     public TypeGridObject GetGridObject(Vector3 woldPos)
     {
        Vector2Int xy = GetXY(woldPos);
        return GetGridObject(xy.x, xy.y); 
     }

    public void TriggerGridObjectChange(int x, int y)
    {
        if(OnValueChange != null) OnValueChange(this, new OnValueChangeArgs{ x = x, y = y});
    }

    public IEnumerable<TypeGridObject> InOrderIteration()
    {
        for (int x = 0; x < colls; x++)
            for (int y = 0; y < rows; y++)
            {
                yield return grid[x, y];
            }
    }

    private static TextMeshPro CreateDebugText(string text, Vector3 position, int fontSize, Color color) {
            GameObject gameObject = new GameObject("DebugText");
            gameObject.AddComponent<TextMeshPro>();
            gameObject.transform.transform.position = position;
            TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;
            return textMesh;
        }
}
}
}