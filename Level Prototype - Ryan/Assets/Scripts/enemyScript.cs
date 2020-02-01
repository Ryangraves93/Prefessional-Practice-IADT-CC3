using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{

    public enum direction{
        North,
        South,
        East,
        West,
        None,
    };
    public bool isSquareEnemy = true;

    public GameObject aStar; // grid references
    GridScript grid;
    public direction dir; // direction of movement
    public float speed = 20f;
    public Vector3 destination; // destination object
    bool onRun = true; // run once regardless of void start
    /*public Node newHilightNode; // highlight variable
    public Transform weapon;*/
    public bool isMovingEnemy = true;

    private void Awake()
    { //Assign grid as a reference to our gridscript class
        aStar = GameObject.Find("A*");
        grid = aStar.GetComponent<GridScript>();
        //weapon = transform.GetChild(1);
    }


    // Update is called once per frame
    void Update()
    {



        if (onRun == true)
        {
            Node snap = grid.NodeFromWorldPoint(transform.position);
            transform.position = snap.worldPosition;
            destination = transform.position;
            onRun = false;
        }


       
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);

/*      Vector3 highlight = new Vector3(weapon.position.x, weapon.position.y, weapon.position.z);
        Node highlightNode = grid.NodeFromWorldPoint(highlight);
        if (highlightNode.walkable)
        {
            highlightNode.parentTile.GetComponentInChildren<Renderer>().material.color = Color.red;
        }*/


        switch (dir)
        {
            case direction.North:
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                }
            case direction.South:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }
            case direction.East:
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                }
            case direction.West:
                {
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                }
        }
     
    }

    public void enemyStep()
    {

        //NORTH
        switch (dir)
        {
            case direction.North:
                {
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + grid.nodeDiameter); //get next position
                    Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

                    if (newNode.walkable && newPos.z <= (grid.gridSizeX / 2) * grid.nodeDiameter * 1.01) // node is walkable and is inside the grid
                    {
                        destination = newNode.worldPosition; // place destination on new node
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.East : direction.South;
                        //dir = direction.South;
                        //enemyStep(); //reverse direction and call function again
                    }
                    break;
                }
            case direction.South:
                {
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - grid.nodeDiameter);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.z >= -1 * ((grid.gridSizeX / 2) * grid.nodeDiameter * 1.01))
                    {
                        destination = newNode.worldPosition;
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.West : direction.North;
                        //dir = direction.North;
                        //enemyStep();
                    }
                    break;
                }
            case direction.East:
                {
                    Vector3 newPos = new Vector3(transform.position.x + grid.nodeDiameter, transform.position.y, transform.position.z);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.x <= ((grid.gridSizeX / 2) * grid.nodeDiameter * 1.01))
                    {
                        destination = newNode.worldPosition;

                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.South : direction.West;
                        //dir = direction.West;
                        //enemyStep();

                    }
                    break;
                }
            case direction.West:
                {
                    Vector3 newPos = new Vector3(transform.position.x - grid.nodeDiameter, transform.position.y, transform.position.z);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.x >= -1 * (grid.gridSizeX / 2) * grid.nodeDiameter * 1.01)
                    {
                        destination = newNode.worldPosition;
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.South : direction.East;
                        //dir = direction.East;
                        //enemyStep();
                    }
                    break;
                }
            case direction.None:
                {
                    transform.Rotate(0, 90, 0);
                }
                break;
        }
    } 
}
