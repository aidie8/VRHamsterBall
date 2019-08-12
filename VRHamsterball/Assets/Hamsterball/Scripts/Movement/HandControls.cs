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
    private LineRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        grabAction.AddOnStateDownListener(TriggerPressed, handType);
        grabAction.AddOnStateUpListener(TriggerReleased, handType);
        renderer = this.GetComponent<LineRenderer>();
        renderer.startColor = Color.red;
        renderer.endColor = Color.red;
        LineRenderer renderer2 = Instantiate(renderer);
    }


    // Update is called once per frame
    void Update()
    {

       /** if (getGrab())
        {
            RaycastHit hit;
            Ray forward = new Ray(rayStart.transform.position, -rayStart.transform.forward);
            forward.origin = forward.GetPoint(3);
            forward.direction = -forward.direction;
            Debug.DrawLine(forward.origin,controller.transform.position,Color.red);
            pointerObject.transform.position = forward.origin;
            if (Physics.Raycast(forward, out hit,6f,9))
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
        }**/

    }

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        print("trigger pressed");
        print("ray start pos " + rayStart.transform.position);
        Ray ray = new Ray(rayStart.transform.position, rayStart.transform.forward);
        renderer.positionCount = 2;
        renderer.SetPosition(0, rayStart.transform.position);
        renderer.SetPosition(1,ray.GetPoint(4));
    }


    private void TriggerReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        this.GetComponent<FixedJoint>().connectedBody = null;
        this.GetComponent<FixedJoint>().anchor = Vector3.zero;
        renderer.positionCount = 0;
        print("Trigger Released");
    }
}
