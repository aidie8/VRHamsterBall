using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public Text timertext;
    private float time;
    private bool TimerOn;

    private void Start()
    {

        TimerOn = true;

    }

    private void Update()
    {
        if (TimerOn)
        {
            time += Time.deltaTime;
            timertext.text = time.ToString("#.##");
        }
    }

    public void StopTimer()
    {

        TimerOn = false;

    }

}
