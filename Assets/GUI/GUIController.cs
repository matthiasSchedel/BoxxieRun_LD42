using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	private float m_lives;
	private float m_coins;

	[SerializeField]
	private AudioClip m_boxBump;
	[SerializeField]
	private AudioClip m_coinBling;
	[SerializeField]
	private AudioClip m_healthBling;
	[SerializeField]
	private AudioClip m_speedBling;
	private AudioSource m_audioSource;
	private Text m_coinText;
	private Text m_healthText;
	private Text m_speedText;
	private LevelManager m_lm;

	private PlayerMovement m_pm;
	// Use this for initialization
	void Start () {
		m_coinText = GameObject.FindObjectOfType<CoinText>().GetComponent<Text>();
		m_healthText = GameObject.FindObjectOfType<Lives>().GetComponent<Text>();
		m_speedText = GameObject.FindObjectOfType<SpeedLeft>().GetComponent<Text>();
		m_audioSource = GetComponent<AudioSource>();
		m_pm = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
		m_lm = GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
		m_coinText.text = "Coins: 0";
		m_healthText.text = "Lives: 9";
		m_lives = 9;
		m_coins = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetLives(int lives) 
	{
		m_lives = lives;
	}
	void DeactivateSpeed()
	{
		m_speedText.text = "";
		m_pm.m_level_speed -= 1;
	}

	public void IncreaseSpeed()
	{
		//m_lives +=1;
		m_speedText.text = "15s speed boost active";
		m_audioSource.clip = m_healthBling;
		m_audioSource.Play();
		m_pm.m_level_speed += 1;
		Invoke("DeactivateSpeed",15);
	}	
	public void DecreaseHealth()
	{
		m_lives -=.5f;

		m_healthText.text = "Lives: " + (int) m_lives;
		m_audioSource.clip = m_boxBump;
		m_audioSource.Play();
		if (m_lives <= 0)
		{
			PlayerPrefs.SetInt("Highscore",(int)m_coins);
			if (PlayerPrefs.HasKey("OverallHighscore") && PlayerPrefs.GetInt("OverallHighscore") >= m_coins)
			{
				//PlayerPrefs.SetInt("OverallHighscore",m_coins);
				PlayerPrefs.SetString("NewHighScore","");
			} else {
				PlayerPrefs.SetInt("OverallHighscore",(int)m_coins);
				PlayerPrefs.SetString("NewHighScore","You set a new Highscore");
			}
			m_lm.LoadLevel("GameOver");
			
		}
	}

	public void IncreaseHealth()
	{
		m_lives +=.5f;
		m_healthText.text = "Lives: " + (int) m_lives;
		m_audioSource.clip = m_healthBling;
		m_audioSource.Play();
	}

	public void IncreaseCoins()
	{
		m_coins +=.5f;
		m_coinText.text = "Coins: " + (int) m_coins;
		m_audioSource.clip = m_coinBling;
		m_audioSource.Play();
	}
}
