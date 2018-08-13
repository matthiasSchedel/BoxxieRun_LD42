using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {
	private Text m_score;
	private Text m_highScore;
	// Use this for initialization
	void Start () {
		m_score = GetComponentsInChildren<Text>()[2];
		m_highScore = GetComponentsInChildren<Text>()[0];
		m_score.text = "You collected " + PlayerPrefs.GetInt("Highscore") + " coins";
		m_highScore.text = PlayerPrefs.GetString("NewHighScore");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
