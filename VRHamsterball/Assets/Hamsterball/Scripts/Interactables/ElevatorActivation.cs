using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivation : MonoBehaviour
{


    public int maxheight = 10;
    public float speed = 0.0005f;
    //private variables 
    private bool atTop = false;
    private bool activated = false;
    private Vector3 intialpos;
    private Vector3 maxspeed;
    // Start is called before the first frame update
    void Start()
    {
        maxspeed = new Vector3(0, speed);
        print(maxspeed);
        intialpos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //print("is this true " + (this.transform.localPosition.y < maxheight + this.transform.position.y));
        if (activated)
        {
           print("intial pos "+intialpos.y);
            print("max height " + ((maxheight - intialpos.y)));
            //print("is this true " + (this.transform.localPosition.y < maxheight + this.transform.position.y));
            print(this.transform.localPosition.y);
            if (this.transform.localPosition.y < (maxheight-intialpos.y))
            {
                //print("current pos " + this.transform.localPosition.y);
                this.transform.position += maxspeed;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        print("yes");
        if (collision.gameObject.tag == "Player") {
            activated = true;
            print("this worked");
        }
    }
}
