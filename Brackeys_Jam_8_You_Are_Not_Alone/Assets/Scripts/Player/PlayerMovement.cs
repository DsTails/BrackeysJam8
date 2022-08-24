using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class PlayerMovement : MonoBehaviour
{
    //Declaring controller
    private CharacterController2D controller;

    //declaring movement floats
    float horizontalMove = 0f;
    bool jumping = false;

    //Declaring Movement Variables
    [SerializeField]
    public float runSpeed = 40f;

    //Declaring Photon View variable
    PhotonView view;

    //Wall Jumping
    public bool canWallJump;
    public bool wallSliding;
    public float wallJumpTime;
    public float wallSlideSpeed;
    public float wallDistance;
    RaycastHit2D WallCheckHit;
    float JumpTime;

    // Start is called before the first frame update
    void Start()
    {
        //Getting Character controller
        controller = GetComponent<CharacterController2D>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            HandleMovement();
        }
    }

    void FixedUpdate()
    {
        if(GetComponent<PlayerType>().isBeingGrabbed || GetComponent<PlayerType>().isBeingThrown)
        {
            return;
        }

        if(controller.m_Grounded && GetComponent<PlayerType>().isBeingThrown)
        {
            GetComponent<PlayerType>().isBeingThrown = false;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jumping);
        jumping = false;

        //Wall jump logic
        if (canWallJump)
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance * (controller.m_FacingRight ? 1 : -1), 0f), wallDistance, controller.m_WhatIsGround);
            Debug.DrawRay(transform.position, new Vector2(wallDistance * (controller.m_FacingRight ? 1 : -1), 0f), Color.blue);

            if (WallCheckHit && !controller.m_Grounded && Input.GetAxisRaw("Horizontal") != 0)
            {
                wallSliding = true;
                JumpTime = Time.time + wallJumpTime;

            } else if(JumpTime < Time.time)
            {
                wallSliding = false;
            }

            if (wallSliding)
            {
                controller.m_Rigidbody2D.velocity = new Vector2(controller.m_Rigidbody2D.velocity.x, Mathf.Clamp(controller.m_Rigidbody2D.velocity.y, wallSlideSpeed, float.MaxValue));
            }
        }

    }

    #region ~Movement~
    //Handles Movement
    void HandleMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButton("Jump"))
        {
            jumping = true;
            Debug.Log("Jumping");
        }
    }
    #endregion
}
