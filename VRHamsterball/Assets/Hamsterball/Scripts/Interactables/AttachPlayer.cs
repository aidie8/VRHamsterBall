using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{

    public GameObject Player;


    private GameObject originalParent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player)
        {
            //Vector3 scale = Player.transform.localScale;
            originalParent = Player.transform.parent.gameObject;
            GameObject parent = Player.transform.parent.gameObject;
            parent.transform.parent = transform.parent;
            //Player.transform.localScale = scale;
        }
    }
    


    private void OnCollisionExit(Collision collision)
    {
        Player.transform.parent.transform.parent = null;
    }
}
