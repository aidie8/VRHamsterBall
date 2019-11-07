using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;

public class VRPresentCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isenabled = true;
    public GameObject todisable;
    void Start()
    {
        if (!XRDevice.isPresent) {
            todisable.SetActive(false);
            isenabled = false;
        }
    }
}
