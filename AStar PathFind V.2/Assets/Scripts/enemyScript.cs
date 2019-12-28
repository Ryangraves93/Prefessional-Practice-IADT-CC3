using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject aStar; // grid references
    GridScript grid;
    public int dir; // direction of movement
    public float speed = 20f;
    public Vector3 destination; // destination object
    bool onRun = true; // run once regardless of void start
    /*public Node newHilightNode; // highlight variable
    public Transform weapon;*/

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


        // align to correct rotation
        if (dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        if (dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if (dir == 2)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
  
        }
        if (dir == -2)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);

        }
    }

    public void enemyStep()
    {
        //NORTH
        if (dir == 1)
        {

            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + grid.nodeDiameter); //get next position
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            

            if (newNode.walkable && newPos.z <= (grid.gridSizeX/2) * grid.nodeDiameter) // node is walkable and is inside the grid
            {
                destination = newNode.worldPosition; // place destination on new node
            }

             else
            {
                dir *= -1;
                enemyStep(); //reverse direction and call function again
            }


        }

     //WEST
        if (dir == -2)
        {
            Vector3 newPos = new Vector3(transform.position.x - grid.nodeDiameter, transform.position.y, transform.position.z);
            Node newNode = grid.NodeFromWorldPoint(newPos);

            if (newNode.walkable && newPos.x >= -1 * (grid.gridSizeX / 2) * grid.nodeDiameter)
            {
                destination = newNode.worldPosition;
            }
            else
            {
                dir = dir * -1;
                enemyStep();
            }
        }
      //SOUTH
        if (dir == -1)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - grid.nodeDiameter);
            Node newNode = grid.NodeFromWorldPoint(newPos);

            if (newNode.walkable && newPos.z >= -1*((grid.gridSizeX / 2) * grid.nodeDiameter))
            {
                destination = newNode.worldPosition;
            }
            else
            {
                dir = dir * -1;
                enemyStep();
            }
        }
        //EAST
        if (dir == 2)
        {
            Vector3 newPos = new Vector3(transform.position.x + grid.nodeDiameter, transform.position.y, transform.position.z); 
            Node newNode = grid.NodeFromWorldPoint(newPos); 

            if (newNode.walkable && newPos.x <= ((grid.gridSizeX / 2) * grid.nodeDiameter))
            {
                destination = newNode.worldPosition;
            }
            else
            {
                dir = dir * -1;
                enemyStep();
            }
        }
    }
}
