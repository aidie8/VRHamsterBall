using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float moveSpeed = 0.05f;
    public float Relativeforce = 10f;
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
        this.transform.position += Direction.normalized * (moveSpeed * reversal);
    }



    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("COLLISION");
        Debug.Log(collision.gameObject);
        if (!collision.gameObject.tag.Equals("Player") && !collision.gameObject.Equals("Platform"));
        {
            reversal *= -1;

        }
    }
}
