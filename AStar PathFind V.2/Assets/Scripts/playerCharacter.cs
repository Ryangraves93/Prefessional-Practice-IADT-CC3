using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    public Transform playerPos;
    public GameObject aStar;
    Node player;
    public Material boardMaterial;

    GridScript grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = aStar.GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
        grid.NodeFromWorldPoint(playerPos.position);

    }

    void displayMovableDirection (bool isHovered)
    {
        List<Node> neighbours = grid.GetNeighbours(grid.NodeFromWorldPoint(grid.player.position));

        if (isHovered == true)
        {
            foreach (Node n in neighbours)
            {
                n.parentTile.GetComponentInChildren<Renderer>().material.color = Color.black;
            }
        }
        if(isHovered == false)
        {
            foreach (Node n in neighbours)
            {
                n.parentTile.GetComponentInChildren<Renderer>().material.color = Color.white;
            }
        }
    }
    public void OnMouseOver()
    {

        displayMovableDirection(true);

                 
    }

    private void OnMouseExit()
    {
        displayMovableDirection(false);
    }
}
