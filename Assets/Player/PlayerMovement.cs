using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	private const float m_control_range  = 0.95f;
	[SerializeField]
	public float m_level_speed = 1f;
	private float fallMultiplier = 3f;

	[SerializeField]
	private AudioClip[] m_jumpAudioClips;

	private AudioSource m_audioSource;
	private float lowJumpMultiplier = 0.3f;

	private float m_movementSpeed_mobile = 5f;

	[SerializeField]
	private int m_maxLives = 3;
	private int m_currLives;

	private GUIController m_guiController;

	[SerializeField]
	private float m_hopPower = 1f;

	private Rigidbody m_rb;
	private Material m_mt;
	private Animator m_am;

	private bool m_isDead;
	private float time = 0.0f;
	public float interpolationPeriod = 0.02f;

	// Use this for initialization
	void Start () {
		m_rb = GetComponentInChildren<Rigidbody>();
		m_mt = GetComponentInChildren<MeshRenderer>().material;
		Debug.Log("color:" + m_mt.color);
		m_am = GetComponent<Animator>();
		m_audioSource = GetComponent<AudioSource>();

		m_guiController = GameObject.FindObjectOfType<GUIController>();
		m_isDead = false;
		m_currLives = m_maxLives;

	}

	void Jump()
	{
		

		//control jump height by length of time jump button held
		if (m_rb.velocity.y >= 0) {
			m_rb.velocity += (Vector3.up + Vector3.left) * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void Hop() {
		Debug.Log("Hop");
		float hopVarUp = Random.Range(140,160);
		float hopVarLeft = Random.Range(80,100);
		m_rb.AddForce(Vector3.up * hopVarUp);
		m_rb.AddForce(Vector3.left * hopVarLeft);
	}

	void FixedUpdate()
	{
		if (transform.position.y <= 0.3f)
		{
			Die();
		}
		if (m_isDead) return;
		transform.Translate(Vector3.left * Time.deltaTime * m_level_speed);
		//m_rb.velocity += Vector3.left * Time.deltaTime;
		//faster falling
		if (m_rb.velocity.y < 0) {
			m_rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		time += Time.deltaTime;

		if (time >= interpolationPeriod) {
			time = 0.0f;
			//if(m_rb.velocity.y <= 0) //Hop();
		}
	}
	// Update is called once per frame
	void Update () {

	
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			//Hop();
		}

		if (Input.GetAxis("Vertical") > 0.2f) 
		{
			//Debug.Log("Vertical" + Input.GetAxis("Vertical"));
		}
		//TODO: An den ecken nur nach links und rechts bewegen können

		float z;
		#if UNITY_IOS
		z = Time.deltaTime * Input.acceleration.x * m_movementSpeed_mobile;
		#elif UNITY_ANDROID
		z = Time.deltaTime * Input.acceleration.x * m_movementSpeed_mobile;
		#else 
		z = Time.deltaTime * 3 * Input.GetAxis("Horizontal"); 
		#endif
		//Debug.Log("Horizontal" +  Input.GetAxis("Horizontal"));
		if (transform.position.z + z < m_control_range && transform.position.z + z > - m_control_range) 
		{
			float pos_z = Mathf.Clamp(transform.position.z + z, -m_control_range, m_control_range);
			transform.position = new Vector3(transform.position.x, transform.position.y, pos_z);
		} else {
			float pos_z = Mathf.Clamp(transform.position.z, -m_control_range, m_control_range);
			transform.position = new Vector3(transform.position.x, transform.position.y, pos_z);
		}
	}

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.collider.tag == "blocking") 
		{
			m_guiController.SetLives(--m_currLives);
			if (m_currLives == 0) { Die(); }
		} else 
		{
			//BlowAway
		}
	}

	void Die()
	{
		m_mt.color = Color.red;
		m_isDead = true;
		GetComponent<Rigidbody>().isKinematic = true;
		m_am.SetBool("p_dead",true);
	}

}
