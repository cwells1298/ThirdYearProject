using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGraph : MonoBehaviour
{
    //Numbers fluctuate as agents enter and exit each zone type, 'snapshot' taken upon each hour and added to list, paused upon new day, allowing data to be recorded, then rest upon unpausing
    public static int agentsSleep = 0;
    public static int agentsRest = 0;
    public static int agentsWork = 0;
    public static int agentsRecreation = 0;
    public static int agentsSocial = 0;
    public static int agentsTravel = 0;

    public int[] hourSpaceGraphSleep;
    public int[] hourSpaceGraphRest;
    public int[] hourSpaceGraphWork;
    public int[] hourSpaceGraphRecreation;
    public int[] hourSpaceGraphSocial;
    public int[] hourSpaceGraphTravel;

    public static bool spatialAnalysis = false;
    public SpawnBaseTypes spawn;

    private bool firstUpdate = true;
    private int currentDay = 0;
    private int currentHour = 0;
    private bool endOfDay = false;

    void Update()
    {
        if (spatialAnalysis)
        {
            if (!TimeHandler.gamePaused && !firstUpdate)
            {
                if (endOfDay)
                {
                    ResetDay();
                }

                if (currentHour != TimeHandler.currentHour)
                {
                    NewHour();
                }

                if (currentDay != TimeHandler.currentDay)
                {
                    endOfDay = true;
                    TimeHandler.PauseGame();
                    return;
                }

            

            }

            if (firstUpdate)
            {
                firstUpdate = false;
                hourSpaceGraphSleep = new int[24];
                hourSpaceGraphRest = new int[24];
                hourSpaceGraphWork = new int[24];
                hourSpaceGraphRecreation = new int[24];
                hourSpaceGraphSocial = new int[24];
                hourSpaceGraphTravel = new int[24];
                agentsTravel = 0;
            }
        }
    }

    private void NewHour()
    {
        hourSpaceGraphSleep[currentHour] = agentsSleep;
        hourSpaceGraphRest[currentHour] = agentsRest;
        hourSpaceGraphWork[currentHour] = agentsWork;
        hourSpaceGraphRecreation[currentHour] = agentsRecreation;
        hourSpaceGraphSocial[currentHour] = agentsSocial;
        hourSpaceGraphTravel[currentHour] = agentsTravel;

        agentsTravel = 0;
        
        currentHour = TimeHandler.currentHour;
    }

    private void ResetDay()
    {
        currentDay = TimeHandler.currentDay;
        currentHour = 0;
        endOfDay = false;
        hourSpaceGraphSleep = new int[24];
        hourSpaceGraphRest = new int[24];
        hourSpaceGraphWork = new int[24];
        hourSpaceGraphRecreation = new int[24];
        hourSpaceGraphSocial = new int[24];
        hourSpaceGraphTravel = new int[24];
    }
}
