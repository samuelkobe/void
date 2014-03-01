using UnityEngine;
using System.Collections;

public class rotationMonitor : MonoBehaviour {

    public Quaternion monitor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        monitor = transform.rotation;
	}
}
