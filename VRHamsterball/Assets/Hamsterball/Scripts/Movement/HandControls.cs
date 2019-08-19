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
    public GameObject rayStart;
    //private variables
    private GameObject pointerball;
    private Vector3 grabPos;
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        grabAction.AddOnStateDownListener(TriggerPressed, handType);
        grabAction.AddOnStateUpListener(TriggerReleased, handType);
    }


    // Update is called once per frame
    void Update()
    {
        /** if (getGrab())
         {
             
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
         }**/

    }

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        /**
        ray = new Ray(rayStart.transform.position, rayStart.transform.forward);
        Ray forward = new Ray(rayStart.transform.position, rayStart.transform.forward);
        forward.origin = forward.GetPoint(10);
        Debug.DrawLine(forward.origin, controller.transform.position, Color.blue);
        forward.direction = -forward.direction;
        Debug.DrawLine(forward.origin, controller.transform.position, Color.red);
        pointerObject.transform.position = forward.origin;
        RaycastHit hit;
        **/

        
        ray = new Ray(rayStart.transform.position, -rayStart.transform.forward);
        Ray forward = new Ray(rayStart.transform.position, rayStart.transform.forward);
        forward.origin = forward.GetPoint(3);
        Debug.DrawLine(forward.origin, controller.transform.position, Color.blue);
        forward.direction = -forward.direction;
        Debug.DrawLine(forward.origin, controller.transform.position, Color.red);
        pointerObject.transform.position = forward.origin;
        RaycastHit hit;
        if (Physics.Raycast(forward, out hit, 6f, 9))
        {
            print(hit.transform.gameObject);
            if (hit.transform.gameObject == ball)
            {
                pointerObject.transform.position = hit.transform.forward;
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


    private void TriggerReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ray = new Ray(new Vector3(0, 0, 0),Vector3.forward);
        this.GetComponent<FixedJoint>().connectedBody = null;
        this.GetComponent<FixedJoint>().anchor = Vector3.zero;
        print("Trigger Released");
    }
}
