using UnityEngine;
using System.Collections;

public class PlanetControl : MonoBehaviour
{
 
    public GUITexture gotUpgradeTexture;
	public GUITexture storeTxture;
	public GUITexture instructionTexture;
	public GUIText upgradeCounterText;
    int upgradeCounter = 0;
    public bool characterCanMoveUp = true;
    bool gotTheUpgrade = false; // for the upgrade

    public gameManager manager;
    public bool _controlInPlanet = true; ///  the control is within planet(include the character and cube and rotate)
    public bool _canControlPlanet = true;  //////!!!!
    public bool _canControlCharacter = true;
    public bool _canRotate = false;
    public bool _isRotating = false;
    public bool _wantRotate = false;
    public CubeTier topCube;  // the monoBehaviour(script) of topCube, to refernce the topCube gameobject, use topCube.gameObject
    public CubeTier bottomCube;
    // public Quaternion rotationMonitor;
    enum enumRotatingTier { top, bottom };
    enumRotatingTier rotatingTier;
    enum enumCameraMoveDirection { noMove, moveToWideAngle, moveBack };
    enumCameraMoveDirection cameraMoveDirection = enumCameraMoveDirection.noMove;
    Vector3 moveDirection;
	bool gameIsStart = false;


    void ItemPicked()
    {
        print("Item is picked");
        gotTheUpgrade = true;
		upgradeCounter++;
	

    }

    bool ControlInPlanet
    {
        get
        {
            return _controlInPlanet;
        }

        set
        {
            _controlInPlanet = value;
            ////////@ switch control with camera
        }

    }

    bool CanControlPlanet  ///setter and getter for the bool canControl planet, note that when the canControlPlanet is changed, canControlCharacter and CanControlCamera will change accordingly
    {
        get
        {
            return _canControlPlanet;
        }

        set
        {
            _canControlPlanet = value;
            ///@ call gameManager switch or directly call the setter in mainCamera Control
            _canControlCharacter = !_canControlPlanet; // @ here may better to: if(true) other are false  (don't care about the false condition)
            if (_canControlCharacter)
            {
                _canRotate = false;
                _wantRotate = false;
                //_isRotating = true;
            }
        }
    }


    bool CanControlCharacter
    {
        get
        {
            return _canControlCharacter;
        }

        set
        {
            _canControlCharacter = value;
            if (_canControlCharacter)
            {
                _canRotate = false;
                _wantRotate = false;
                _canControlPlanet = false; /// when the character control button is released, should change the 
                //_isRotating = true;
            }
        }
    }

    bool CanRotate
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
                //_isRotating = false;
                _wantRotate = true; //////////$
                _canControlPlanet = false;
                _canControlCharacter = false;
                cameraMoveDirection = enumCameraMoveDirection.moveBack;
            }
        }
    }

    //bool isRotating
    //{
    //    get 
    //    {
    //        return _isRotating;
    //    }

    //    set 
    //    {
    //        _isRotating = value;
    //        if (_isRotating)
    //        {
    //            _wantRotate = false;
    //            _canRotate = false;
    //            _canControlPlanet = true;
    //            _canControlCharacter = true;
    //        }

    //    }
    //}

    bool WantRotate // in the stage of move camera to wide angle, rotate tier, move camera back to playmode position, WantRotate will all be true;
    {
        get
        {
            return _wantRotate;
        }
        set
        {
            _wantRotate = value;
            if (_wantRotate)
            {
                _canRotate = false;
                _canControlCharacter = false;
                _canControlPlanet = false;
                // _isRotating = false;
            }
            else if (!_wantRotate)
            {
                _canRotate = false;
                _canControlCharacter = true;  ////$ here, when the control is given back to player, he can chooose to control planet or character
                _canControlPlanet = true;  ////$ so in canControlPlanet and canControlCharacter, just set the other one be false
                cameraMoveDirection = enumCameraMoveDirection.noMove;
            }

        }
    }

    void releasePlanetOrCharacterButton()
    {
        _canControlCharacter = true;
        _canControlPlanet = true;
    }

    // Use this for initialization
    void Start()
    {
        _controlInPlanet = true;
        _canControlPlanet = true;
        _canControlCharacter = true;
    }

    void SetPlanetCanRotate(bool canControl)
    {
        CanControlPlanet = canControl;
        CanControlCharacter = canControl;
        print("receive message");
    }



    // Update is called once per frame
    void Update()
    {

		upgradeCounterText.text = upgradeCounter.ToString ();

		if (!gameIsStart && Input.GetKeyUp(KeyCode.Return)) 
		{
			if(storeTxture.enabled)
			{
				storeTxture.enabled = false;
				instructionTexture.enabled = true;
			}
			else if(instructionTexture.enabled)
			{
				instructionTexture.enabled = false;
				gameIsStart = true;
			}
			return;

		}
        //if (!ControlInPlanet) ////////@later
        //    return;
        if (gotTheUpgrade)
        {
            gotUpgradeTexture.enabled = true;
        }

      
        if (CanControlPlanet) /////@ here may put the getkey in the top if condition (according to the releasebutton condition)
        {
            if (Input.GetKey("r") && topCube.canRotate)
            {
              
                    WantRotate = true;
                    rotatingTier = enumRotatingTier.top;
               

            }
            else if (Input.GetKey("e") && bottomCube.canRotate)
            {
                WantRotate = true;
                rotatingTier = enumRotatingTier.bottom;
            }
        }
        else if (WantRotate)
        {
            if (!CanRotate)
            {
                if (cameraMoveDirection == enumCameraMoveDirection.noMove)
                {
                    /// add a bool to indicate the direction of camera movement
                    if (manager.cameraControl.moveToWideAnglePosition())
                    {
                        CanRotate = true;
                    }
                }

                else if (cameraMoveDirection == enumCameraMoveDirection.moveBack) ///$ infact the condition of moveToWideAngle is not used
                {
                    if (manager.cameraControl.moveBackToPlayModePositionFromWideAngle())  /////////////////@ next time start from here
                    {
                        WantRotate = false;

                    }
                }
            }
            else
            {
                if (rotatingTier == enumRotatingTier.top)
                {
                    if (topCube.rotateTier())
                    {
                        CanRotate = false;
                        ////@ add the character can move later
                    }
                }
                else if (rotatingTier == enumRotatingTier.bottom)
                {
                    if (bottomCube.rotateTier())
                    {
                        CanRotate = false;
                    }
                }
            }
        }

    }




}
