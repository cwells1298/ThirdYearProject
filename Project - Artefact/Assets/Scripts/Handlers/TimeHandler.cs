using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

public struct TimeOfDay
{
    public int minute;
    public int hour;
}

public class TimeHandler : MonoBehaviour {

    //Keeps a log of the current time
    public static int currentMinute;
	public static int currentHour;
    public static int currentDay;
    //Tracks how many minutes pass each pass (set as static to help determine relevant movement speeds)
    public static int timeScale = 1;
    //How long each pass takes
    public float passDelay;
    //Tell the rest of the game the it is paused
    public static bool gamePaused;
       
    private float currentPassTime;
    private static float currentTimeScale = 60.0f;

    private void Start()
    {
        currentDay = 0;
        currentHour = 0;
        currentMinute = 0;
        currentPassTime = 0.0f;
        Time.timeScale = currentTimeScale;
        gamePaused = false;
    }

    private void Update()
    {
        if (!gamePaused)
        {
            bool timeChanged = false;

            currentPassTime += Time.deltaTime;
            //Debug.Log("Time: " + currentPassTime);

            if (currentPassTime >= passDelay)
            {
                currentPassTime = 0.0f;
                currentMinute += 1;               
            }

            //Increment currentHour if currentMinute is over 60
            if (currentMinute >= 60)
            {
                currentMinute = currentMinute - 60;
                currentHour++;
                timeChanged = true;
            }

            //Increment currentDay if currentHour is over 24
            if (currentHour >= 24)
            {
                currentHour = currentHour - 24;
                currentDay++;
            }

            if (timeChanged)
            {
               Debug.Log("Day: " + currentDay + "; Hour: " + currentHour + "; Minute: " + currentMinute);
            }
        }        
    }

    public static void ChangeTimeScale(float ts)
    {
        if (currentTimeScale + ts < 1)
        {
            //Debug.Log("Cannot decrease timeScale to less than 0");
            return;
        }

        currentTimeScale += ts;
        Time.timeScale = currentTimeScale;
        Debug.Log("Current Timescale: " + currentTimeScale);
    }

    public static void PauseGame()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Debug.Log("Paused");
            Time.timeScale = 0.0f;
        }
        else
        {
            Debug.Log("Unpaused");
            Time.timeScale = currentTimeScale;
        }
        
    }

    public static Days DayNumToString(int intDay)
    {
        Days day = Days.Monday;
        int dayNum = intDay % 7;

        switch (dayNum)
        {
            case 0:
                day = Days.Monday;
                break;
            case 1:
                day = Days.Tuesday;
                break;
            case 2:
                day = Days.Wednesday;
                break;
            case 3:
                day = Days.Thursday;
                break;
            case 4:
                day = Days.Friday;
                break;
            case 5:
                day = Days.Saturday;
                break;
            case 6:
                day = Days.Sunday;
                break;
            default:
                break;
        }
        return day;
    }
}
