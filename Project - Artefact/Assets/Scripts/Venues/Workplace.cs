using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : NeedSupply
{
    public bool workplaceFull = false;

    public int totalJobCount = 0;
    public int currentJobCount = 0;
    private List<InitPawn> workers;

    private void Start()
    {
        int xScale = (int)transform.localScale.x;
        int yScale = (int)transform.localScale.y;
        int squareSize = xScale * yScale;
        float rootScale = Mathf.Sqrt(squareSize);
        totalJobCount = Mathf.CeilToInt(rootScale /*/ 2*/);
        workers = new List<InitPawn>();
    }

    public void AssignWorker(InitPawn newWorker)
    {
        workers.Add(newWorker);
        newWorker.workplaceAssigned = true;
        newWorker.SetWorkZone(transform);
        currentJobCount++;
        if (currentJobCount >= totalJobCount)
        {
            workplaceFull = true;
        }
    }
}

