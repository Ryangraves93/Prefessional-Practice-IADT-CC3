using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target; //Player and target postion

    GridScript grid; //Grid reference

    
    private void Awake()
    {
        grid = GetComponent<GridScript>(); //Assign grid as a reference to our gridscript class
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
        if (Input.GetMouseButtonDown(0))
        {
            movePlayer();
        }
 
    }
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); //List of nodes from "OpenSet" which are nodes that have not been checked yet
        HashSet<Node> closedSet = new HashSet<Node>();//HashSet of nodes from "ClosedSet" which are nodes that have alreadt been checked
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


    void RetracePath(Node startNode, Node EndNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = EndNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);//Adds current node to the path
            currentNode = currentNode.parent;//Parents node to retrace
            
        }

        path.Reverse();//Reverses path as the path was retraced
        grid.path = path;
        Debug.Log(grid.path[0].worldPosition);
        
     
    }

    void movePlayer()
    {
        


    }

    //Calculates distance from nodes ALLOWS FOR DIAGONAL
    int GetDistance(Node NodeA, Node NodeB)
    {
        int dstX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
        int dstY = Mathf.Abs(NodeB.gridY - NodeA.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
