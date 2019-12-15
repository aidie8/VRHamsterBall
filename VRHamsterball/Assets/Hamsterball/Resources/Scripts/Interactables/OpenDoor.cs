using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator Animator;
    bool button = false;
    void Start()
    {
        Animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
//        print(button);
        if (button) {
            Animator.SetBool("Open", true);

        }
        // Animator.SetBool("Open", button);
        if (!button) {
            Animator.SetBool("Open", false);
            Animator.enabled = true;
        }
        
    }

    public void DoorOpened() {
        Animator.enabled = false;
    }

    public void ButtonPressed() {
        button = true;
    }

    public void ButtonReleased() {
        button = false;
    }
}
