using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour 
{
    //test for git

    void OnTriggerStay(Collider collider)
    {
        print("enter ladder " + collider.tag);
        if (collider.tag == "Player")
        {
            print("character enter ladder");
            collider.gameObject.SendMessage("setCanMoveUp", true);
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            print("character enter ladder");
            collider.gameObject.SendMessage("setCanMoveUp", false);
        }

    }

}
