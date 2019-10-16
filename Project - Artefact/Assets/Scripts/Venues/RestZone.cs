using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsRest++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsRest--;
        }
    }
}
