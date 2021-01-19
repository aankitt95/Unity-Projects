//make sure to add a CharacterController to the thing that you want to move
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float speed = 9.0f;
    Animator animator;


    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //animator = this.gameObject.GetComponent<Animator>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical) * (speed * Time.deltaTime));
        


        if (characterController.isGrounded)
        {

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
            
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
     
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        if(Input.GetKey("left") && Input.GetKey("left shift") || Input.GetKey("right") && Input.GetKey("left shift") || Input.GetKey("up") && Input.GetKey("left shift") || Input.GetKey("down") && Input.GetKey("left shift"))
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

    }
}