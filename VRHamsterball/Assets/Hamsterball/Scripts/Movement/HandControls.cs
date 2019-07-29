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
        
        if (!ballGrabbed && getGrab()) {
            ballGrabbed = true;
            RaycastHit hit;
            if (Physics.Raycast(controller.transform.position, controller.transform.TransformDirection(Vector3.forward),out hit)) {
                Debug.Log("ray hit");
                pointerball = Instantiate(pointerObject);
                pointerObject.transform.position = hit.transform.position;
                this.GetComponent<SpringJoint>().connectedBody = ball.GetComponent<Rigidbody>();
                this.GetComponent<SpringJoint>().connectedAnchor = hit.transform.position;
            }
        }
        if (!getGrab()) {
            this.GetComponent<SpringJoint>().connectedBody = null;
            //this.GetComponent<SpringJoint>().connectedAnchor = null;
        }
    }
    bool getGrab()
    {
        return grabAction.GetStateDown(handType);

    }
}
