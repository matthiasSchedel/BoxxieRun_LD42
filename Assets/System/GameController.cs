using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	public GameState m_gameState;
	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else
		{
			DestroyImmediate(this);
		}
	}

	public static GameController m_instance;
	public static GameController Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new GameController();
			}
			return m_instance;
		}
}
}
public enum GameState
{
	Start,
	Playing,
	Dead
}