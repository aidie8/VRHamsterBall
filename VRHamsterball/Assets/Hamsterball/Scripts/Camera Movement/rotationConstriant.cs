using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationConstriant : MonoBehaviour
{


    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Console.WriteLine(player.transform.rotation);
        player.transform.rotation = player.GetComponentInParent<Rigidbody>().transform.rotation;
    }
}
