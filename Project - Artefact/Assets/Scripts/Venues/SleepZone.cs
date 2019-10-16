using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsSleep++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsSleep--;
        }
    }
}
