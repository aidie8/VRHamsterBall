using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class something : MonoBehaviour
{

    public Text timertext;
    private float time;

    private void FixedUpdate()
    {

        time += Time.deltaTime;
        timertext.text = time.ToString();

    }

}
