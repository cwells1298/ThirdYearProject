using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPawn : MonoBehaviour
{
    public int chanceToSpawn = 0;
    public bool workplaceAssigned = false;
    public WorkContainer workContainer;

    private bool CheckedWork = false;
    private Schedule_Base pawnSchedule;
    private Transform sleepZone;
    private Transform restZone;
    public Transform workZone;

    private int currentMinute = 0;

    private bool isSleep = false;
    private bool isRest = false;
    private bool isWork = false;
    private bool isRecreation = false;
    private bool isSocial = false;
    private bool isTravel = false;

    private int startMin = 0;
    private int startHour = 0;

    public int classType;
    // Start is called before the first frame update
    void Start()
    {
        pawnSchedule = GetComponent<Schedule_Base>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!CheckedWork)
        //{
        //    if (workContainer.workplacesAssigned)
        //    {
        //        CheckedWork = true;
                //if (workplaceAssigned)
                //{
                //    for (int i = 0; i < pawnSchedule.taskListToday.Count; i++)
                //    {
                //        //if (pawnSchedule.taskListToday[i] == TaskTypes.Work)
                //        //{
                //        //    pawnSchedule.taskListToday[i] = TaskTypes.Joy;
                //        //}
                //    }
                //}
        //    }
        //}

        //if (TimeGraphing.timeAnalysis && !TimeHandler.gamePaused)
        //{
        //    if (currentMinute != TimeHandler.currentMinute)
        //    {
        //        if (isSleep)
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.sleepTime[classType]++;
        //        }
        //        else if (isRest)
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.restTime[classType]++;
        //        }
        //        else if (isWork)
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.workTime[classType]++;
        //        }
        //        if (isRecreation)
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.recreationTime[classType]++;
        //        }
        //        if (isSocial)
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.socialTime[classType]++;
        //        }
        //        else
        //        {
        //            currentMinute = TimeHandler.currentMinute;
        //            TimeGraphing.travelTime[classType]++;
        //        }
        //    }
            
        //}
    }

    public Transform GetSleepZone()
    {
        return sleepZone;
    }

    public Transform GetWorkZone()
    {
        return workZone;
    }

    public Transform GetRestZone()
    {
        return restZone;
    }

    public void SetSleepZone(Transform zone)
    {
        sleepZone = zone;
    }

    public void SetWorkZone(Transform zone)
    {
        workZone = zone;
    }

    public void SetRestZone(Transform zone)
    {
        restZone = zone;
    }
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeGraphing.timeAnalysis && !TimeHandler.gamePaused)
        {
            if (collision.tag == "Sleep")
            {
                isSleep = true;
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.travelTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = false;
            }

            if (collision.tag == "Rest")
            {
                isRest = true;
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.travelTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = false;
            }

            if (collision.tag == "Work")
            {
                isWork = true;
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.travelTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = false;
            }

            if (collision.tag == "Recreation")
            {
                isRecreation = true;
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.travelTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = false;
            }

            if (collision.tag == "Social")
            {
                isSocial = true;
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.travelTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TimeGraphing.timeAnalysis && !TimeHandler.gamePaused)
        {
            if (collision.tag == "Sleep")
            {
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.sleepTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = true;
                isSleep = false;
            }

            if (collision.tag == "Rest")
            {
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.restTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = true;
                isRest = false;
            }

            if (collision.tag == "Work")
            {
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.workTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = true;
                isWork = false;
            }

            if (collision.tag == "Recreation")
            {
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.recreationTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = true;
                isRecreation = false;
            }

            if (collision.tag == "Social")
            {
                int endHour = TimeHandler.currentHour;
                int endMin = TimeHandler.currentMinute;

                TimeGraphing.socialTime[classType] += GetTimeDifference(startHour, startMin, endHour, endMin);

                startHour = endHour;
                startMin = endMin;
                isTravel = true;
                isSocial = false;
            }
        }
    }

    private int GetTimeDifference(int sHour, int sMin, int eHour, int eMin)
    {
        int startTime = (sHour * 60) + sMin;
        int endTime = (eHour * 60) + eMin;
        int diff = 0;

        if (endTime < startTime)
        {
            diff = ((24 * 60) - startTime) + endTime;
        }
        else
        {
            diff = endTime - startTime;
        }
        return diff;
    }

}
