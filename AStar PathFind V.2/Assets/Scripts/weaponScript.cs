using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject aStar;
    GridScript grid;
    // Start is called before the first frame update
    void Awake()
    {
        aStar = GameObject.Find("A*");
        grid = aStar.GetComponent<GridScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Node weaponNode = grid.NodeFromWorldPoint(transform.position);
        if (weaponNode.walkable
            && transform.position.z >= -1 * ((grid.gridSizeX / 2) * grid.nodeDiameter)
            && transform.position.z <= 1 * ((grid.gridSizeX / 2) * grid.nodeDiameter)
            && transform.position.x <= 1 * ((grid.gridSizeX / 2) * grid.nodeDiameter)
            && transform.position.x >= -1 * ((grid.gridSizeX / 2) * grid.nodeDiameter))
        {
            weaponNode.parentTile.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }
}
