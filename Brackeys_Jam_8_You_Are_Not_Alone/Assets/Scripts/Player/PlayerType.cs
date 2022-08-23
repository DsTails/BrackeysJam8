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
    public bool isBeingThrown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //For each input, check the player's type
        //Call the method corresponding to the type
    }

    //Insert methods for each player type, depending on what they do/can do
    #region Heavy Player Methods
    public void HeavyGrabObject()
    {
        //Check if the object is a player/has the playerMovement/type script
        //If it does, then set a bool to prevent them from moving
    }

    public void HeavyThrowObject()
    {
        //Throw the object (Will need to implement a kind of throwing arc so that the player knows how far an object/player is being thrown
    }

    public void HeavyPushObject()
    {

    }

    #endregion

    #region Light Player Methods

    public void LightPressButton()
    {

    }

    #endregion

    #region Object Grab Interface Methods
    public void ObjectGrabbed()
    {

    }

    public void ResetObjectGrab()
    {

    }
    #endregion
}
