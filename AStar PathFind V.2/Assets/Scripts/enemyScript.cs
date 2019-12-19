using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject aStar;
    GridScript grid;
    public int dir = 0;
    public float speed = 20f;
    Vector3 destination;
    bool onRun = true;

    // Start is called before the first frame update
    private void Awake()
    { //Assign grid as a reference to our gridscript class
        aStar = GameObject.Find("A*");
        grid = aStar.GetComponent<GridScript>();
    }

    void Start()
    {
        /*Node snap = grid.NodeFromWorldPoint(transform.position);
        transform.position = snap.worldPosition;
        destination = transform.position;*/
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

    }

    public void enemyStep()
    {
        if (dir == 1)
        {

            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + grid.nodeDiameter); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            if (newNode.walkable)
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
        if (dir == -1)
        {
            Vector3 newPos = new Vector3(transform.position.x - grid.nodeDiameter, transform.position.y, transform.position.z); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            if (newNode.walkable)
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
        if (dir == 2)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - grid.nodeDiameter); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            if (newNode.walkable)
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
        if (dir == -2)
        {
            Vector3 newPos = new Vector3(transform.position.x + grid.nodeDiameter, transform.position.y, transform.position.z); //check next tile in direction desired * number of tiles
            Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

            if (newNode.walkable)
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
