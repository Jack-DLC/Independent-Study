using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController; // used to move the player 
    private Animator animator; // used to handle animations
    private Rigidbody rigg; // A reference to the rigidbody of the player

    bool didChangeLastFrame;

    //public float gravity;
    public float jumpForce; // the vertical force added to the player
    private bool onGround; // check if the player is on the ground or not

    // These variables deal with lane position and change
    public int laneNumber = 0; // the lane the player is currently
    
    // determines how many lanes the player can move between, there are actualy three but this works 
    // around a bug
    public int lanesCount = 2; 
    public float laneDistance = 10; // distance between lanes
    public float firstLaneXPos = 0; // the x coordinate of the starting lane
    public float deadZone = 0.1f; // if an input value is greater than this, the player can perform an action
    public float sideSpeed = 5f; // how quickly a player changes lanes
    public float runningSpeed; // the running speed of the player    

    void Start() // Start is called before the first frame update
    {
        //animator = GetComponent<Animator>(); // used for setting the animations
        rigg = GetComponent<Rigidbody>(); // Not sure if i need this anymore
        rigg.constraints = RigidbodyConstraints.FreezeRotation;

        //  these variables reset when the game begins
        onGround = true;
        runningSpeed = 11f;
        didChangeLastFrame = false;
    }

    
    void Update()
    {
        //"Horizontal" is a default input axis set to arrow keys and A/D
        //We want to check whether it is less than the deadZone instead of whether it's equal to zero 
        float horizontalInput = Input.GetAxis("Horizontal");

        // if the player hits "a", "d", left arrow, or right arrow the character will change lanes
        if (Mathf.Abs(horizontalInput) > deadZone)
        {
            if (!didChangeLastFrame)
            {
                didChangeLastFrame = true; //Prevent overshooting lanes
                laneNumber += Mathf.RoundToInt(Mathf.Sign(horizontalInput));
                if (laneNumber < -1) laneNumber = -1;
                else if (laneNumber >= lanesCount) laneNumber = lanesCount - 1;
            }
        }      
        else
        {
            //The user hasn't pressed a direction this frame, so allow changing directions next frame.
            didChangeLastFrame = false;
        }
        
        // if the player hits "w" or up arrow, a vertical force is added to the rigidbody to make the player jump
        if (Input.GetAxis("Vertical") > deadZone && onGround == true)
        {
            rigg.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false; // prevent double jumping
        }
        
        Vector3 playerPosition = transform.position; //get the current position of the player 
        
        // ensures player can jump without losing their vertical and horizontal velocity
        rigg.velocity = new Vector3(0, rigg.velocity.y, runningSpeed); 
        playerPosition.x = Mathf.Lerp(playerPosition.x, firstLaneXPos + laneDistance * laneNumber, Time.deltaTime * sideSpeed);
        transform.position = playerPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // when player collides with the ground, set onGround to true
        if (collision.collider.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    // used to increase player speed
    public void SpeedMultiplier ()
    {
        if (transform.position.z % 50 == 0)
        {
            runningSpeed += runningSpeed * .1f;
        }
    }
}
