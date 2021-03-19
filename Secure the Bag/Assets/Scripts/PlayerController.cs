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
    Vector3 playerPosition;


    public float gravity = -9.8f;
    public float jumpForce; // how quickly they jump
    private bool onGround;

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

        //animator = GetComponent<Animator>(); // used for setting the animations

        onGround = true;
        rigg = GetComponent<Rigidbody>(); // Not sure if i need this anymore
        rigg.constraints = RigidbodyConstraints.FreezeRotation;
        //playerPosition = transform.position;
        playerPosition = new Vector3(0, 4.23f, 0);
    }


    void Update()
    {
        playerPosition = transform.position;
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

        //jump
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onGround)
        {
            //Vector3 upForce = Vector3.up * jumpForce;
            //Vector3 horizontalForce = rigg.GetPointVelocity(transform.position);
            //rigg.AddForce(upForce + horizontalForce, ForceMode.Impulse);
           rigg.AddForce( Vector3.up * jumpForce, ForceMode.Impulse);
           onGround = false;
        }

        if (!onGround)
        {
            //playerPosition.y -= gravity * Time.deltaTime;
        }

        rigg.velocity = new Vector3(0,0,runningSpeed);
        playerPosition.x = Mathf.Lerp(playerPosition.x, firstLaneXPos + laneDistance * laneNumber, Time.deltaTime * sideSpeed);
        transform.position = playerPosition;
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("true");

        if (collision.gameObject.CompareTag("Ground"))
        {
            
            onGround = true;
        }
    }
}