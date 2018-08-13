using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider coll) 
	{
		if (coll.tag == "Player")
		{
			GameObject.FindObjectOfType<GUIController>().IncreaseSpeed();
			GameObject.Destroy(gameObject);
		}
	}
}
