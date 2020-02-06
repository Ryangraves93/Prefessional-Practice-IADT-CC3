using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[ExecuteInEditMode]*/
public class GridScript : MonoBehaviour
{
    public Transform player; //Reference for the player
    public LayerMask unwalkableMask; //Mask used to identify objects which are unwalkable
    public Vector2 gridWorldSize; //Area in world cooardinates that the grid will cover
    public float nodeRadius; //Radius of each Node
    public GameTile tilePrefab = default;
    //public GameObject board;
    

    public GameTile[,] grid; //2D array of our Node class which will mark out the grid in nodes
    public float nodeDiameter; //Diameter of node which is calulated and used to determine the size of grid x and y
    public int gridSizeX, gridSizeY; //The x and y size of the grid
    int posX, posY;
    
    //On start this will set x and y values to our grid divided by the node diameter
    private void Start() 
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//Determines how many nodes we can fit on the x axis
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//Determines how many nodes we can fit on the y axis
        CreateGrid();
        //board.transform.localScale = new Vector3(gridSizeX * nodeDiameter + 3, board.transform.localScale.y, gridSizeY * nodeDiameter + 3);
        //board.transform.position = new Vector3(0.5f, -0.8f, 0.5f);
    }


    //CreateGrid() will create a grid for the game that you can place on any surface. This grid can be manipulated in the inspector to any size you want by seting the
    //gridSizeX and gridSizeY 
    void CreateGrid()
    {
        grid = new GameTile[gridSizeX, gridSizeY];//Passes in two variables into our 2D array of nodes 
        
       // tiles = new Node[posX, posY];
        //Calculate the bottom left by setting original position to the center grid than subtracting and multiplying to reach bottom left
        //NOTE Z AXIS USED INSTEAD OF Y FOR FORWARD
        Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                tilePrefab.transform.localScale = new Vector3(nodeDiameter,tilePrefab.transform.localScale.y,nodeDiameter);
                //Moves across the grid while x and y increment measuring in nodes
                Vector3 worldPoint = worldbottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));//Determines if a tile is walkable
                grid[x, y] = Instantiate(tilePrefab, worldPoint, Quaternion.identity);
              
                grid[x,y].tileMap = new Node(walkable, worldPoint, x, y);
                grid[x, y].tileMap.parentTile = grid[x, y];
                //Debug.Log("hi");
                //tiles[posX, posY] = new Node(walkable, worldPoint, x, y);



                
                //Debug.Log(grid[x,y].gridX + "s");
            }
        }
    }
    //GetNeighbours() is used to calculate what direction the alogrithm checks. Essentially what this means is, this function is responsible for what type of path we would create.
    //Below i've added in a diagram that will help understand what I mean. The two for loops below determine what directions we check in. 
    //Because we're not checking anything anything that isn't just North, South , East or West we can't go diagonally

   //  N.W   N  N.E 
   //     \  |  / 
   //      \ | / 
   // W----Cell----E 
   //      / | \ 
   //     /  |  \ 
   //  S.W   S  S.E

   // Cell-->Popped Cell(x, y)
   // N --> North(x-1, y)
   // S --> South(x+1, y)
   // E --> East(x, y+1)
   // W --> West(x, y-1)
   // N.E--> North-East(x-1, y+1)
   // N.W--> North-West(x-1, y-1)
   // S.E--> South-East(x+1, y+1)
   // S.W--> South-West(x+1, y-1)
   //Checks negative x axis first

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
                    neighbours.Add(grid[checkX, checkY].tileMap);
                }
            }
        }

        return neighbours;
    }

    /*public List<Node> ShowAvailableMoves(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                if (x * y != 0)
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
    }*/

    //NodeFromWorldPoint() - This function takes in any vector 3 and transfers it into any world point on our grid. Use this if you want to move anything grid.
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
        return grid[x, y].tileMap;
    }


    public List<Node> path;


    //Only used for development purposes, it draws a visualisation of the grid on the game scene to show the span of the grid and the width of the node diameters.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
       {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach (GameTile n in grid)
            {
                Gizmos.color = (n.tileMap.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n.tileMap))
                        Gizmos.color = Color.black;
             }
                if (playerNode == n.tileMap)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.tileMap.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
