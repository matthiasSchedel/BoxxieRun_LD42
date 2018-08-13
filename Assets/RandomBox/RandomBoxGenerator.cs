using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class RandomBoxPrefab : IComparable<RandomBoxPrefab> {

	[SerializeField] public string m_name;
	[SerializeField] public GameObject m_randomBoxPrefab;
	[SerializeField] public bool m_hasFeature;

	public int CompareTo(RandomBoxPrefab rbp)
	{
		return (m_name.Equals(rbp.m_name)) ? 0 : 1;
	}

}

[System.Serializable]
public class ShelfPrefab : IComparable<ShelfPrefab> {
	[SerializeField] public string m_name;
	[SerializeField] public GameObject m_shelfPrefab;
	[SerializeField] public bool m_hasFeature;

	public int CompareTo(ShelfPrefab rbp)
	{
		return (m_name.Equals(rbp.m_name)) ? 0 : 1;
	}
}

public class RandomBoxGenerator : MonoBehaviour {
	[SerializeField] List<RandomBoxPrefab> m_randomBoxPrefabs;
	[SerializeField] List<ShelfPrefab> m_shelfPrefabs;
	[SerializeField] List<Color> m_boxColors;
	[SerializeField] List<float> m_box_z_Positions;
	[SerializeField] public float m_current_x_position;
	[SerializeField] public float[] m_y_positions = {0f,1f,2f};
	[SerializeField] public GameObject m_coinPrefab;
	[SerializeField] public GameObject m_heartPrefab;
	[SerializeField] public GameObject m_speedUpPrefab;

	private const float m_shelfLength = 9f;
	private const int m_maxNumberOfShelfs = 9;
	private const float m_minBoxDistance = 1.4f;
	private const float m_startBoxDistance = 3f;
	private const float m_minShelfDistance = 1f;

	private const float m_firstShelfPositionX = -3f;
	private float m_lastShelfPositionX;
	private int m_shelfCount;
	private float m_currentBoxDistance;

	private Transform m_playerTransform;
	private Transform m_randomBoxesParent;
	private Transform m_shelfesParent;
	private Transform m_coinParent;

	private LinkedList<GameObject> m_shelfs;
	private LinkedList<LinkedList<GameObject>> m_randomBoxesOfShelf;

	// Use this for initialization
	void Start () {
		m_playerTransform = GameObject.FindObjectOfType<PlayerMovement>().transform;
		m_randomBoxesParent = GameObject.FindObjectOfType<RandomBoxesParent>().transform;
		m_shelfesParent = GameObject.FindObjectOfType<ShelfesParent>().transform;
		m_coinParent = GameObject.FindObjectOfType<CoinParent>().transform;
		m_shelfs = new LinkedList<GameObject>();
		m_randomBoxesOfShelf = new LinkedList<LinkedList<GameObject>>();
		m_shelfCount = 0;
		m_currentBoxDistance = m_startBoxDistance; 

		while(m_shelfs.Count < m_maxNumberOfShelfs)
		{
			GenerateNewShelf();	

		}
	}

	// Update is called once per frame
	void Update () {
		if (isNewShelfNeeded(m_playerTransform.position.x))
		{
			GameObject last = m_shelfs.Last.Value;
			m_shelfs.RemoveLast();
			GameObject.Destroy(last);
			GenerateNewShelf();
		}
	}
	bool isNewShelfNeeded(float pos_x) 
	{
		if (Vector3.Distance(m_shelfs.Last.Value.transform.position, m_playerTransform.position) > 20f) return true;
		return false;
	}

	void SpawnCoin(Vector3 pos)
	{
		int spawnItem = UnityEngine.Random.Range(0,40);
		GameObject coin;
		if (spawnItem == 33)
		{
			coin = GameObject.Instantiate(m_heartPrefab, pos, Quaternion.identity);
		} else if((spawnItem % 19) == 0)
		{
			coin = GameObject.Instantiate(m_speedUpPrefab, pos, Quaternion.identity);	
		} else 
		{
			coin = GameObject.Instantiate(m_coinPrefab, pos, Quaternion.identity);
		}
		 
		coin.transform.parent = m_coinParent;
	}

	void GenerateNewShelf() 
	{
		Vector3 newPosDiff = Vector3.left * (m_firstShelfPositionX + m_shelfLength * m_shelfCount);
		GameObject newShelf = GameObject.Instantiate(m_shelfPrefabs[0].m_shelfPrefab, m_shelfPrefabs[0].m_shelfPrefab.transform.position + newPosDiff, Quaternion.identity);
		m_shelfs.AddFirst(newShelf);
		newShelf.transform.parent = m_shelfesParent;


		int boxRows = (int) (m_shelfLength/m_currentBoxDistance);
		//boxRows -=1;
		LinkedList<GameObject> randomBoxes = new LinkedList<GameObject>();
		Debug.Log("Boxrows" + boxRows);
		//GenerateRandom Boxes
		for(int i = 0; i < boxRows; i++)
		{
			int boxType = UnityEngine.Random.Range(0,3);
			int boxType2 = UnityEngine.Random.Range(0,3);
			int boxPosition = UnityEngine.Random.Range(-1,2);
			int boxPosition2 = UnityEngine.Random.Range(-1,2); 
			if (boxPosition2 != boxPosition) 
			{
				Vector3 pos2 = m_randomBoxPrefabs[0].m_randomBoxPrefab.transform.position;
				pos2.z = (float)(boxPosition2) * 0.8f;
				float shelfx2 = newShelf.transform.position.x;
				pos2.x = shelfx2 + 3 - (i * m_currentBoxDistance); 
				//pos.x *= -1;
				pos2.y = 4.617f;
				GameObject nrb2 = GenerateNewRandomBox(pos2, boxType2);
				randomBoxes.AddFirst(nrb2);
			}
			Vector3 pos = m_randomBoxPrefabs[0].m_randomBoxPrefab.transform.position;
			pos.z = (float)(boxPosition) * 0.8f;
			float shelfx = newShelf.transform.position.x;
			pos.x = shelfx + 3 - (i * m_currentBoxDistance); 
			//pos.x *= -1;
			pos.y = 4.617f;
			GameObject nrb = GenerateNewRandomBox(pos, boxType);
			pos.z = (float)(boxPosition) * -0.8f;
			SpawnCoin(pos + Vector3.left);
			SpawnCoin(pos - Vector3.left);

			randomBoxes.AddFirst(nrb);
			//first spot -0.8 ,second 0.0 ,third +0.8

		}
		m_randomBoxesOfShelf.AddFirst(randomBoxes);


		m_shelfCount++;
	}

	public GameObject GenerateNewRandomBox(Vector3 pos, int boxType)
	{
		//Color color = UnityEngine.Random.ColorHSV();
		GameObject newRandomBox = GameObject.Instantiate(m_randomBoxPrefabs[boxType].m_randomBoxPrefab, pos, Quaternion.identity);
		newRandomBox.transform.parent = m_randomBoxesParent;
		return newRandomBox; 
	}

}
