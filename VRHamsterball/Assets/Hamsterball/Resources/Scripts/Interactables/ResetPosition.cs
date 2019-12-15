using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{

    private Vector3 StartPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ResetPostion() {
        this.transform.position = StartPosition;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }
}
