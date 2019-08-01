using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandControls : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Input_Sources handType;
    public GameObject ball;
    public Hand controller;
    public GameObject pointerObject;
    public Player player;
    //private variables
    private GameObject pointerball;
    private bool ballGrabbed;
    private Vector3 grabPos;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        if (getGrab()) {
            print("Trigger Pressed");
        }
           print("triggers current state" + getTriggerState());
        
        if (!ballGrabbed && getGrab()) {
            ballGrabbed = true;
            RaycastHit hit;
            if (Physics.Raycast(controller.transform.position, controller.transform.TransformDirection(Vector3.forward), out hit))
            {
                if (hit.transform.gameObject == ball)
                {
                    Debug.Log("ray hit");
                    pointerObject.transform.position = hit.transform.position;

                    this.GetComponent<FixedJoint>().connectedBody = ball.GetComponent<Rigidbody>();
                    this.GetComponent<FixedJoint>().connectedAnchor = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position;
                    print("1");
                    print("3" + this.GetComponent<FixedJoint>().connectedAnchor);
                    print("2");
                    print(this.GetComponent<FixedJoint>().connectedBody);
                }
            }
        }
        if (!getGrab()) {
            this.GetComponent<FixedJoint>().connectedBody = null;
            this.GetComponent<FixedJoint>().connectedAnchor = new Vector3(0, 0, 0);
            ballGrabbed = false;
        }
    }
    bool getGrab()
    {
        return grabAction.GetStateDown(handType);

    }

    bool getTriggerState() {
        return grabAction.GetState(handType);
    }
}
