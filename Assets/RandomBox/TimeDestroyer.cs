using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroyer : MonoBehaviour
{

	void Start()
	{
		Invoke("DestroyObject", LifeTime);
	}

	void DestroyObject()
	{
		if (GameController.m_instance.m_gameState != GameState.Dead)
			Destroy(gameObject);
	}

	public float LifeTime = 10f;
}
