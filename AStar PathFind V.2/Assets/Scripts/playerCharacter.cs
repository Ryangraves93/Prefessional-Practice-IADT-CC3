using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    public Transform playerPos;
    public GameObject aStar;
    Node player;
    public Material boardMaterial;
    public GameObject enemies;
    public int firstCol = 0;
    enemyScript[] enemyList;
    GridScript grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = aStar.GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
        grid.NodeFromWorldPoint(playerPos.position);
        enemyList = enemies.GetComponentsInChildren<enemyScript>();

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyScript enemy = collision.gameObject.GetComponent<enemyScript>();
            PathFinding path = aStar.GetComponent<PathFinding>();

            if (enemy.dir != path.dir * -1)
            {
                Debug.Log("Kill");
                collision.gameObject.SetActive(false);
            }

            else
            {
                Debug.Log("player die");
            }
        }

        if (collision.gameObject.tag == "dest")
        {
            if (firstCol > 0)
            {
                foreach (enemyScript emy in enemyList)
                {
                    emy.enemyStep();
                }
            }
            firstCol++;
        }

    }


}
