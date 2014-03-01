using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public bool inputMoniter; //////&&

    public CharacterController characterController;
    Vector3 moveDirection;
    public float moveSpeed;
    public float jumpSpeed;
    public float climbLadderSpeed;
    public float gravity;
    public bool characterCanMoveUp;
    private bool _getInputControl = false; //@private later
    private bool _noInput = true; //reset to true every frame

    public void setCanMoveUp(bool value)  // called by function in other script with "send message"
    {
        characterCanMoveUp = value;
    }

    private bool GetInputControl //@ private later
    {
        get
        {
            return _getInputControl;
        }
        set 
        {
            _getInputControl = value;
            gameManager.inputDisabled = _getInputControl;
            if (_getInputControl) // if there is input of character's controll key
            {
                _noInput = false;
            }
        }
    }

	// Use this for initialization
	void Start () 
    {
        characterController = GetComponent<CharacterController>();
        characterCanMoveUp = false; 
        moveSpeed = 0.15f;
        jumpSpeed = 25f;
        gravity = 3f;
        climbLadderSpeed = 0.1f;

	
	}
	
	// Update is called once per frame
	void Update ()
    {
        inputMoniter = gameManager.inputDisabled; //&&

        _noInput = true;
        if(!gameManager.inputDisabled || GetInputControl)
        {
            if (characterController.isGrounded || characterCanMoveUp) //@@@ ground funtion has some error, so no the character mutex is not efficient
            {
                if (Input.GetKey("d"))
                {
                    moveDirection = Vector3.right * moveSpeed;
                    GetInputControl = true;
                }
                else if (Input.GetKey("a"))
                {
                    moveDirection = Vector3.left * moveSpeed;
                    GetInputControl = true;
                }
                else
                {
                    moveDirection = Vector3.zero; // move left/righ stop as soon as keyUp
                }
                if (Input.GetKey("w"))
                {
                    if (characterCanMoveUp) // when there is a ladder
                        moveDirection = Vector3.up * climbLadderSpeed;
                    else
                        moveDirection.y = jumpSpeed * Time.deltaTime;
                    GetInputControl = true;
                }
                else if (Input.GetKey("s") && characterCanMoveUp)
                {
                    moveDirection = Vector3.down * climbLadderSpeed;
                    GetInputControl = true;
                }

            }

            if (_noInput)
                 GetInputControl = false;

        }

        if(!characterController.isGrounded && !characterCanMoveUp) // is character is not at ground and is not at a ladder
        {
            if ((characterController.collisionFlags & CollisionFlags.Above) != 0)
                moveDirection = Vector3.zero;
            if (!characterCanMoveUp)
                moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection); // finally, update the position, and camera position
        //@@@ update cameraControl later. manager.cameraControl.updateCamera(character.transform.position);
	}
}
