using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    public Transform playerPos;
    public GameObject aStar;
    Node player;

    GridScript grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = aStar.GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
        //player = grid.NodeFromWorldPoint(grid.player.position);
        Debug.Log(player);
        //player = grid.NodeFromWorldPoint(playerPos.position);
        //playerPos.position = player.worldPosition;
        Debug.Log(playerPos);

    }

    public void start()
    {
       
    }

    public void OnMouseOver()
    {

        List<Node> neighbours= grid.GetNeighbours(grid.NodeFromWorldPoint(grid.player.position));

        foreach (Node n in neighbours)
        {
            n.parentTile.GetComponentInChildren<Renderer>().material.color = Color.black;
        }
        
        Debug.Log(neighbours[0].worldPosition);
                 
    }
}
