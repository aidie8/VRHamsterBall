using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    Vector3 offset;
    public GameObject followObject;
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followObject.transform.position + offset;
    }
}
