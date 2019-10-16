using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyZone : PublicZone
{
    new protected void OnTriggerEnter2D(Collider2D collision)
    {
        AgentEnter();

        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsRecreation++;
        }
    }

    new protected void OnTriggerExit2D(Collider2D collision)
    {
        AgentExit();

        if (SpatialGraph.spatialAnalysis)
        {
            SpatialGraph.agentsRecreation--;
        }
    }
}
