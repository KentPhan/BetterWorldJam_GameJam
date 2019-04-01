using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
	private Vector3 torque;

	void Awake()
	{
		torque = new Vector3(0, 0, Random.Range(-100, 100));
	}

	void Update()
	{
		transform.Rotate(torque * Time.deltaTime);

		Vector3 position = GetComponent<SpriteRenderer>().bounds.max;
		float screenY = Camera.main.WorldToScreenPoint(position).y;

		if(screenY < 0)
			GameObject.Destroy(gameObject);
	}
}
