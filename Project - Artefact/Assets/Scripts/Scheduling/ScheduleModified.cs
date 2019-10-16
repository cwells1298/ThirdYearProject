using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleModified : Schedule_Base
{
    private static int minVariance = 1; // minimum time in minutes that a pawn will leave early/late to their next task
    private static int maxVariance = 15; // maximum time in minutes that a pawn will leave early/late to their next task

    //measured in metres per second?
    //Average walking speed according to wikipedia (+/- 0.5 metres per second)
    //TODO get viable reference
    public float minSpeed = 0.9f;
    public float maxSpeed = 1.9f;
    public float crowdSlowFactor = 0.5f;
    [SerializeField]
    private float pawnSpeed = 1.4f;
    [SerializeField]
    private int potentialTimeVariance = 0;
    private int currentTimeVariance = 0;

    private int intendedArrivalHour = 0;
    private int intendedArrivalMinute = 0;
    public TaskTypes currentActivity;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        potentialTimeVariance = Random.Range(minVariance, maxVariance);
        pawnSpeed = 1.0f;/*Random.Range(minSpeed, maxSpeed);*/
      //  Debug.Log("pawnspeed: " + pawnSpeed);

        currentTimeVariance = Random.Range(-potentialTimeVariance, potentialTimeVariance);
    }

    // Update is called once per frame
    new void Update()
    {
        if (workZone == null)
        {
            if (pawn.GetWorkZone() != null)
            {
                workZone = pawn.GetWorkZone();
            }
        }

        if (restZone == null)
        {
            if (pawn.GetRestZone() != null)
            {
                restZone = pawn.GetRestZone();
            }
        }

        if (sleepZone == null)
        {
            if (pawn.GetSleepZone() != null)
            {
                sleepZone = pawn.GetSleepZone();
            }
        }

        if (currentDay != TimeHandler.currentDay)
        {
            ResetTimetable();
        }

        if (taskQueueToday.Count > 0)
        {
            if (CompareTime())
            {
                ActivateTask();
            }
        }

        if (pathfinding.targetReached)
        {
            pathfinding.targetReached = false;

            int actualArrivalHour = TimeHandler.currentHour;
            int actualArrivalMinute = TimeHandler.currentMinute;

            //positive if late, negative if early
            int minuteDifference = actualArrivalMinute - intendedArrivalMinute;
            int hourDifference = actualArrivalHour - intendedArrivalHour;

            int totalDiffInMin = minuteDifference + (hourDifference * 60);

            currentTimeVariance = Random.Range(-potentialTimeVariance, potentialTimeVariance);
            AlterSchedule(totalDiffInMin);
        }



    }

    private void AlterSchedule(int timeDiff)
    {
        //if more than 5 minutes early/late, alter schedule for tomorrow
        if (Mathf.Abs(timeDiff) > 5)
        {
            //Designate travel time
            int totalHours = timeDiff / 60;
            int totalMinutes = timeDiff % 60;
            int newHour = intendedArrivalHour - totalHours;
            int newMinute = intendedArrivalMinute - totalMinutes;
            newMinute = (int)Mathf.Round(newMinute / 5) * 5;

            if (newMinute < 0)
            {
                newHour--;
                newMinute += 60;
            }
            else if (newMinute >= 60)
            {
                newHour++;
                newMinute -= 60;
            }

            taskQueueTomorrow.Enqueue(TaskTypes.Travel);
            taskQueueTimeHourTomorrow.Enqueue(newHour);
            taskQueueTimeMinuteTomorrow.Enqueue(newMinute);

            //Set tomorrows schedule
            taskQueueTomorrow.Enqueue(currentTask);
            taskQueueTimeHourTomorrow.Enqueue(intendedArrivalHour);
            taskQueueTimeMinuteTomorrow.Enqueue(intendedArrivalMinute);
        }
        else
        {
            taskQueueTomorrow.Enqueue(currentTask);
            taskQueueTimeHourTomorrow.Enqueue(intendedArrivalHour);
            taskQueueTimeMinuteTomorrow.Enqueue(intendedArrivalMinute);
        }
    }

    private bool CompareTime()
    {
        int hour = taskQueueTimeHourToday.Peek();
        int minute = taskQueueTimeMinuteToday.Peek();
        minute += currentTimeVariance;
        if (minute >= 60)
        {
            minute -= 60;
            hour++;
        }
        else if (minute < 0)
        {
            minute += 60;
            hour--;
        }

        if (hour < TimeHandler.currentHour)
        {
            return true;
        }
        if (hour == TimeHandler.currentHour)
        {
            if (minute <= TimeHandler.currentMinute)
            {
                return true;
            }
        }

        return false;
    }

    private void ActivateTask()
    {
        Transform targetLocation = null;

        //Reset known status of zones
        fullJoyZones.Clear();
        fullInteractionZones.Clear();

        //TODO remove duplicate code??
        //Determine target location
        switch (taskQueueToday.Peek())
        {
            case TaskTypes.Sleep:
                if (currentActivity != TaskTypes.Travel)//If agent hasnt already reached target
                {
                    targetLocation = sleepZone;
                    intendedArrivalHour = taskQueueTimeHourToday.Peek();
                    intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();
                }

                currentTask = TaskTypes.Sleep;
                currentActivity = TaskTypes.Sleep;
                taskQueueToday.Dequeue();
                break;
            case TaskTypes.Eat:
                if (currentActivity != TaskTypes.Travel)//If agent hasnt already reached target
                {
                    targetLocation = restZone;
                    intendedArrivalHour = taskQueueTimeHourToday.Peek();
                    intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();
                }

                currentTask = TaskTypes.Eat;
                currentActivity = TaskTypes.Eat;
                taskQueueToday.Dequeue();
                break;
            case TaskTypes.Work:
                if (currentActivity != TaskTypes.Travel)//If agent hasnt already reached target
                {
                    targetLocation = workZone;
                    intendedArrivalHour = taskQueueTimeHourToday.Peek();
                    intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();
                }

                currentTask = TaskTypes.Work;
                currentActivity = TaskTypes.Work;
                taskQueueToday.Dequeue();
                break;
            case TaskTypes.Joy:
                if (currentActivity != TaskTypes.Travel)//If agent hasnt already reached target
                {
                    targetLocation = FindJoyLocation();
                    intendedArrivalHour = taskQueueTimeHourToday.Peek();
                    intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();
                }

                currentTask = TaskTypes.Joy;
                currentActivity = TaskTypes.Joy;
                taskQueueToday.Dequeue();
                break;
            case TaskTypes.Social:
                if (currentActivity != TaskTypes.Travel)//If agent hasnt already reached target
                {
                    targetLocation = FindInteractionLocation();
                    intendedArrivalHour = taskQueueTimeHourToday.Peek();
                    intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();
                }
                
                currentTask = TaskTypes.Social;
                currentActivity = TaskTypes.Social;
                taskQueueToday.Dequeue();
                break;
            case TaskTypes.Travel:
                taskQueueToday.Dequeue();
                taskQueueTimeHourToday.Dequeue();
                taskQueueTimeMinuteToday.Dequeue();

                currentTask = taskQueueToday.Peek();
                intendedArrivalHour = taskQueueTimeHourToday.Peek();
                intendedArrivalMinute = taskQueueTimeMinuteToday.Peek();

                //TODO remove duplicate code
                if (currentTask == TaskTypes.Sleep)
                {
                    targetLocation = sleepZone;
                }
                else if (currentTask == TaskTypes.Work)
                {
                    targetLocation = workZone;
                }
                else if (currentTask == TaskTypes.Eat)
                {
                    targetLocation = restZone;
                }
                else if (currentTask == TaskTypes.Joy)
                {
                    targetLocation = FindJoyLocation();
                }
                else if (currentTask == TaskTypes.Social)
                {
                    targetLocation = FindInteractionLocation();
                }

                currentActivity = TaskTypes.Travel;  
                break;
            default:
                break;
        }

        if (currentActivity != TaskTypes.Travel)
        {
            taskQueueTimeHourToday.Dequeue();
            taskQueueTimeMinuteToday.Dequeue();
        }

        if (targetLocation != null)
        {
            SetTarget(targetLocation);
        }
    }
}
