using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsWork++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsWork--;
        }
    }
}
