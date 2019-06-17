using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public Rigidbody ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      this.transform.position=ball.transform.position - new Vector3(0, ball.GetComponent<SphereCollider>().radius / 2);
    }
}
