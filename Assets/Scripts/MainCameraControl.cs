using UnityEngine;

public class MainCameraControl : MonoBehaviour
{

    public gameManager manager; // use findWithTag to access the planetControl script
    public Vector3 playModePosition;   // now set it public to get the value of them in console. later change to private
    public Quaternion playModeRotation;
    public Vector3 rotateStartPosition;
    public Quaternion rotateStartRotation;
    public Vector3 wideAnglePosition; // for viewing surface when rotate the planet
    float rotateSpeed = 30f;
    float resetRotateSpeed = 60f;
    float nextReset = 0;
    const float resetInterval = 0.2f; // the interval between when palyer release the rotate camera key and the when start the camera reset
    public bool _canRotate = false; // camera can start rotate only when it has moved to the resetRotationPosition
    public bool _canMoveToPlayModePosition = false;
    public bool _canMoveToWideAnglePosition = false;
    public bool _controlIncamera = true;
    public bool rotateDirection = true; // true for rotate clockwise, false for rotate anticlockwise 
    float playModePositionAccuracy;  // the accuracy to determine, whether two vector3 are same position
    float moveToRotateStartAccuracy;  // the accuracy depend on the rotate speed, initialize in Start()
    Vector3 cameraDistance = new Vector3(0f, 0f, -5f); // distance between camera and character;


    public void updateCamera(Vector3 characterPosition)
    {
        playModePosition = transform.position = characterPosition + cameraDistance;
    }

    public void disablePlanetAndCharacter()
    {
        manager.planetControl._canControlCharacter = false;
        manager.planetControl._canControlPlanet = false;
    }

    public void enablePlanetAndCharacter()
    {
        manager.planetControl._canControlCharacter = true;
        manager.planetControl._canControlPlanet = true ;
    }

    public bool ControlInCamera
    {
        get
        {
            return _controlIncamera;
        }

        set
        {
            _controlIncamera = value;

        }
    }

    public bool CanMoveToPlayModePosition
    {
        get
        {
            return _canMoveToPlayModePosition;
        }

        set
        {
            _canMoveToPlayModePosition = value;
            if (_canMoveToPlayModePosition)  // if this is true, the other action must be false, but this is false,that doesn't means the other two must be false
            {
                _canMoveToWideAnglePosition = false;
                _canRotate = false;
            }
        }

    }

    public bool CanMoveToWideAnglePosition
    {
        get
        {
            return _canMoveToWideAnglePosition;
        }

        set
        {
            _canMoveToWideAnglePosition = value;
            if (_canMoveToWideAnglePosition)
            {
                _canMoveToPlayModePosition = false;
                _canRotate = false;
            }
        }
    }

    public bool CanRotate
    {
        get
        {
            return _canRotate;
        }

        set
        {
            _canRotate = value;
            if (_canRotate)
            {
                _canMoveToWideAnglePosition = false;
                _canMoveToPlayModePosition = false;
            }
        }
    }



    // Use this for initialization
    void Start()
    {
        playModePosition = transform.position;
        playModeRotation = transform.rotation;
        wideAnglePosition = new Vector3(0f, 0f, -18f);
        rotateStartPosition = new Vector3(0, 7.5f, -32f);
        rotateStartRotation = new Quaternion(-0.2143169f, -0.008785776f, 0.001927818f, -0.9767228f);
        moveToRotateStartAccuracy = resetRotateSpeed / 20f;  // the accuracy depend on the rotate speed
        playModePositionAccuracy = 0.5f;
        _controlIncamera = true;
    }

    public void ResetCamera()
    {
        if (Vector3.Distance(transform.position, rotateStartPosition) < moveToRotateStartAccuracy)
        {
            CanRotate = false;
            CanMoveToPlayModePosition = true;
        }
        if (CanRotate)
            rotateBackToRotateStartPosition();
        if (CanMoveToPlayModePosition)   //////
        {
            moveBackToPlayModePosition();
        }
        if(Vector3.Distance(transform.position,playModePosition) <= playModePositionAccuracy)
        {
            enablePlanetAndCharacter();
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

    void moveToRotateStartPosition()
    {
        transform.position = Vector3.Slerp(transform.position, rotateStartPosition, 3f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateStartRotation, 3f * Time.deltaTime);
        if (Vector3.Distance(transform.position, rotateStartPosition) < moveToRotateStartAccuracy)
            CanRotate = true;
    }

    void moveBackToPlayModePosition()
    {
        transform.position = Vector3.Slerp(transform.position, playModePosition, 3f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, playModeRotation, 3f * Time.deltaTime);

    }

    public bool moveBackToPlayModePositionFromWideAngle()
    {
        transform.position = Vector3.Slerp(transform.position, playModePosition, 3f * Time.deltaTime);
        if (Vector3.Distance(transform.position, playModePosition) > playModePositionAccuracy)
        {
        
            return false;
           
        }
    
        return true;
    }

    void rotateBackToRotateStartPosition()
    {
        if (rotateDirection == true)
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 1f, 0), resetRotateSpeed * Time.deltaTime);
        else
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, -1f, 0), resetRotateSpeed * Time.deltaTime);

    }

    public bool moveToWideAnglePosition() // called in planet, not here, return true when the movement finshed
    {

        CanMoveToWideAnglePosition = true;
        if (Vector3.Distance(transform.position, wideAnglePosition) > 0.5f)
        {
            print("wide position is called");
            transform.position = Vector3.Slerp(transform.position, wideAnglePosition, 3f * Time.deltaTime);
            return false;
        }
        else
        {
            print(transform.position.ToString());
            CanMoveToWideAnglePosition = false;
            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ControlInCamera) // the the control is now not in camera, do noting in update
            return;

        if (Input.GetKey("p"))
        {
           disablePlanetAndCharacter();
            //planet.SendMessage("SetPlanetCanRotate", false);
            if (CanRotate)
            {
                rotateAnticlockwise();
                nextReset = Time.time + resetInterval;
            }
            else
                moveToRotateStartPosition();
        }
        else if (Input.GetKey("o"))
        { /////////
            //planet.SendMessage("SetPlanetCanRotate", false);
            disablePlanetAndCharacter();
            if (CanRotate)
            {
                rotateClockwise();
                nextReset = Time.time + resetInterval;
            }
            else
                moveToRotateStartPosition();

        }
        else if (Time.time > nextReset && Vector3.Distance(transform.position, playModePosition) > playModePositionAccuracy)
        {
            ResetCamera();
        }
        else
        {
            //planet.SendMessage("SetPlanetCanRotate", true);   /////@ change in the future
            CanMoveToPlayModePosition = false;

        }


    }

}
