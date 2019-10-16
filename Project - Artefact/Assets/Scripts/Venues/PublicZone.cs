using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicZone : MonoBehaviour
{
    public bool areaFull = false;

    public int maxCapacity = 0;
    public int currentOccupancy = 0;

    void Start()
    {
        int xScale = (int)transform.localScale.x;
        int yScale = (int)transform.localScale.y;
        int squareSize = xScale * yScale;
        float rootScale = Mathf.Sqrt(squareSize);
        maxCapacity = Mathf.CeilToInt(rootScale / 2);
    }

    protected void AgentEnter()
    {
        currentOccupancy++;
        if (currentOccupancy >= maxCapacity)
        {
            areaFull = true;
        }
    }

    protected void AgentExit()
    {
        currentOccupancy--;
        if (currentOccupancy < maxCapacity)
        {
            areaFull = false;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        AgentEnter();
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        AgentExit();
    }
}
