using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLerpTest : MonoBehaviour
{
    public double speed = 0.5d;
    public Vector3 endpos;
    private double time;
    private Vector3 startpos;
    private Vector3 originpos;
    private Quaternion startangle;
    private Quaternion endangle;
    private float distance;
    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        startpos = this.transform.position;
        originpos = startpos;
        endpos = startpos + new Vector3(-10, -10, 0);
        time = Time.time;
        distance = Vector3.Distance(startpos, endpos);
        startangle = this.transform.rotation;
        endangle = new Quaternion(0,0,-90,0);

    }

    // Update is called once per frame
    void Update()
    {
        print(pressed);
        if (pressed)
        {
            transform.position = Vector3.Lerp(this.transform.position, endpos, (float)getFraction());
            transform.rotation = Quaternion.Lerp(this.transform.rotation, endangle, (float)getFraction());
        }
        else
        {
            transform.position = Vector3.Lerp(this.transform.position, startpos, (float)getFraction());
            transform.rotation = Quaternion.Lerp(this.transform.rotation,startangle, (float)getFraction());
        }
        if (getFraction() >= 1f)
        {
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

    double getDistanceCovered()
    {
        return (Time.time - time) * speed;


    }

    double getFraction()
    {
        return getDistanceCovered() / distance;
    }


    public void onTargetchange()
    {
        resetTime();
        setStartPos();
    }

    public void SetEndPos(int num)
    {
        onTargetchange();
        switch (num)
        {


           
        }


    }




    public void setStartPos()
    {
        startpos = this.transform.position;
    }

    public void setStartRotation() {
        
    }
}

