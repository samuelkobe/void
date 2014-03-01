using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {

    public CubeTier topCube;  // the monoBehaviour(script) of topCube, to refernce the topCube gameobject, use topCube.gameObject
    public CubeTier bottomCube;
    public MainCameraController mainCamera;
    // public Quaternion rotationMonitor;
    enum enumRotatingTier { top, middle, bottom };
    enumRotatingTier rotatingTier;
    bool _getInputControl;
    bool _noInput;
    public int numberOfTiers;

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


    // Use this for initialization
    void Start()
    {
        _getInputControl = false;
        _noInput = true;
        numberOfTiers = 2;
    }
	
	
	// Update is called once per frame
	void Update () 
    {
        if (gameManager.inputDisabled || GetInputControl)
        {
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3)) 
            {
                GetInputControl = true;
                if (!cameraArriveAtWideAngle)
                {
                    mainCamera.SlerpToPosition(mainCamera.wideAnglePosition);
                    if (Vector3.Distance(mainCamera.transform.position, mainCamera.wideAnglePosition) < mainCamera.wideAnglePositionAccuracy)
                        cameraArriveAtWideAngle = true;
                }
                else
                {
                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                        
                    }
                }
            }
        }
	}
}
