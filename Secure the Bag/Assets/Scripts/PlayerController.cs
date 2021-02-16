using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [SerializeField]
    private float movementSpeed, rotationSpeed, jumpSpeed, gravity;


    //render item if in fog of war
    //get position of charater in the world
    //place objects based a rule set
        // left block !next to a right block  || two blocks cannot be next to eachother
    // how often an object spawns 
    // if there is an object at position x already
    // detect if player hitbox collides with object hitbox
    //

    private Vector3 movementDirection = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the Character
        Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");
        characterController.Move(inputMovement * Time.deltaTime);
        //transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * rotationSpeed);
        /*if (Input.GetButton("Left") && characterController.isGrounded)
        {

        }*/
        // This controlls jumping
        if(Input.GetButton("Jump") && characterController.isGrounded)
        {
            movementDirection.y = jumpSpeed;
        }

        movementDirection.y -= gravity * Time.deltaTime;

        characterController.Move(movementDirection * Time.deltaTime);


        // Controlls Animaition
        animator.SetBool("isRunning", Input.GetAxisRaw("Vertical") != 0);
        animator.SetBool("isJumping", !characterController.isGrounded);
    }
}
