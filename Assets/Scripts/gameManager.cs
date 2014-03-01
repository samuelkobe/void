using UnityEngine;
using System.Collections;


public class gameManager : MonoBehaviour
{
    public MainCameraControl cameraControl;
    public PlanetControl planetControl;
    bool canControlPlanetOrCharacter = true;
    bool canControlCamera = false;
    public static int enumPlanet = 1;
    public static int enumCamera = 2;
    public static bool inputDisabled;

    //public void switchControl(int control)
    //{
    //    if (control == enumPlanet)
    //    {
    //        cameraControl._controlIncamera = false;
    //    }
    //    else if (control == enumCamera)
    //    {
    //        planetControl._controlInPlanet = false;
    //    }
    //}

    // Use this for initialization
    void Start()
    {
        inputDisabled = false;
    }

    // Update is called once per frame
    void Update()
    {



    }

}
