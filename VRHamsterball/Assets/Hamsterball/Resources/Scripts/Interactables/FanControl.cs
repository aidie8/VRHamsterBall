using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationalSpeed = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(this.transform.localPosition, rotationalSpeed * Time.deltaTime);
    }
}
