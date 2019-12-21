using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject aStar;
    GridScript grid;
    public int dir;
    public float speed = 20f;
    public Vector3 destination;
    bool onRun = true;

    // Start is called before the first frame update
    private void Awake()
    { //Assign grid as a reference to our gridscript class
        aStar = GameObject.Find("A*");
        grid = aStar.GetComponent<GridScript>();
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
        if (dir == 1) // north +z
        {

            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + grid.nodeDiameter); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            if (newNode.walkable && newPos.z <= (grid.gridSizeX/2) * grid.nodeDiameter)
            {
                destination = newNode.worldPosition;
            }

             else
            {
                dir *= -1;
                enemyStep();
            }


        }

        //A key - on x axis
        if (dir == -2)
        {
            Vector3 newPos = new Vector3(transform.position.x - grid.nodeDiameter, transform.position.y, transform.position.z); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

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
        //S key - on z axis
        if (dir == -1)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - grid.nodeDiameter); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

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
        //D key + on x axis
        if (dir == 2)
        {
            Vector3 newPos = new Vector3(transform.position.x + grid.nodeDiameter, transform.position.y, transform.position.z); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

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
