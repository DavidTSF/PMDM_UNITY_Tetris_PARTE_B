using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] pieces;
    private List<GameObject> piecesPool = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject prefab in pieces)
        {
            GameObject piece = Instantiate(prefab, transform.position, Quaternion.identity);
            piece.SetActive(false);
            piecesPool.Add(piece);
        }

        //SpawnNext();
        ActivateNextPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNext()
    {
        int i = Random.Range(0, pieces.Length);
        //Vector3 initialPos = new Vector3(5, 15, 0);
        
        
        Instantiate(pieces[i], transform.position, Quaternion.identity);
    }
    
    public void ActivateNextPiece()
    {
        int randomIndex = Random.Range(0, piecesPool.Count);
        GameObject piece = piecesPool[randomIndex];

        while (piece.activeInHierarchy)
        {
            randomIndex = Random.Range(0, piecesPool.Count);
            piece = piecesPool[randomIndex];
        }
        piece.transform.position = transform.position;
        piece.SetActive(true);
        piece.GetComponent<Piece>().enabled = true;
    }

}
