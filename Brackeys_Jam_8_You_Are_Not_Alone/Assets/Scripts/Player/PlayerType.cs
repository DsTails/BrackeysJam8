using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTypes
{
    Heavy,
    Light
}


public class PlayerType : MonoBehaviour, IObjectGrabInterface
{
    public PlayerTypes type;
    public bool canGrabThrow;
    public bool isBeingGrabbed;
    public bool isGrabbing;
    public bool isBeingThrown;

    public LayerMask whatAreObjects;

    public Transform throwPoint;
    public Transform throwPivot;

    Vector2 throwDirection;
    public float grabDistance;
    public float throwForce;
    public GameObject grabPoint;
    GameObject[] grabPoints;
    public int numberOfPoints;
    public float spaceBetweenPoint;
    IObjectGrabInterface grabbedObject;
    GameObject foundObject;

    

    PlayerMovement movement;
    CharacterController2D controller;
    // Start is called before the first frame update
    void Start()
    {
        
        grabPoints = new GameObject[numberOfPoints];
        for(int i = 0; i < numberOfPoints; i++)
        {
            grabPoints[i] = Instantiate(grabPoint, throwPoint.position, Quaternion.identity);
            grabPoints[i].transform.parent = transform;
            grabPoints[i].gameObject.SetActive(false);
        }



        movement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController2D>();

        if(type == PlayerTypes.Light)
        {
            canGrabThrow = false;
            movement.canWallJump = true;
            movement.wallJumpTime = .2f;
            movement.wallDistance = .55f;
            movement.wallSlideSpeed = .7f;
        } else if(type == PlayerTypes.Heavy)
        {
            canGrabThrow = false;
            movement.canWallJump = false;
            movement.runSpeed = 20f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For each input, check the player's type
        //Call the method corresponding to the type
        if(type == PlayerTypes.Heavy)
        {
            CheckForHeavyActions();
        }
        else
        {
            CheckForLightActions();
        }

    }



    //Insert methods for each player type, depending on what they do/can do
    #region Heavy Player Methods
    public void CheckForHeavyActions()
    {
        Vector2 startThrowPos = throwPivot.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        throwDirection = mousePos - startThrowPos;
        throwDirection= throwDirection * (controller.m_FacingRight ? 1 : -1);
        throwPivot.transform.right = throwDirection;

        if (Input.GetMouseButton(0))
        {
            if (!isGrabbing)
            {
                HeavyGrabObject();
            }
            else
            {
                

                //Render the throw arc line
                for(int i = 0; i < numberOfPoints; i++)
                {
                    if (!grabPoints[i].activeInHierarchy)
                    {
                        grabPoints[i].gameObject.SetActive(true);
                    }
                    grabPoints[i].transform.position = PointPosition(i * spaceBetweenPoint);
                }
            }
        } else if (Input.GetMouseButtonUp(0))
        {
            if (isGrabbing)
            {
                HeavyThrowObject();

                

                for(int i = 0; i < numberOfPoints; i++)
                {
                    grabPoints[i].gameObject.SetActive(false);
                }

            }
        }


    }


    public void HeavyGrabObject()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(throwPoint.position, throwDirection, grabDistance, whatAreObjects);
        Debug.DrawRay(throwPoint.position, throwDirection, Color.red);
        //Check if the object is a player/has the playerMovement/type script
        //If it does, then set a bool to prevent them from moving
        if(hit && controller.m_Grounded && grabbedObject == null)
        {
            Debug.Log("FOUND OBJECT!!");

            foundObject = hit.collider.gameObject;
            grabbedObject = hit.collider.gameObject.GetComponent<IObjectGrabInterface>();


            if(grabbedObject != null)
            {
                grabbedObject.ObjectGrabbed();
                foundObject.transform.position = throwPoint.position;
                foundObject.transform.parent = throwPoint;
                foundObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                isGrabbing = true;

            }
        }
    }

    public void HeavyThrowObject()
    {
        //Throw the object (Will need to implement a kind of throwing arc so that the player knows how far an object/player is being thrown
        isGrabbing = false;
        foundObject.transform.parent = null;
        foundObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        foundObject.GetComponent<Rigidbody2D>().velocity = (throwPoint.transform.right * (throwForce * (controller.m_FacingRight ? 1 : -1)));
        foundObject = null;
        grabbedObject.ResetObjectGrab();
        grabbedObject = null;
    }

    public void HeavyPushObject()
    {

    }

    #endregion

    #region Light Player Methods
    public void CheckForLightActions()
    {

    }

    public void LightPressButton()
    {

    }

    #endregion

    #region Object Grab Interface Methods
    public void ObjectGrabbed()
    {
        isBeingGrabbed = true;

        
    }

    public void ResetObjectGrab()
    {
        isBeingGrabbed = false;
        isBeingThrown = true;
    }
    #endregion

    Vector2 PointPosition(float t)
    {
        Vector2 position = (Vector2)throwPoint.position + ((throwDirection.normalized * (throwForce * (controller.m_FacingRight ? 1 : -1)) * t) + 0.5f * Physics2D.gravity * (t * t));
        return position;
    }
}
