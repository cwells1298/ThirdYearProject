using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class PerformanceLog : MonoBehaviour
{
    public Text currentText;
    public Text averageText;
    public Text minimumText;
    public Text maximumText;
    public Text currentMText;
    public Text averageMText;
    public Text minimumMText;
    public Text maximumMText;

    private float currentFPS = 0.0f;    
    private float averageFPS = 0.0f;
    private float minimumFPS = Mathf.Infinity;
    private float maximumFPS = 0.0f;
    private float currentMemory = 0.0f;    
    private float averageMemory = 0.0f;
    private float minimumMemory = Mathf.Infinity;
    private float maximumMemory = 0.0f;

    private int currentDay = 0;
    private float currentTime = 0.0f;
    private float currentMemTotal = 0.0f;
    private int currentFrame = 0;

    private bool firstUpdate = true;
    private int frameCount = 0;

    private void Update()
    {
        if (!TimeHandler.gamePaused && !firstUpdate)
        {
            //FPS for previous frame
          //  currentTime += Time.deltaTime ;
            currentTime += Time.unscaledDeltaTime ;
            currentText.text = "Current FPS: " + currentFPS.ToString("F2");

            //Average FPS for current day
            currentFrame++;
            //currentFPS = 1.0f / (Time.unscaledDeltaTime);
            currentFPS = 1.0f / (Time.unscaledDeltaTime);
            averageFPS = currentFrame / currentTime;
            averageText.text = "Average FPS: " + averageFPS.ToString("F2");

            //Max/Min FPS for current day
            if (currentFPS < minimumFPS)
            {
                //Avoid anomalous readings
                if (currentFPS > 5.0f)
                {
                    minimumFPS = currentFPS;
                    minimumText.text = "Minimum FPS: " + minimumFPS.ToString("F2");
                }         
            }

            if (currentFPS > maximumFPS)
            {
                //Avoid anomalous readings
                if (currentFPS < 120.0f)
                {
                    maximumFPS = currentFPS;
                    maximumText.text = "Maximum FPS: " + maximumFPS.ToString("F2");
                }
            }

            ////Current Memory Usage
            //System.GC.Collect();
            //currentMemory = System.GC.GetTotalMemory(true) / 1000000.0f;
            //currentMText.text = "Current Memory: " + currentMemory.ToString("F2") + " MB";
            ////Average Memory Usage
            //currentMemTotal += currentMemory;
            //averageMemory = currentMemTotal / currentFrame;
            //averageMText.text = "Average Memory: " + averageMemory.ToString("F2") + " MB";
            ////Min and Max Memory Usage
            //if (currentMemory < minimumMemory)
            //{
            //    minimumMemory = currentMemory;
            //    minimumMText.text = "Minimum Memory: " + minimumMemory.ToString("F2") + " MB";
            //}
            //if (currentMemory > maximumMemory)
            //{
            //    maximumMemory = currentMemory;
            //    maximumMText.text = "Maximum Memory: " + maximumMemory.ToString("F2") + " MB";
            //}
            
            if (currentDay != TimeHandler.currentDay)
            {
                ResetDay();
                return;
            }
        }

        if (firstUpdate)
        {
            frameCount++;

            //Avoid FPS drops from initial spawning
            if (frameCount > 5)
            {
                firstUpdate = false;
                return;
            }
        }

    }

    private void ResetDay()
    {
        currentDay = TimeHandler.currentDay;
        currentFrame = 0;
        currentTime = 0.0f;
        currentMemTotal = 0.0f;
        minimumFPS = Mathf.Infinity;
        maximumFPS = 0.0f;
        maximumMemory = 0.0f;
        minimumMemory = Mathf.Infinity;
        TimeHandler.PauseGame();
    }
}
