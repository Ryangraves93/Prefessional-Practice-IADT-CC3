using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{

    public GameObject aStar;
    Node player;

    GridScript grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = aStar.GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
        //player = grid.NodeFromWorldPoint(grid.player.position);
        Debug.Log(player);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        List<Node> neighbours= grid.GetNeighbours(grid.NodeFromWorldPoint(grid.player.position));
        Debug.Log(neighbours[0].worldPosition);
                 
    }
}
