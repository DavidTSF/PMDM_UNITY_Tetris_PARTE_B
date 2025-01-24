using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private float lastFall = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!IsValidBoard())
		{
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame.
    // Implements all piece movements: right, left, rotate and down.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePiece(new Vector3(-1, 0, 0));
            
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePiece(new Vector3(1, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (IsValidBoard())
                UpdateBoard();
            else
                transform.Rotate(0, 0, 90);
            
        }else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            MovePiece(new Vector3(0, -1, 0));
            lastFall = Time.time;
        }
    }

    void MovePiece(Vector3 direction)
    {
        transform.position += direction;
        if (IsValidBoard()) // la bajada es válida
        {
            UpdateBoard();
        }
        else
            // No es válido la siguiente bajada --> quizás hay pieza?
        {
            transform.position -= direction;
            // revertir el mov. pieza si la posición no es válida
            // si el mov. que se intentó era hacia abajo. Los valores Vector3 indican hacia abajo.
            if (direction == new Vector3(0, -1, 0))
            {
                foreach (Transform child in transform)
                {
                    Vector2 v = Board.RoundVector2(child.position);
                    Board.grid[(int)v.x, (int)v.y].SetActive(true);
                }

                Board.DeleteFullRows();
                // Al tocar la superficie montón si hay lineas completadas las elimina.
                //obsolet!! FindObjectOfType<Spawner>().SpawnNext();
                //
                //Object.FindFirstObjectByType<Spawner>().SpawnNext();
                
               
                Object.FindFirstObjectByType<Spawner>().ActivateNextPiece();
                transform.position = new Vector3(5, 17, 0);
                gameObject.SetActive(false);
                
                enabled = false; // Deshabilitar el script para esta pieza
            }
        }
    }
    
    
    // TODO: Updates the board with the current position of the piece. 
    void UpdateBoard()
    {
        // First you have to loop over the Board and make current positions of the piece null.
        
        // Then you have to loop over the blocks of the current piece and add them to the Board.
        
        for (int y = 0; y < Board.h; ++y)
        {
            for (int x = 0; x < Board.w; ++x)
            {
                if (Board.grid[x, y].activeSelf && Board.grid[x, y].transform.parent == transform )
                {
                    Board.grid[x, y].SetActive(false); 
                    
                }
            }
        }
        foreach (Transform child in transform)
        {
            Vector2 v = Board.RoundVector2(child.position);
            //Board.grid[(int)v.x, (int)v.y].SetActive(true);
        }
    }

    // Returns if the current position of the piece makes the board valid or not
    bool IsValidBoard()
    {
        
        foreach (Transform child in transform)
        {
            Vector2 v = Board.RoundVector2(child.position);

            // Not inside Border?
            if (!Board.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Board.grid[(int)v.x, (int)v.y].activeSelf && Board.grid[(int)v.x, (int)v.y].transform.parent != transform)
                return false;
        } 
        
        return true;
    }
}
