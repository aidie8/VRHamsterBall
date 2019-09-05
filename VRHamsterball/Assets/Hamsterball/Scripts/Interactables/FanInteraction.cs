using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanInteraction : MonoBehaviour
{
    public float strength = 5.5f;
    public float range = 10f;
    //private variables
    public float radius;
    public Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = GetComponentInParent<MeshRenderer>().bounds.size;
        radius = size.z/2;
        print(radius);
        Console.WriteLine("radius is before this");
      
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Vector3 orgin = this.transform.position;
        Vector3 forward = this.transform.right;
        // if (Physics.Raycast(orgin, forward,out hit, range)) {
        if (Physics.SphereCast(orgin, radius, forward, out hit, range))
        {
            if (hit.collider.gameObject.tag == "Player") {
            Debug.DrawLine(orgin, hit.point, Color.black);
            print(hit.point);
            print(hit.rigidbody.name);
            Vector3 stuff = Vector3.Scale(((hit.rigidbody.position - this.transform.position)), forward.normalized) / range * strength;
            hit.rigidbody.AddForceAtPosition(forward.normalized * strength, hit.point, ForceMode.Impulse);
        } }
       // }



    }
}
 