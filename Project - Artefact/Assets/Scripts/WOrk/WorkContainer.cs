using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkContainer : MonoBehaviour
{
    public List<Workplace> workplaces;
    public GameObject pawnHolder;
    public bool workplacesAssigned = false;

    public List<InitPawn> pawns;
    private int currentWorkplace = 0;
    private int workPlaceCount = 0;
    private bool firstUpdate = true;

    void Start()
    {
        workplaces = new List<Workplace>();

        foreach (Workplace work in GetComponentsInChildren<Workplace>())
        {
            workplaces.Add(work);
            workPlaceCount++;
        }
    }

    private void Update()
    {
        if (firstUpdate)
        {
            pawns = new List<InitPawn>();

            foreach (InitPawn p in pawnHolder.GetComponentsInChildren<InitPawn>())
            {
                Schedule_Base schedule_Base = p.gameObject.GetComponent<Schedule_Base>();
                //Prevents agents without a work schedule from being assigned a workplace
                if (schedule_Base.taskListToday.Contains(TaskTypes.Work))
                {
                    pawns.Add(p);
                }              
            }

            for (int i = 0; i < pawns.Count; i++)
            {
                //Debug.Log("Assigning Pawn Work");
                workplaces[currentWorkplace].AssignWorker(pawns[i]);
                if (workplaces[currentWorkplace].workplaceFull)
                {
                   // Debug.Log("Workplace Full");
                    currentWorkplace++;
                    if (currentWorkplace >= workPlaceCount)
                    {
                    //    Debug.Log("All Workplaces Full");
                        break;
                    }
                }
            }

            workplacesAssigned = true;

            firstUpdate = false;
        }
    }
}
