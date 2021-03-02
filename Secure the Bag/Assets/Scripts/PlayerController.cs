using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    public int laneNumber = 0;
    public int lanesCount = 2;

    bool didChangeLastFrame = false;

    // These variables deal with lane position and change
    public float laneDistance = 10; // dist. between lanes
    public float firstLaneXPos = 0; // the x coordinate of the starting lane
    public float deadZone = 0.1f; // if a value is greater than this the player can perform an action
    public float sideSpeed = 5; // how quickly a player changes lanes

    // these variables hand jumping
    public float jumpHeight = 2.5f; //how high the player jumps
    public float jumpForce = 5; // how quickly they jump
    public float gravity; // the force pulling them to the ground
 

    public Vector3 objectVelocity; // this will handle how fast objects move
    public Vector3 jump; //

    private Rigidbody rigg;



    void Start() // Start is called before the first frame update
    {

        // This controls the character animations
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rigg = GetComponent<Rigidbody>();
        jump = new Vector3(0f, jumpHeight, 0f);

    }


    void Update() // Update is called once per frame
    {
        Vector3 currentPosition = transform.position; //the current position of the player 

        //add a gui element to start the game whena button is pressed then begin the animations
        animator.SetBool("isRunning", Input.GetAxisRaw("Vertical") != 0); // running animation runs when the player starts playing

        //"Horizontal" is a default input axis set to arrow keys and A/D
        //We want to check whether it is less than the deadZone instead of whether it's equal to zero 
        float changeLanes = Input.GetAxis("Horizontal");

        if (Mathf.Abs(changeLanes) > deadZone)
        {
            if (!didChangeLastFrame)
            {
                didChangeLastFrame = true; //Prevent overshooting lanes
                laneNumber += Mathf.RoundToInt(Mathf.Sign(changeLanes));
                if (laneNumber < -1) laneNumber = -1;
                else if (laneNumber >= lanesCount) laneNumber = lanesCount - 1;
            }
        }
        else
        {
            didChangeLastFrame = false;
            //The user hasn't pressed a direction this frame, so allow changing directions next frame.
        }

        float jumpKey = Input.GetAxis("Vertical");

        if (Mathf.Abs(jumpKey) > deadZone && rigg.velocity.y == 0)
        {
            //animator.SetBool("isJumping", !characterController.isGrounded);

            rigg.AddForce(jump * jumpForce, ForceMode.Impulse);
            //Debug.Log(test);
        }


        currentPosition.x = Mathf.Lerp(currentPosition.x, firstLaneXPos + laneDistance * laneNumber, Time.deltaTime * sideSpeed);
        transform.position = currentPosition;


    }
}
