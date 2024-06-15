using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Grid<TGridObject >
{

    private Vector3 originPosition;
    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridArray;
    

    
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
               
                Debug.DrawLine(GetWorldPosition(x - .5f, y - .5f), GetWorldPosition(x - .5f, y + .5f), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x - .5f, y - .5f), GetWorldPosition(x + .5f, y - .5f), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0 - .5f, height - .5f), GetWorldPosition(width - .5f, height - .5f), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width - .5f, 0 - .5f), GetWorldPosition(width - .5f, height - .5f), Color.white, 100f);
    }




    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }


    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize;
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }
}


