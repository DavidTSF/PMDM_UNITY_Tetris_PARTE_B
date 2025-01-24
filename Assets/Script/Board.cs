using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    public static GameObject[,] grid = createGrid(w, h);

    public static GameObject[,] createGrid(int w, int h)
    {
        GameObject[,] grid = new GameObject[w, h];

        for (int y = 0; y < h ; y++)
        {
            for (int x = 0; x < w; x++)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<SpriteRenderer>();
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/block");
                gameObject.transform.position = new Vector3(x, y, 0);
                gameObject.SetActive(false);
                grid[x, y] = gameObject;
                
            }
        }
    
        
        return grid;
    }
    
    
    // Rounds Vector2 so does not have decimal values
    // Used to force Integer coordinates (without decimals) when moving pieces
    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    // TODO: Returns true if pos (x,y) is inside the grid, false otherwise
    public static bool InsideBorder(Vector2 pos)
    {
        if (pos.x < 0 || pos.x >= w || pos.y < 0 || pos.y >= h)
        {
            return false;
        }
        return true;
    }

    // TODO: Deletes all GameObjects in the row Y and set the row cells to null.
    // You can use Destroy function to delete the GameObjects.
    public static void DeleteRow(int y)
    {
        for (int i = 0; i < w; i++)
        {
            //Destroy(grid[i, y]);
            grid[i, y].SetActive(false);
        }
    }

    // TODO: Moves all gameobject on row Y to row Y-1
    // 2 thing change:
    //  - All GameObjects on row Y go from cell (X,Y) to cell (X,Y-1)
    //  - Changes the GameObject transform position Gameobject.transform.position += new Vector3(0, -1, 0).
    public static void DecreaseRow(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y].activeSelf)
            {
                //grid[i, y].transform.position += new Vector3(0, -1, 0);
                grid[i, y-1].SetActive(grid[i, y].activeSelf);
                grid[i, y].SetActive(false);
                //grid[i, y] = null;
            }
        }
    }

    // TODO: Decreases all rows above Y
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h ; i++)
        {
            DecreaseRow(i);
        }
    }

    // TODO: Return true if all cells in a row have a GameObject (are not null), false otherwise
    public static bool IsRowFull(int y)
    {
        for (int i = 0; i < w; i++)
        {
            if (grid[i, y].activeSelf == false)
            {
                return false;
            }
        }
        return true;
    }

    // Deletes full rows
    public static void DeleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                Debug.Log("Fila " + y);
                DeleteRow(y );
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }

}
