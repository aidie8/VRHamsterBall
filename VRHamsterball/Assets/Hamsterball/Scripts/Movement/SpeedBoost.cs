using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{


    public GameObject Ball;
    public float speedmultiplier = 10.0f;


    //private variables
    private int cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0) {
            cooldown--;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        print("collisions");
        if (cooldown <= 0) {
        cooldown = 20;
            Vector3 boost = this.transform.up * speedmultiplier;
            Ball.GetComponent<Rigidbody>().AddForce(boost,ForceMode.VelocityChange);
        }
    }
}
