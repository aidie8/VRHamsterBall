using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardMovement : MonoBehaviour
{

    private int speed = 50000;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool forward = Input.GetKey("up");
        bool backward = Input.GetKey("down");
        bool left = Input.GetKey("left");
        bool right = Input.GetKey("right");
        if (forward)
        {
            Vector3 dir = new Vector3(speed, 0, 0);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Acceleration);
        }
        else if (backward)
        {
            Vector3 dir = new Vector3(-speed, 0, 0);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Acceleration);
        }
        else if (left)
        {
            Vector3 dir = new Vector3(0, 0, speed);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Acceleration);
        }
        else if (right)
        {
            Vector3 dir = new Vector3(0, 0, -speed);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Acceleration);
        }
    }
}
