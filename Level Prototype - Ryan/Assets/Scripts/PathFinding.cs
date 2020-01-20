using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //bool tileSelected = false;

    public float speed = 50f; //player speed
    public Transform player; // reference to player
    public bool moveAllowed = true; // switch for player movement
    public int dir; // variable for North South East West
    public Vector3 destination; 
    public GameObject destinationObject; // position and object for controlling the players movement
    public GameObject enemies; // object containing enemies
    enemyScript[] enemyList; // array of enemies
    GridScript grid; //Grid reference

    private void Start()
    {
        Node snap = grid.NodeFromWorldPoint(player.position); 
        
        destination = player.position; // snaps player and destination object to nearest grid center
    }
    private void Awake()
    {

        enemyList = enemies.GetComponentsInChildren<enemyScript>();
        grid = GetComponent<GridScript>(); //Assign references to grid and enemies
    }

    public void Update()
    {
        //movement in 4 directions. passes  direction to function
        if (Input.GetKeyDown(KeyCode.W) && moveAllowed == true)
        {
            dir = 1;
           selectTile(1,0);
        }
        if (Input.GetKeyDown(KeyCode.D) && moveAllowed == true)
        {
            dir = 2;
            selectTile(2, 0);
        }
         if (Input.GetKeyDown(KeyCode.S) && moveAllowed == true)
        {
            
            dir = -1;
            selectTile(-1, 0);
        }
         if (Input.GetKeyDown(KeyCode.A) && moveAllowed == true)
        {
            dir = -2;
            selectTile(-2,0);
        }

         //rotate player to appropriate direction
        if (dir == 1)
        {
            player.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (dir == -1)
        {
            player.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (dir == 2)
        {
            player.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (dir == -2)
        {
            player.rotation = Quaternion.Euler(0, 90, 0);
        }

        //player is constantly moving towards the destination object at a fixed speed
        player.position = Vector3.MoveTowards(player.position, destination, Time.deltaTime * speed);
        destinationObject.transform.position = destination;

        //player movement is allowed only when player and destination are in same position
        if (Vector3.Distance(player.position, destination) < 0.01f)
        {
            moveAllowed = true;
        }
    }

    public void selectTile(int dir, int stepSize) // recursive function for player movement to check for walls and edges
    {
        var stepCount = stepSize+1; //stores the variable for number of tiles to move
        moveAllowed = false;

        
        // W + z axis NORTH
        if (dir == 1)
        {

                Vector3 newPos = new Vector3(player.position.x, player.position.y, player.position.z + grid.nodeDiameter*stepCount); //check next tile in direction desired * number of tiles
                Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

                    if (newNode.walkable && stepCount <= grid.gridSizeX) // if the node is vacant and were not moving an entire grid length
                    {
                    selectTile(dir, stepCount); // call the function again with the increased step count/size
                    }
                    else
                    {
                     Vector3 lastPos = new Vector3(player.position.x, player.position.y, player.position.z + grid.nodeDiameter * (stepCount - 1)); // if unwalkable we get the previous node
                     Node lastNode = grid.NodeFromWorldPoint(lastPos);
                     destination = lastNode.worldPosition; // put our destination object there and the player will move towards it
            }
         }
        
        //A key - on x axis WEST
        if (dir == -2)
        {
            Vector3 newPos = new Vector3(player.position.x - grid.nodeDiameter*stepCount, player.position.y, player.position.z);
            Node newNode = grid.NodeFromWorldPoint(newPos);

            if (newNode.walkable && stepCount <= grid.gridSizeX)
            {
                selectTile(dir, stepCount);
            }
            else
            {
                Vector3 lastPos = new Vector3(player.position.x - grid.nodeDiameter * (stepCount - 1), player.position.y, player.position.z);
                Node lastNode = grid.NodeFromWorldPoint(lastPos);
                destination = lastNode.worldPosition;
            }

        }
        //S key - on z axis SOUTH
        if (dir == -1)
        {
            Vector3 newPos = new Vector3(player.position.x, player.position.y, player.position.z - grid.nodeDiameter * stepCount);
            Node newNode = grid.NodeFromWorldPoint(newPos);

            if (newNode.walkable && stepCount <= grid.gridSizeX)
            {
                selectTile(dir, stepCount);
            }
            else
            {
                Vector3 lastPos = new Vector3(player.position.x, player.position.y, player.position.z - grid.nodeDiameter * (stepCount - 1));
                Node lastNode = grid.NodeFromWorldPoint(lastPos);
                destination = lastNode.worldPosition;
            }

        }
        //D key + on x axis EAST
        if (dir == 2)
        {
            Vector3 newPos = new Vector3(player.position.x + grid.nodeDiameter * stepCount, player.position.y, player.position.z);
            Node newNode = grid.NodeFromWorldPoint(newPos);

            if (newNode.walkable && stepCount <= grid.gridSizeX)
            {
                selectTile(dir, stepCount);
            }
            else
            {
                Vector3 lastPos = new Vector3(player.position.x + grid.nodeDiameter * (stepCount-1), player.position.y, player.position.z);
                Node lastNode = grid.NodeFromWorldPoint(lastPos);

                destination = lastNode.worldPosition;

            }
            
        }

    }

    public void enemyMove() // function to call each enemy to step once
    {
        foreach (enemyScript emy in enemyList)
        {
            emy.enemyStep();
        }

    }

}
