using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveableButton : MonoBehaviour
{
    public UnityEvent ButtonPressed;
    public UnityEvent ButtonReleased;
    public float maxOffset = .6f;
    //public bool ReverseDirection;
    private Vector3 intialPos;
    private bool pressed = false;
    // Start is called before the first frame update
    void Start()
    { 
        intialPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        double diff = Math.Round((Mathf.Abs(transform.localPosition.y - intialPos.y)),3);
        //print(diff);
        if (diff > maxOffset && !pressed)
        {
            pressed = true;
            ButtonPressed.Invoke();
        }
        else if  (diff < maxOffset && pressed)
            {
            
                ButtonReleased.Invoke();
                pressed = false;
            
            
            }
        }


    public bool getPressed() {
        return pressed;
    }
}
