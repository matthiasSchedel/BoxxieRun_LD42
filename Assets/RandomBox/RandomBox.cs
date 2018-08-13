using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour {
	[SerializeField]
	private bool m_isBlocking;

	[SerializeField]
	private TimeDestroyer m_timeDestroyer;
	// Use this for initialization
	void Start () {
		//m_timeDestroyer = new TimeDestroyer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision c)
	{
		if(c.collider.tag == "Player")
		{
			GameObject.FindObjectOfType<GUIController>().DecreaseHealth();
			GameObject.Destroy(gameObject);
		}	
	}
}
