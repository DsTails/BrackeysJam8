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
    private float runSpeed = 40f;

    //Declaring Photon View variable
    PhotonView view;

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
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jumping);
        jumping = false;
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
