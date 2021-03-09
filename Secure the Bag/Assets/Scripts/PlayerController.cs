using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController; // used to move the player 
    private Animator animator; // used to handle animations
    private Rigidbody rigg; // A reference to the rigidbody of the player

    bool didChangeLastFrame = false; 

    public float jumpHeight = 2.5f; //how high the player jumps
    public float jumpForce = 5; // how quickly they jump
    public float gravity; // the force pulling them to the ground

    // These variables deal with lane position and change
    public int laneNumber = 0; // the lane the player is currently
    public int lanesCount = 2; // there are actually 
    public float laneDistance = 10; // dist. between lanes
    public float firstLaneXPos = 0; // the x coordinate of the starting lane
    public float deadZone = 0.1f; // if a value is greater than this the player can perform an action
    public float sideSpeed = 5; // how quickly a player changes lanes
    public float runningSpeed; // how quickly a player changes lanes     

    void Start() // Start is called before the first frame update
    {

        // This controls the character animations
        characterController = GetComponent<CharacterController>(); // used for animations
        animator = GetComponent<Animator>(); // used for setting the animations
        rigg = GetComponent<Rigidbody>(); // Not sure if i need this anymore
    }


    void Update()
    {
        //"Horizontal" is a default input axis set to arrow keys and A/D
        //We want to check whether it is less than the deadZone instead of whether it's equal to zero 
        float input = Input.GetAxis("Horizontal");

        // if the player hits "a", "d", left arrow, or right arrow the character will change lanes
        if (Mathf.Abs(input) > deadZone)
        {
            if (!didChangeLastFrame)
            {
                didChangeLastFrame = true; //Prevent overshooting lanes
                laneNumber += Mathf.RoundToInt(Mathf.Sign(input));
                if (laneNumber < -1) laneNumber = -1;
                else if (laneNumber >= lanesCount) laneNumber = lanesCount - 1;
            }
        }
        
        else
        {
            didChangeLastFrame = false;
            //The user hasn't pressed a direction this frame, so allow changing directions next frame.
        }
        

        
        if (Input.GetAxis("Horizontal") > deadZone)
        {
            rigg.AddForce(0, jumpForce, 0);
        }
        

        rigg.velocity = new Vector3(0, 0, runningSpeed);// jumping
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, firstLaneXPos + laneDistance * laneNumber, Time.deltaTime * sideSpeed);
        transform.position = pos;
    }
    
}
