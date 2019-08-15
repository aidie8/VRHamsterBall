using UnityEngine;
using System.Collections;
using Valve.VR.Extras;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class MovementHamsterBall : MonoBehaviour
{
    public float Delay = .2f;
    [Tooltip("Set this to a prefab. It'll show up in-game to indicate your anchor and grip positions.")]
    public GameObject marker;

    Rigidbody _cachedBody;
    Rigidbody CachedBody
    {
        get
        {
            if (_cachedBody == null)
            {
                _cachedBody = GetComponent<Rigidbody>();
            }
            return _cachedBody;
        }
    }

    // This ILeverageProvider class provides two floats:
    //     ArmLengthToRadiusRatio and Radius.
    // It's useful if you hamster ball is changing size.
    // Otherwise, I just have it ArmLengthToRadiusRatio be 0.5 and
    // Radius be, well, the radius of the ball.

    [Tooltip("I usually have this map to the grip buttons or something")]
    public SteamVR_Action_Boolean GrabAction;

    [Tooltip("Usually two of these, one for each hand.")]
    public SteamVR_Behaviour_Pose[] trackThese;

    // One marker, one front anchor, one back anchor for each controller.
    GameObject[] markers;
    GameObject[] frontAnchors;
    GameObject[] backAnchors;

    // This array tracks if the user has opened their hand since the beginning of the level.
    // Part of a way to fix the player getting thrown around if they start the level grabbing on, before
    // the physics has settled.
    bool[] opened;

    bool[] heldLastFrame;
    float delayUntil = 0f;

    // Use this for initialization
    void Start()
    {
        // Creating the data to track for each controller.
        // Really, instead of multiple arrays of trackThese.Length, this should
        // be one array of objects that have each of these as fields.
        heldLastFrame = new bool[trackThese.Length];
        markers = new GameObject[trackThese.Length];
        frontAnchors = new GameObject[trackThese.Length];
        backAnchors = new GameObject[trackThese.Length];
        opened = new bool[trackThese.Length];
        for (uint i = 0; i < trackThese.Length; i++)
        {
            
            heldLastFrame[i] = false;

            markers[i] = Instantiate(marker as GameObject);
            markers[i].SetActive(false);
            markers[i].transform.parent = transform;
            print(markers[i]);
            // frontAnchors[i] = new GameObject("Front Anchor");
            frontAnchors[i] = Instantiate(marker as GameObject);
            frontAnchors[i].AddComponent<Rigidbody>().isKinematic = true;
            frontAnchors[i].GetComponent<Rigidbody>().solverIterations = 20;

            //backAnchors[i] = new GameObject("Back Anchor");
            backAnchors[i] = Instantiate(marker as GameObject);
            backAnchors[i].AddComponent<Rigidbody>().isKinematic = true;
            backAnchors[i].GetComponent<Rigidbody>().solverIterations = 20;

            opened[i] = false;
        }

        GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
        GetComponent<Rigidbody>().solverIterations = 20;
    }


    void Awake()
    {
        delayUntil = Time.time + Delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < delayUntil) return;
        for (uint i = 0; i < trackThese.Length; i++)
        {
            SteamVR_Behaviour_Pose checkMe = trackThese[i];
            if (checkMe == null || !checkMe.gameObject.activeSelf)
            {
                break;
            }
            // Project from the center of the sphere, through the controller, to the surface of the sphere.
            Vector3 localPointOnSphere = transform.InverseTransformPoint(checkMe.transform.position).normalized * getArmLength();
            // That projection is now the position of the 'front' anchor.
            frontAnchors[i].GetComponent<Rigidbody>().MovePosition(transform.TransformPoint(localPointOnSphere));
            // The 'back' anchor gets moved to the opposite side.
            backAnchors[i].GetComponent<Rigidbody>().MovePosition(transform.TransformPoint(-1 * localPointOnSphere));
            if (opened[i])
            {
                // Don't want to do this until the user has let go at least once, to avoid
                // weird situations if they start the level holding onto the grip buttons.

                if (!heldLastFrame[i] && GrabAction.GetState(checkMe.inputSource))
                {
                    // The player wasn't gripping before, but is now.
                    print("Grip start! At arm length: " + getArmLength());

                    // Move the markers into place and show them.
                    markers[i].SetActive(true);
                    markers[i].transform.localPosition = transform.InverseTransformPoint(frontAnchors[i].transform.position);

                    // Create springs
                    AddJoint(frontAnchors[i]);
                    AddJoint(backAnchors[i]);
                }
                if (heldLastFrame[i] && !GrabAction.GetState(checkMe.inputSource))
                {
                    // The player was gripping before, but stopped

                    // Remove the springs
                    foreach (Joint breakMe in frontAnchors[i].GetComponents<Joint>())
                    {
                        if (breakMe.connectedBody == CachedBody)
                        {
                            Destroy(breakMe);
                        }
                    }
                    foreach (Joint breakMe in backAnchors[i].GetComponents<Joint>())
                    {
                        if (breakMe.connectedBody == CachedBody)
                        {
                            Destroy(breakMe);
                        }
                    }

                    // Hide the marker.
                    markers[i].SetActive(false);
                    print("Grip end!");
                }
            }
            heldLastFrame[i] = GrabAction.GetState(checkMe.inputSource);
            if (heldLastFrame[i] == false)
            {
                opened[i] = true;
            }
        }
    }

    // Creates a spring between 'go' and this.gameObject
    // The start and end of the spring should both be at the current position of 'go'.
    void AddJoint(GameObject go)
    {
        var newJoint = go.AddComponent<SpringJoint>();
        newJoint.connectedAnchor = transform.InverseTransformPoint(go.transform.position);
        newJoint.connectedBody = CachedBody;
        newJoint.spring = 1500;
        newJoint.breakForce = float.PositiveInfinity;
        newJoint.breakTorque = float.PositiveInfinity;
        newJoint.maxDistance = getArmLength() / 4f;
    }

    float getArmLength()
    {
        float radius = this.GetComponent<SphereCollider>().radius;

        return radius;
    }
}