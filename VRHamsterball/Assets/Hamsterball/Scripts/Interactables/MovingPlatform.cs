using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float moveSpeed = 0.05f;
    public Vector3 Direction;


    private int reversal = 1;
    // Start is called before the first frame update
    void Start()
    {
        Direction = this.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float distance = 0;
        float distance2 = 0;
        float finaldist = 0;
        float speed = moveSpeed;    
        Vector3 origin = transform.position + new Vector3(0, 0.3f);
        Vector3 direction = Direction * reversal;
        
        if (Physics.Raycast(origin, direction, out hit,10f)) {
            distance = hit.distance;
            Debug.DrawLine(origin, origin + (direction * hit.distance));
        }
        Vector3 reverseDirection = direction * -1;
        RaycastHit hit2;
        if (Physics.Raycast(origin, reverseDirection, out hit2,10f)) {
            distance2 = hit2.distance;
            Debug.DrawLine(origin, origin + (reverseDirection * hit2.distance));
        }
        Debug.DrawLine(origin, origin + (direction * 10),Color.red);

        if (distance > distance2)
        {
            if (distance2 != 0)
            {
                finaldist = distance2;
            }
            else { finaldist = distance; }
        }
        else if (distance < distance2) {
            if (distance != 0)
            {
                finaldist = distance;
            }
            else {
                finaldist = distance2;
            }
        }
        if (distance != distance2) {
            speed = moveSpeed * ((finaldist / 10));
           }

        if (speed == 0) speed = moveSpeed;
            this.transform.position += Direction.normalized * (speed * reversal); 
    }



    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (!collision.gameObject.tag.Equals("Player") && !collision.gameObject.Equals("Platform"))
        {
            reversal *= -1;

        }
    }
}
