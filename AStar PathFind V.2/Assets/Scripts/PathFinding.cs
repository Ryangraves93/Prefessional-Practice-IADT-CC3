using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    //bool tileSelected = false;

    private float startTime;
    public float speed = 50f;
    public Transform player, target; //Player and target postion
    public bool moveAllowed = true;
    public int dir;
    public Vector3 destination;
    public GameObject enemies;
    public GameObject destinationObject;
    enemyScript[] enemyList;
    GridScript grid; //Grid reference
   // int frameCount = 0;

    private void Start()
    {
        startTime = Time.time;
        Node snap = grid.NodeFromWorldPoint(player.position);
        player.position = snap.worldPosition;
        destination = player.position;
    }
    private void Awake()
    {

        enemyList = enemies.GetComponentsInChildren<enemyScript>();
        grid = GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
        //enemy = enemies.GetComponent<enemyScript>();
    }

    public void Update()
    {

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

        player.position = Vector3.MoveTowards(player.position, destination, Time.deltaTime * speed);
        destinationObject.transform.position = destination;

        if (Vector3.Distance(player.position, destination) < 0.01f)
        {
            moveAllowed = true;
        }

    }

    public void selectTile(int dir, int stepSize)
    {
        var stepCount = stepSize+1; //stores the variable for number of tiles to move
        moveAllowed = false;

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
                     Vector3 lastPos = new Vector3(player.position.x, player.position.y, player.position.z + grid.nodeDiameter * (stepCount - 1)); // put the player on the node before this one if its unwalkable.
                     Node lastNode = grid.NodeFromWorldPoint(lastPos);
                     destination = lastNode.worldPosition;
            }
         }
        
        //A key - on x axis
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
        //S key - on z axis
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
        //D key + on x axis
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

                Debug.Log("bump");
            }
            
        }
        //enemy.enemyStep();

    }

    public void enemyMove()
    {
        foreach (enemyScript emy in enemyList)
        {
            emy.enemyStep();
        }

    }


    //FindPath() is going to be the function you use to calculate the distance between two points on the grid. This is important though, before you use it make sure you're 
    //passing it in NODE classes and not vectors. If you need to convert a vector3 for it pass that into the gridFromWorldPoint() function on the gridscript.
    /*void FindPath(Node startNode, Node targetNode)
    {
        //Node startNode = grid.NodeFromWorldPoint(startPos);
        //Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); //List of nodes from "OpenSet" which are nodes that have not been checked yet
        HashSet<Node> closedSet = new HashSet<Node>();//HashSet of nodes from "ClosedSet" which are nodes that have already been checked
        openSet.Add(startNode);//Adds the first node

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //Checks the node being evalutated to see if it's fCost is lower or if the fCost is the same AND the hCost is lower
                //if so switches to that node
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            //Remove Current from the openset and add to the closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            //If destination is found runs RetracePath function passing in both the start and target node
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                //Calculates movement cost to neighbour using the gCost and GetDistance function
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;//Set new g cost
                    neighbour.hCost = GetDistance(neighbour, targetNode);//Calculates hCost with GetDistance function
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

        }
    }

    //RetracePath() allows yous to retrace the path between one point to another, you wouldn't really use this function as it's called in FindPath(Function above) and it's
    //used to retrace the steps back from the path found.
    void RetracePath(Node startNode, Node EndNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = EndNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);//Adds current node to the path
            currentNode = currentNode.parent;//Parents node to retrace
            
        }

        //path.Reverse();//Reverses path as the path was retraced
        grid.path = path;
        //Debug.Log(grid.path[0].worldPosition);
        
     
    }*/

    //Calculates distance from nodes 
    int GetDistance(Node NodeA, Node NodeB)
    {
        int dstX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int dstY = Mathf.Abs(NodeB.gridY - NodeA.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
