using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public float timeScaleChangeIncrement;

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            TimeHandler.PauseGame();
        }

        if (Input.GetButtonDown("ChangeSpeedUp"))
        {
            TimeHandler.ChangeTimeScale(timeScaleChangeIncrement);
        }
        else if (Input.GetButtonDown("ChangeSpeedDown"))
        {
            TimeHandler.ChangeTimeScale(-timeScaleChangeIncrement);
        }
    }
}
