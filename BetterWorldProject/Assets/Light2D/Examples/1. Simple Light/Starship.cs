using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starship : MonoBehaviour
{
	[SerializeField]
	private float m_Speed = 1;

	void Update()
	{
		transform.Translate(Vector3.up * m_Speed * Time.deltaTime);
	}	
}
