using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Task { sleep, work, joy, interaction, relax};

public class DefaultScheduleBase : MonoBehaviour
{
    public Task[] tasks = new Task[24];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
