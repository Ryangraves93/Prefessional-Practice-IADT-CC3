using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
    public GameObject aStar; // grid references
    GridScript grid;
    bool onRun = true; // run once regardless of void start


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
            onRun = false;
        }


    }
}
