using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{

    public enum direction{
        North,
        South,
        East,
        West,
        None,
    };
    public bool isSquareEnemy = true;

    public GameObject aStar; // grid references
    GridScript grid;
    public direction dir; // direction of movement
    public float speed = 20f;
    public int range;
    public Vector3 destination; // destination object
    bool onRun = true; // run once regardless of void start

    public bool isMovingEnemy = true;
    public bool enemySharingTile = false; //Is there more than one enemy on one tile
    public LayerMask enemyMask, obstacleMask;

    public Vector3 testDirection = -Vector3.up;

    public Vector3 nodeOffset = new Vector3(3, 0, 3); //Offset for enemies on multiple tiles

    private void Awake()
    { //Assign grid as a reference to our gridscript class
        aStar = GameObject.Find("A*");
        grid = aStar.GetComponent<GridScript>();
        //weapon = transform.GetChild(1);
    }


    // Update is called once per frame
    void Update()
    {
        enemyAttack();
        if (onRun == true)
        {
            Node snap = grid.NodeFromWorldPoint(transform.position);
            transform.position = snap.worldPosition;
            destination = transform.position;
            onRun = false;
        }
       
        multipleEnemiesOnTile();
        
        switch (dir)
        {
            case direction.North:
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                }
            case direction.South:
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }
            case direction.East:
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                }
            case direction.West:
                {
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                }
        }
    }
    //Runs movetowards function in update if no enemies are sharing the same tile
   
    public void multipleEnemiesOnTile () 
    {
        
        if (enemySharingTile == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        }
    }
    //Collider to detect if enemies are sharing the same tile, if they are an offset is created and enemies are placed at a new position
    //At the end of enemyStep the bool enemySharingTile is set to false so the next move the enemies will continue on their path.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemySharingTile = true;
            Vector3 newPos = transform.position + nodeOffset;
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);
        }
    }

    // Creates a raycast from the enemies postion with a range of one node multiple by a public variable, Range which determines the max range of the raycast
    //If the tag of the ray hit is the player than will set active.
    //The enemyMask parameter of the raycast makes sure that it can also hit obstacles so the player cannot be destoryed from behind cover
    public void enemyAttack()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * (grid.nodeDiameter * range), Color.red, 0, false);// Debugging ray to see raycast length and direction
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, grid.nodeDiameter * range,enemyMask) && enemySharingTile == false)
              { 
                 if (hit.transform.tag == "Player")
                     {
                        Debug.Log(hit.transform.tag);
                        hit.transform.gameObject.SetActive(false);
                    }
             }
    }
    public void enemyStep()
    {
        enemyAttack();
        switch (dir)
        {
            case direction.North:
                {
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + grid.nodeDiameter); //get next position
                    Node newNode = grid.NodeFromWorldPoint(newPos); //get the node

                    if (newNode.walkable && newPos.z <= (grid.gridSizeX / 2) * grid.nodeDiameter * 1.01) // node is walkable and is inside the grid
                    {
                        destination = newNode.worldPosition; // place destination on new node
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.East : direction.South;
                        //dir = direction.South;
                        //enemyStep(); //reverse direction and call function again
                    }
                    break;
                }
            case direction.South:
                {
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - grid.nodeDiameter);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.z >= -1 * ((grid.gridSizeX / 2) * grid.nodeDiameter * 1.01))
                    {
                        destination = newNode.worldPosition;
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.West : direction.North;
                        //dir = direction.North;
                        //enemyStep();
                    }
                    break;
                }
            case direction.East:
                {
                    Vector3 newPos = new Vector3(transform.position.x + grid.nodeDiameter, transform.position.y, transform.position.z);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.x <= ((grid.gridSizeX / 2) * grid.nodeDiameter * 1.01))
                    {
                        destination = newNode.worldPosition;

                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.South : direction.West;
                        //dir = direction.West;
                        //enemyStep();

                    }
                    break;
                }
            case direction.West:
                {
                    Vector3 newPos = new Vector3(transform.position.x - grid.nodeDiameter, transform.position.y, transform.position.z);
                    Node newNode = grid.NodeFromWorldPoint(newPos);

                    if (newNode.walkable && newPos.x >= -1 * (grid.gridSizeX / 2) * grid.nodeDiameter * 1.01)
                    {
                        destination = newNode.worldPosition;
                    }
                    else
                    {
                        dir = isSquareEnemy ? direction.South : direction.East;
                        //dir = direction.East;
                        //enemyStep();
                    }
                    break;
                }
            case direction.None:
                {
                    transform.Rotate(0, 90, 0);
                }
                break;
        }
        enemySharingTile = false;
    }
 
}
