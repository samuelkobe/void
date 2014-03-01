using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

    public GameObject associatedObject;
    bool canBePushed = false;
	
	// Update is called once per frame
	void Update () {
        if (canBePushed && Input.GetKey("j"))
        {
            Destroy(associatedObject);
			gameObject.renderer.material.color = Color.black;
        }
	
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
            canBePushed = true;

    }

	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Player")
						canBePushed = false;
	}

}
