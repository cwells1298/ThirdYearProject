using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;


public class PathfindingTestScript : MonoBehaviour {

    public Tilemap pathMap;
    public GridLayout grid;

    public AILerp aILerp;
    public float movementSpeed = 3.0f;
    [Range(0.0f, 1.0f)]
    public float crowdedMovementFactor = 0.5f;
    //TODO get academic sources (oliviers crowd sim perhaps???) for average speed reduction when moving in a crowd

    public bool targetReached = false;

    public float distance = 0;

    private Vector3 targetPosition;
    private bool targetReady;

    private Seeker seeker;

    private AstarPath path;

    private float currentSpeed;
    private InitPawn init;
    private bool travelledThisHour;
    private int currentHour = 0;

    private void Start()
    {
        targetReady = false;
        seeker = GetComponent<Seeker>();
        grid = FindObjectOfType<GridLayout>();
        pathMap = grid.GetComponentInChildren<Tilemap>();
        init = GetComponent<InitPawn>();

        path = FindObjectOfType<AstarPath>();

        currentSpeed = movementSpeed;
        aILerp.speed = currentSpeed;
    }


    // Update is called once per frame
    void Update ()
    {
        if (!TimeHandler.gamePaused)
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    Vector3Int cellClicked = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //    SetTarget(cellClicked);
            //}
            if (currentHour != TimeHandler.currentHour)
            {
                currentHour = TimeHandler.currentHour;
                travelledThisHour = false;
            }


            if (targetReady)
            {
                if (transform.position == targetPosition)
                {
                    //Debug.Log("Target Position Reached");
                    targetReady = false;
                    targetReached = true;
                }

                if (!travelledThisHour)
                {
                    SpatialGraph.agentsTravel++;
                    travelledThisHour = true;
                }
            }
        }
    }

    public void SetTarget(Vector3Int cellClicked)
    {
        TileBase tileClicked = pathMap.GetTile(cellClicked);

        if(tileClicked == null)
        {
            //Debug.Log("Valid Position");
            targetPosition = new Vector3(cellClicked.x + 0.5f, cellClicked.y + 0.5f, cellClicked.z);
            if (targetPosition == transform.position)
            {
                //Debug.Log("Already at target position");
                targetReady = false;
            }
            else
            {
                //Debug.Log("New Target Position Set:: x: " + targetPosition.x + ", y: " + targetPosition.y);
                targetReady = true;
                //transform.position = targetPosition;

                seeker.StartPath(transform.position, targetPosition, OnPathComplete);
            }
        }
        else
        {
            //Debug.Log("Invalid Position");
            targetReady = false;
        }
            
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.Log("Path cannot be created");
            targetReady = false;
        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==  11)
        {
            currentSpeed = movementSpeed * crowdedMovementFactor;
            aILerp.speed = currentSpeed;
        }

        //if (collision.gameObject.layer == 9)
        //{
        //    if (SpatialGraph.spatialAnalysis)
        //    {
        //        SpatialGraph.agentsTravel--;
        //    }
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            currentSpeed = movementSpeed * crowdedMovementFactor;
            aILerp.speed = currentSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            currentSpeed = movementSpeed;
            aILerp.speed = currentSpeed;
        }


        //if (collision.gameObject.layer == 9)
        //{
        //    if (SpatialGraph.spatialAnalysis)
        //    {
        //        SpatialGraph.agentsTravel++;
        //    }
        //}
    }

    public void SetSpeed(float s)
    {
        movementSpeed = s;
        aILerp.speed = movementSpeed;
    }
}
