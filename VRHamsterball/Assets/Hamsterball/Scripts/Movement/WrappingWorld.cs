using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public int MinimumHeight = -50;
    public int TeleportHeight = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<GameObject>().transform.position.y < MinimumHeight)
        {
            this.GetComponent<GameObject>().transform.position= new Vector3 (0, TeleportHeight, 0);
        }
            
    }
}
