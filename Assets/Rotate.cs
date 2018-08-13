using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	[SerializeField]
	private float m_spinSpeed = 300;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, m_spinSpeed * Time.deltaTime, 0);	
	}
}
