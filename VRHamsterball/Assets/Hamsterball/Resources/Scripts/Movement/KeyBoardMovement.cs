using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardMovement : MonoBehaviour
{

    public int speed = 50;
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
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
        }
        if (backward)
        {
            Vector3 dir = new Vector3(-speed, 0, 0);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
        }
        if (left)
        {
            Vector3 dir = new Vector3(0, 0, speed);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
        }
        if (right)
        {
            Vector3 dir = new Vector3(0, 0, -speed);
            this.GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
        }
    }
}
