using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedType { Work, Sleep, Eat, Joy, Social}

public class NeedSupply : MonoBehaviour
{
    protected Need[] needsFulfilled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Need
{
    private NeedType type;

}


