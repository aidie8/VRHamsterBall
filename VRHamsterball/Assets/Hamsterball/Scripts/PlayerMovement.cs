using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody ball;
    public Player player;
    Player playerScript;
    bool up;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //transforms player to the ball every update,makes the player down a bit in the ball. can be ajusted to work better
        playerScript.trackingOriginTransform.position = ball.transform.position - new Vector3(0, ball.GetComponent<SphereCollider>().radius / 2);
            Vector3 dir = new Vector3(0, 0, 9);
            ball.AddRelativeTorque(dir,ForceMode.Impulse);

        }
    }
