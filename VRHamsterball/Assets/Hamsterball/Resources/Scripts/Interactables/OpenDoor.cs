using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator Animator;
    bool button = false;
    public something timerscript;
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
            timerscript.Timer = false;

        }
        // Animator.SetBool("Open", button);
        if (!button) {
            Animator.SetBool("Open", false);
            Animator.enabled = true;
            timerscript.Timer = false;
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
