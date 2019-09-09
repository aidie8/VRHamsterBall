using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{

    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Vector3 scale = Player.transform.localScale;
        GameObject parent = Player.transform.parent.gameObject;
        parent.transform.parent = transform.parent;
        //Player.transform.localScale = scale;
    }


    private void OnCollisionExit(Collision collision)
    {
        Player.transform.parent = null;
    }
}
