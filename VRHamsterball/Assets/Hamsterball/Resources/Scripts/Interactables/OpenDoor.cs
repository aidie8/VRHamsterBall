using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator Animator;
    void Start()
    {
        Animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorOpen()
    {
        Animator.Play("open");

    }

    public void DoorClose() {
        Animator.Play("close");
        

    }
}
