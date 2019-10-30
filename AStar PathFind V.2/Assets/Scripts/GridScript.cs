using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform player; //Reference for the player
    public LayerMask unwalkableMask; //Mask used to identify objects which are unwalkable
    public Vector2 gridWorldSize; //Area in world cooardinates that the grid will cover
    public float nodeRadius; //Radius of each Node

    Node[,] grid; //2D array of our Node class which will mark out the grid in nodes
    float nodeDiameter; //Diameter of node which is calulated and used to determine the size of grid x and y
    int gridSizeX, gridSizeY; //The x and y size of the grid

    //On start this will set x and y values to our grid divided by the node diameter
    private void Start() 
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//Determines how many nodes we can fit on the x axis
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//Determines how many nodes we can fit on the y axis
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];//Passes in two variables into our 2D array of nodes 
        //Calculate the bottom left by setting original position to the center grid than subtracting and multiplying to reach bottom left
        //NOTE Z AXIS USED INSTEAD OF Y FOR FORWARD
        Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                //Moves across the grid while x and y increment measuring in nodes
                Vector3 worldPoint = worldbottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                //Debug.Log(grid[x,y].gridX + "s");
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                    if(x*y != 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    //Takes in a position and calculates the percentage of how far along the node is on the grid
    public Node NodeFromWorldPoint (Vector3 worldPostion)
    {
        //Calculates percent of node position on grid
        float percentX = (worldPostion.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPostion.z + gridWorldSize.y / 2) / gridWorldSize.y;
        //Clamp to prevent values outside of the grid
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if ( grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(path !=null)
                {
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                }
                if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
