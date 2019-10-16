using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskTypes { Work, Sleep, Eat, Joy, Social, Travel};

public class Task_Base{
    public TimeOfDay startTime; //Standard Work times
    public TaskTypes taskType;
}
