using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] m_AstroidPrefabs = null;
	[SerializeField]
	private float m_PerSecond = 0.5f;

	private float m_Time = 0;

	void Update()
	{
		m_Time += Time.deltaTime;

		int count = (int)(m_Time * m_PerSecond);
		if(count > 0)
		{
			m_Time -= count / m_PerSecond;

			for(int i=0; i < count; i++)
			{
				GameObject prefab = m_AstroidPrefabs[Random.Range(0, m_AstroidPrefabs.Length)];
				GameObject go = GameObject.Instantiate(prefab);

				Vector3 screenPosition = new Vector3(
					Random.Range(0, Screen.width),
					Random.Range(Screen.height + 10, Screen.height + 30),
					0);

				Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);

				go.transform.position = position;
				float scale = Random.Range(0.5f, 1.3f);
				go.transform.localScale = new Vector3(scale, scale, 0f);
			}
		}
	}
}
