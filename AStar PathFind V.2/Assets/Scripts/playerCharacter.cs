using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCharacter : MonoBehaviour
{
    public Transform playerPos; // reference to player position
    public GameObject aStar; // reference to grid object
    public GameObject enemies; // reference to enemies container
    bool firstCol = false; // bool to prevent initial snap to grid counting as a collision
    enemyScript[] enemyList; // array of enemies
    GridScript grid; // grid reference
    public GameObject endGameOverlay; //UI components
    public GameObject nextLevel;
    public GameObject loseText;
    public GameObject winText;

    int enemyCount; // number of enemies

    void Start()
    {
        grid = aStar.GetComponent<GridScript>();
        enemyList = enemies.GetComponentsInChildren<enemyScript>(); // script references
        enemyCount = enemyList.Length; //initialize number of enemies
    }

    private void Update()
    {
        foreach (GameTile n in grid.grid)
        {
            if (n.tileMap.walkable)
            {
                n.tileMap.parentTile.GetComponentInChildren<Renderer>().material.color = Color.white; // each frame set all to base colour
            }
            else
            {
                n.tileMap.parentTile.GetComponentInChildren<Renderer>().material.color = Color.blue; // each frame set all to base colour
            }
        }
    }

/*    void displayMovableDirection (bool isHovered)
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
    }*/
/*    public void OnMouseOver()
    {

        displayMovableDirection(true);

                 
    }

    private void OnMouseExit()
    {
        displayMovableDirection(false);
    }*/

    void OnCollisionEnter(Collision collision) // collision logic
    {
        if (collision.gameObject.tag == "Enemy")
        {
            /*enemyScript enemy = collision.gameObject.GetComponent<enemyScript>();
            PathFinding path = aStar.GetComponent<PathFinding>();*/

            /*if (enemy.dir != path.dir * -1)
            {
                Debug.Log("Kill");
                collision.gameObject.SetActive(false);
            }

            else
            {
                Debug.Log("player die");
            }*/

            collision.gameObject.SetActive(false); // set enemy inactive

            enemyCount -=1; // reduce number of enemies

            if(enemyCount <= 0) //if no enemies left display win UI components
            {
                endGameOverlay.SetActive(true);
                nextLevel.SetActive(true);
                winText.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "weapon") // display lose UI components
        {
           gameObject.SetActive(false);
           endGameOverlay.SetActive(true);
           loseText.SetActive(true);
        }

        if (collision.gameObject.tag == "dest") // on arrival at destination object each enemy takes one step
        {
            if (firstCol == true) // prevents initial snap to grid causing an enemy step
            {
                
                foreach (enemyScript emy in enemyList)
                {
                    
                    emy.enemyStep();   

                }
            }
            firstCol = true;
        }

    }


}
