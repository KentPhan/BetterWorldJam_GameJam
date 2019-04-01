using UnityEngine;
using System.Collections.Generic;

namespace Light2D
{
	public class ColliderPointsPool
	{
		private readonly Dictionary<int, Vector2[]> m_Pool = new Dictionary<int, Vector2[]>();

		public ColliderPointsPool()
		{
		}

		public Vector2[] Get(EdgeCollider2D collider)
		{
			int instanceId = collider.GetInstanceID();

			if(!m_Pool.ContainsKey(instanceId))
			{
				Vector2[] points = collider.points;
				m_Pool[instanceId] = points;
			}
			return m_Pool[instanceId];
		}

		public Vector2[] Get(PolygonCollider2D collider)
		{
			int instanceId = collider.GetInstanceID();

			if(!m_Pool.ContainsKey(instanceId))
			{
				Vector2[] points = collider.points;
				m_Pool[instanceId] = points;
			}
			return m_Pool[instanceId];
		}
	}
}