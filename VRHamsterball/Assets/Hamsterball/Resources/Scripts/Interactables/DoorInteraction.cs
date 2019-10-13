using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteration : MonoBehaviour
{
    
    public void OpenDoor() {
        this.enabled = false;
    }

    public void CloseDoor() {
        this.enabled = true;
    }
}
