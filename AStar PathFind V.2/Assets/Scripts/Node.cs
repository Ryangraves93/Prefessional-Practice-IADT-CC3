using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;//Boolean for walkable tiles
    public Vector3 worldPosition;

    public int gCost;//How far away the node is from the starting node
    public int hCost;//How far away the node if from the end node

    public int gridX;//Reference to nodes position on the x axis
    public int gridY;//Reference to nodes position on the y axis
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        
    }

    public int fCost //Fcost is Gcost and Hcost combined
    {
        get
        {
            return gCost + hCost;
        }
    }
}
