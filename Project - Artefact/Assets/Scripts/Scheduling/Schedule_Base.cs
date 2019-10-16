using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule_Base : MonoBehaviour {

    public List<TaskTypes> taskListToday;
    public List<int> taskTimeHourToday;
    public List<int> taskTimeMinuteToday;
    public List<TaskTypes> taskListTomorrow;
    public List<int> taskTimeMinuteTomorrow;
    public List<int> taskTimeHourTomorrow;
    public Queue<TaskTypes> taskQueueToday;
    public Queue<int> taskQueueTimeHourToday;
    public Queue<int> taskQueueTimeMinuteToday;
    public Queue<TaskTypes> taskQueueTomorrow;
    public Queue<int> taskQueueTimeMinuteTomorrow;
    public Queue<int> taskQueueTimeHourTomorrow;
    public TaskTypes currentTask;

    protected Transform workZone, restZone, sleepZone;
    protected InitPawn pawn;

    protected PathfindingTestScript pathfinding;
    protected int currentDay = 0;

    protected List<JoyZone> fullJoyZones;
    protected List<InteractionZone> fullInteractionZones;
    protected PublicZone targetZone;

    //TODO develop graphing parameters

    protected void Start()
    {
        pathfinding = GetComponent<PathfindingTestScript>();
        pawn = GetComponent<InitPawn>();
        taskQueueToday = new Queue<TaskTypes>();
        taskQueueTimeHourToday = new Queue<int>();
        taskQueueTimeMinuteToday = new Queue<int>();
        taskQueueTomorrow = new Queue<TaskTypes>();
        taskQueueTimeHourTomorrow = new Queue<int>();
        taskQueueTimeMinuteTomorrow = new Queue<int>();

        fullInteractionZones = new List<InteractionZone>();
        fullJoyZones = new List<JoyZone>();

        for (int i = 0; i < taskListToday.Count; i++)
        {
            taskQueueToday.Enqueue(taskListToday[i]);
            taskQueueTimeHourToday.Enqueue(taskTimeHourToday[i]);
            taskQueueTimeMinuteToday.Enqueue(taskTimeMinuteToday[i]);
        }
    }

    protected void Update()
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
    }

    protected void ResetTimetable()
    {
       // Debug.Log("Resetting");
        currentDay = TimeHandler.currentDay;

        while (taskQueueTomorrow.Count > 0)
        {
            taskQueueToday.Enqueue(taskQueueTomorrow.Dequeue());
            taskQueueTimeHourToday.Enqueue(taskQueueTimeHourTomorrow.Dequeue());
            taskQueueTimeMinuteToday.Enqueue(taskQueueTimeMinuteTomorrow.Dequeue());
        }
    }

    private bool CompareTime()
    {
        if (taskQueueTimeHourToday.Peek() < TimeHandler.currentHour)
        {
            return true;
        }
        if (taskQueueTimeHourToday.Peek() == TimeHandler.currentHour)
        {
            if (taskQueueTimeMinuteToday.Peek() <= TimeHandler.currentMinute)
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

        //Determine target location
        switch (taskQueueToday.Peek())
        {
            case TaskTypes.Sleep:
                targetLocation = sleepZone;
                currentTask = TaskTypes.Sleep;
                break;
            case TaskTypes.Eat:
                targetLocation = restZone;
                currentTask = TaskTypes.Eat;
                break;
            case TaskTypes.Work:
                targetLocation = workZone;
                currentTask = TaskTypes.Work;
                break;
            case TaskTypes.Joy:
                targetLocation = FindJoyLocation();
                currentTask = TaskTypes.Joy;
                break;
            case TaskTypes.Social:
                targetLocation = FindInteractionLocation();
                currentTask = TaskTypes.Social;
                break;
            default:
                break;
        }
        
        //Set tomorrows schedule and remove current task from list
        taskQueueTomorrow.Enqueue(taskQueueToday.Dequeue());
        taskQueueTimeHourTomorrow.Enqueue(taskQueueTimeHourToday.Dequeue());
        taskQueueTimeMinuteTomorrow.Enqueue(taskQueueTimeMinuteToday.Dequeue());

        if (targetLocation != null)
        {
            SetTarget(targetLocation);
        }
    }

    protected Transform FindJoyLocation()
    {
        JoyZone[] joyZones = FindObjectsOfType<JoyZone>();
        JoyZone currentClosest = null;
        float distToClosest = Mathf.Infinity;

        foreach (JoyZone zone in joyZones)
        {
            if (!fullJoyZones.Contains(zone))
            {
                float newDist = (zone.transform.position - transform.position).sqrMagnitude;
                if (newDist < distToClosest)
                {
                    distToClosest = newDist;
                    currentClosest = zone;
                }
            }      
        }

        return currentClosest.transform;
    }

    protected Transform FindInteractionLocation()
    {
        InteractionZone[] InteractionZones = FindObjectsOfType<InteractionZone>();
        InteractionZone currentClosest = null;
        float distToClosest = Mathf.Infinity;

        foreach (InteractionZone zone in InteractionZones)
        {
            if (!fullInteractionZones.Contains(zone))
            {
                float newDist = (zone.transform.position - transform.position).sqrMagnitude;
                if (newDist < distToClosest)
                {
                    distToClosest = newDist;
                    currentClosest = zone;
                }
            }
        }

        return currentClosest.transform;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentTask == TaskTypes.Joy)
        {
            JoyZone zoneJoy = collision.GetComponent<JoyZone>();
            if (zoneJoy != null)
            {
                if (zoneJoy.areaFull)
                {
                    fullJoyZones.Add(zoneJoy);
                    Transform newPos = FindJoyLocation();
                    SetTarget(newPos);
                }
            }
        }

        if (currentTask == TaskTypes.Social)
        {
            InteractionZone zoneInteraction = collision.GetComponent<InteractionZone>();
            if (zoneInteraction != null)
            {
                if (zoneInteraction.areaFull)
                {
                    fullInteractionZones.Add(zoneInteraction);
                    Transform newPos = FindInteractionLocation();
                    SetTarget(newPos);
                }
            }
        }
    }

    protected void SetTarget(Transform targetLocation)
    {
        int xPos = Mathf.CeilToInt(targetLocation.position.x);
        int yPos = Mathf.CeilToInt(targetLocation.position.y);

        Vector3Int newPos = new Vector3Int(xPos, yPos, 0);
        pathfinding.SetTarget(newPos);
    }

}




