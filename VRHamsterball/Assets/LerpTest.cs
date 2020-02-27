using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public double speed = 0.5d;
    public Vector3 endpos;
    private double time;
    private Vector3 startpos;
    private Vector3 origin;
    private float distance;
    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        startpos = this.transform.position;
        origin = startpos;
        endpos = startpos + new Vector3(0, 4, 0);
        time = Time.time;
        distance = Vector3.Distance(startpos, endpos);

    }

    // Update is called once per frame
    void Update()
    {
        print(pressed);
        if (pressed)
        {
            transform.position = Vector3.Lerp(this.transform.position, endpos, (float)getFraction());
        }
        else {
            transform.position = Vector3.Lerp(this.transform.position, startpos, (float)getFraction());
        }
        if (getFraction() >= 1f) {
            resetTime();
        }

    }


    public void buttonpressed()
    {
        pressed = true;
    }

    public void buttonrelease()
    {
        pressed = false;

    }

    void resetTime()
    {

        time = Time.time;
    }

    double getDistanceCovered() {
        return (Time.time - time) * speed;


    }

    double getFraction() {
        return getDistanceCovered() / distance;
    }
    

    public void onTargetchange() {
        resetTime();
        setStartPos();
    }

    public void SetEndPos(int num) {
        onTargetchange();
        switch (num) {


            case 0:
                endpos = origin;
                break;
            case 1:
               endpos = origin + new Vector3(4,0,0);
                break;
            case 2:
                endpos = origin + new Vector3(4, 4, 0);
                break;
            case 3:
                endpos = origin + new Vector3(0, 4, 0);
                break;
            default:
                endpos = origin;
                break;
                
        }
       

    }

   


    public void setStartPos() {
        startpos = this.transform.position;
    }
}