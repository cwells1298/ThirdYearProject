using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGraphing : MonoBehaviour
{
    //Record number of minutes each agent spends on each activity, separate by agent class and determine average at end of each day

    public static bool timeAnalysis = false;
    private bool firstUpdate = true;

    public static int[] sleepTime;
    public static int[] restTime;
    public static int[] workTime;
    public static int[] recreationTime;
    public static int[] socialTime;
    public static int[] travelTime;

    public int[] sleepTimeShow;
    public int[] restTimeShow;
    public int[] workTimeShow;
    public int[] recreationTimeShow;
    public int[] socialTimeShow;
    public int[] travelTimeShow;

    private int currentDay = 0;
    private bool endOfDay = false;

    private void Start()
    {
        if (timeAnalysis)
        {
            sleepTime = new int[14];
            restTime = new int[14];
            workTime = new int[14];
            recreationTime = new int[14];
            socialTime = new int[14];
            travelTime = new int[14];

            sleepTimeShow = new int[14];
            restTimeShow = new int[14];
            workTimeShow = new int[14];
            recreationTimeShow = new int[14];
            socialTimeShow = new int[14];
            travelTimeShow = new int[14];
        }
    }

    void Update()
    {
        if (timeAnalysis)
        {
            if (!TimeHandler.gamePaused && !firstUpdate)
            {
                if (endOfDay)
                {
                    ResetDay();
                }

                if (currentDay != TimeHandler.currentDay)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        if (SpawnBaseTypes.classNumbers[i] > 0)
                        {
                            sleepTimeShow[i] = sleepTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                            restTimeShow[i] = restTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                            workTimeShow[i] = workTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                            recreationTimeShow[i] = recreationTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                            socialTimeShow[i] = socialTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                            travelTimeShow[i] = travelTimeShow[i] / SpawnBaseTypes.classNumbers[i];
                        }         
                    }
                    endOfDay = true;
                    TimeHandler.PauseGame();
                    return;
                }

                sleepTimeShow = sleepTime;
                restTimeShow = restTime;
                workTimeShow = workTime;
                recreationTimeShow = recreationTime;
                socialTimeShow = socialTime;
                travelTimeShow = travelTime;
            }

            if (firstUpdate)
            {             

                firstUpdate = false;
            }
        }
    }

    private void ResetDay()
    {
        currentDay = TimeHandler.currentDay;
        endOfDay = false;

        sleepTime = new int[14];
        restTime = new int[14];
        workTime = new int[14];
        recreationTime = new int[14];
        socialTime = new int[14];
        travelTime = new int[14];

        sleepTimeShow = new int[14];
        restTimeShow = new int[14];
        workTimeShow = new int[14];
        recreationTimeShow = new int[14];
        socialTimeShow = new int[14];
        travelTimeShow = new int[14];
    }
}
