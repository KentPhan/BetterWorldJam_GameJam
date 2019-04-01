using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	[SerializeField]
	private Transform m_Target = null;

	void Update()
	{
		if(m_Target == null)
			return;

		Vector3 position = transform.position;
		Vector3 targetPosition = m_Target.position;

		transform.position = targetPosition;
	}
}
