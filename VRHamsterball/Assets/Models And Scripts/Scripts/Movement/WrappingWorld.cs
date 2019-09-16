using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public int MinimumHeight = -50;
    public int TeleportHeight = 100;
    public float TpX;
    public float TpY;
    public float TpZ;


    void Start()
    {
        TpY = this.transform.position.y;
        TpX = this.transform.position.x;
        TpZ = this.transform.position.z;
    } 

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < MinimumHeight)
        {
            this.transform.position = new Vector3(TpX, TpY, TpZ);
        }
            
    }
}
