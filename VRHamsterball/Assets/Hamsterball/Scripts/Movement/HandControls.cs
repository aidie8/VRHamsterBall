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
    private Vector3 grabPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (getGrab())
        {
            print("Trigger Pressed");
        }
        print("triggers current state" + getTriggerState());

        if (getGrab())
        {
            RaycastHit hit;
            Ray forward = new Ray(controller.transform.position, controller.transform.forward);
            forward.origin = forward.GetPoint(2);
            forward.direction = -forward.direction;
            Debug.DrawLine(forward.origin,controller.transform.position);
            pointerObject.transform.position = forward.origin;
            print(forward.origin);
            if (Physics.Raycast(forward, out hit))
            {
                print(hit.transform.gameObject);
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

            if (!getTriggerState())
            {
                this.GetComponent<FixedJoint>().connectedBody = null;
                this.GetComponent<FixedJoint>().connectedAnchor = new Vector3(0, 0, 0);
            }
        }
        bool getGrab()
        {
            return grabAction.GetStateDown(handType);

        }

        bool getTriggerState()
        {
            return grabAction.GetState(handType);
        }
    }
}
