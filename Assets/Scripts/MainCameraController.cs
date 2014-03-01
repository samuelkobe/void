using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {
    
    //related object
    public GameObject character;

    // booleans
    private bool _getInputControl;
    private bool _noInput = true; // reset to true every frame
    private bool canRotateView = false; ///@@ private later
    private bool isBeingReset = false; /// indicate camera is being reset (player release the space button)
    private bool rotateDirection;
    private bool arriveAtPlayModePosition;
    private bool arriveAtRotateStartPosition;

    //parameters
    public float rotateSpeed; //////@@@@@@@@@@@@
    public Vector3 playModePosition;   // now set it public to get the value of them in console. later change to private
    public Quaternion playModeRotation;
    public Vector3 rotateStartPosition;
    public Quaternion rotateStartRotation;
    public Vector3 wideAnglePosition;
    public Vector3 playModeCameraDistance;


    //accuracies
    public float moveToRotateStartAccuracy; // from playModePosition to rotateStartPosition
    public float rotateBackToRotateStartPositionAccuracy;
    public float playModePositionAccuracy;

    private bool GetInputControl
    {
        get
        {
            return _getInputControl;
        }
        set
        {
            _getInputControl = value;
            gameManager.inputDisabled = _getInputControl;
            if (_getInputControl)
            {
                _noInput = false;
            }
        }
    }

     void rotateAnticlockwise()
    {
        rotateDirection = false;
        transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1f, 0), rotateSpeed * Time.deltaTime);
    }

    void rotateClockwise()
    {
        rotateDirection = true;
        transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1f, 0), rotateSpeed * Time.deltaTime);
    }

    void rotateBackToRotateStartPosition()
    {
        rotateCamera(rotateDirection);
    }

    void rotateCamera(bool currentRotateDirection)
    {
        if (currentRotateDirection == true)
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1f, 0), rotateSpeed * Time.deltaTime);
        else
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1f, 0), rotateSpeed * Time.deltaTime);
    }


    public void SlerpToPositionAndRotation(Vector3 destPosition, Quaternion destRotation)
    {
        transform.position = Vector3.Slerp(transform.position, destPosition, 3f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, destRotation, 0.5f * Time.deltaTime);
    }
    public void SlerpToPosition(Vector3 destPosition)
    {
        transform.position = Vector3.Slerp(transform.position, destPosition, 3f * Time.deltaTime);
    }



	// Use this for initialization
	void Start () 
    {
        rotateSpeed = 30f;
  
        wideAnglePosition = new Vector3(0f, 0f, -32f);
        rotateStartPosition = new Vector3(0, 5f, -32f);
        rotateStartRotation = Quaternion.Euler(10f,0,0);
        playModeCameraDistance = new Vector3(0, 0, -5f);
        transform.rotation  = playModeRotation = Quaternion.Euler(5f, 0, 0);
        transform.position = playModePosition = character.transform.position + playModeCameraDistance; /////@ change later, camera only update position when character move
        moveToRotateStartAccuracy = 0.5f;  // the accuracy depend on the rotate speed
       // playModePositionAccuracy = 0.5f;
        arriveAtPlayModePosition = false;
        arriveAtRotateStartPosition = false;
        rotateBackToRotateStartPositionAccuracy = rotateSpeed / 20; // related to  rotateSpeed
        playModePositionAccuracy = 0.2f;
    
    }
	
	
	// Update is called once per frame

    void Update()
    {
        
        _noInput = true;
        if (!gameManager.inputDisabled || GetInputControl) // can input to control camera, or is controlling camera
        {
            bool needRotateBack = true;
            if (Input.GetKey(KeyCode.Space)) // hold space, waiting for other key or space up, if is being reset, disable input
            {
                GetInputControl = true;
                if (canRotateView) 
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rotateClockwise();
                        //@@@ reset later, maybe don't need, because now we have to hold a key to ratate, so on that key up, reset
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rotateAnticlockwise();
                        //@@@ reset later
                    }
                }
                else // can't rotate because camera haven't arrive at rotateStartPosition
                {
                    SlerpToPositionAndRotation(rotateStartPosition,rotateStartRotation);  // move camera to roatate start position, and rotate camera to rotate start rotation
                    if (Vector3.Distance(transform.position, rotateStartPosition) < moveToRotateStartAccuracy)
                        canRotateView = true;
                }

            }
            else if (Input.GetKeyUp(KeyCode.Space)) // release space key, start to reset camera
            {
                isBeingReset = true;
                arriveAtPlayModePosition = false;
                arriveAtRotateStartPosition = false;
                if (!canRotateView) // if camera has never arrived at rotate start position when space key released 
                {
                    needRotateBack = false;
                }
                canRotateView = false;
                
            }

            if (isBeingReset)
            {
                if (!arriveAtRotateStartPosition ) 
                {
                    
                    if (Vector3.Distance(transform.position, rotateStartPosition) < rotateBackToRotateStartPositionAccuracy || !needRotateBack)
                    {
                        arriveAtRotateStartPosition = true;
                    }
                    else
                        rotateBackToRotateStartPosition();
                }
                else
                {
                    if (!arriveAtPlayModePosition) // move camera from rotate start to playMode position, return true if arrives at playMode position
                    {
                        SlerpToPositionAndRotation(playModePosition,playModeRotation); // move back to playModePosition, rotate back to playModeRotation
                        if (Vector3.Distance(transform.position, playModePosition) < playModePositionAccuracy)
                            arriveAtPlayModePosition = true;
                    }
                    else
                    {
                        isBeingReset = false;
                       
                    }
                }
            }
            

            if (_noInput && !isBeingReset)
            {
                GetInputControl = false;
            }
        }

        if (!GetInputControl) // camera don't control input, is not being reset either
        {
            transform.position = character.transform.position + playModeCameraDistance;
        }
    }
}
