using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class something : MonoBehaviour
{

    public Text timertext;
    private float time;
    public bool Timer;

    private void Start()
    {

        Timer = false;
          
    }

    private void FixedUpdate()
    {

        if (Timer)
        {

            time += Time.deltaTime;
            timertext.text = time.ToString("0.00");

        }

    }

}
