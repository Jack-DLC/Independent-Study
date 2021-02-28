using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [SerializeField]
    private Vector3 Xposition;
    public float movementSpeed, Xincrement, jumpSpeed, gravity;

    private Vector3 movementDirection = Vector3.zero;


    
    void Start() // Start is called before the first frame update
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    
    void Update() // Update is called once per frame
    {
        // Vector2.MoveTowards() moves the player towards certain position instead of teleporting them there
        // Time.delteTime uses the time of the players computer to sync events. This reduces some performance issues on less poerful computers
        transform.position = Vector3.MoveTowards(transform.position, Xposition, movementSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow)) // if player has hit 'A' the Character moves left Xincrement units
        {
            Xposition = new Vector3(transform.position.x + Xincrement, transform.position.z, transform.position.y);
            transform.position = Xposition;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // if player hits 'S' the character moves left -Xincrement units
        {
            Xposition = new Vector3(transform.position.x - Xincrement, transform.position.z, transform.position.y);
            transform.position = Xposition;
        }


        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * movementSpeed);
        
        //An alternative movement mechanic
        //Vector3 inputMovement = transform.forward * movementSpeed * Input.GetAxisRaw("Vertical");
        //characterController.Move(inputMovement * Time.deltaTime);

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
