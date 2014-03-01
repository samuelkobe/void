using UnityEngine;
using System.Collections;

public class PickableItems : MonoBehaviour {

    public GameObject hanlder;
    bool canBePicked = false;

	// Update is called once per frame
	void Update () {
        if (canBePicked && Input.GetKey("j"))
        {
            Destroy(gameObject);
            hanlder.SendMessage("ItemPicked"); 
           
        }
	
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
            canBePicked = true;

    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
            canBePicked = false;

    }
}
