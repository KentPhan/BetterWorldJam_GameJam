using UnityEngine;

namespace Light2D
{
	public abstract class Collider2DRaycaster
	{
		protected Light2DRaycast m_Raycaster = null;
		protected static readonly ColliderPointsPool m_PointsPool = new ColliderPointsPool();

		public Collider2DRaycaster(Light2DRaycast raycaster)
		{
			m_Raycaster = raycaster;
		}

		public abstract void Raycast(Transform lightTransform, Light2DRadius lightRadius, Collider2D collider);

		protected void RaycastIntersectionPointsOfLine(Transform lightTransform, float worldRadius, Vector2 pointA, Vector2 pointB)
		{
			Vector2 lightPosition = lightTransform.position;

			float distA = Vector2.Distance(lightPosition, pointA);
			float distB = Vector2.Distance(lightPosition, pointB);

			if(distA > worldRadius || distB > worldRadius)
			{
				Vector2 d = pointB - pointA;
				float a = d.x * d.x + d.y * d.y;
				float b = 2 * (d.x * (pointA.x - lightPosition.x) + d.y * (pointA.y - lightPosition.y));
				float c = lightPosition.x * lightPosition.x + lightPosition.y * lightPosition.y;
				c += pointA.x * pointA.x + pointA.y * pointA.y;
				c -= 2 * (lightPosition.x * pointA.x + lightPosition.y * pointA.y);
				c -= worldRadius * worldRadius;

				float bb4ac = b * b - 4 * a * c;

				if(Mathf.Abs(a) < float.Epsilon || bb4ac < 0)
				{
					return;
				}
				else if(bb4ac == 0)
				{
					float t = (-b + Mathf.Sqrt(bb4ac)) / (2 * a);
					Vector2 p1 = new Vector2(pointA.x + t * d.x, pointA.y + t * d.y);

					m_Raycaster.WorldRaycast(lightTransform, p1);
				}
				else
				{
					float t = (-b + Mathf.Sqrt(bb4ac)) / (2 * a);
					Vector2 p1 = new Vector2(pointA.x + t * d.x, pointA.y + t * d.y);
					Vector2 p = new Vector2(
						(p1.x - pointB.x) / (pointA.x - pointB.x),
						(p1.y - pointB.y) / (pointA.y - pointB.y)
					);

					if((p.x >= 0 && p.x <= 1) || (p.y >= 0 && p.y <= 1))
						m_Raycaster.WorldRaycast(lightTransform, p1);

					t = (-b - Mathf.Sqrt(bb4ac)) / (2 * a);
					Vector2 p2 = new Vector2(pointA.x + t * d.x, pointA.y + t * d.y);
					p = new Vector2(
						(p2.x - pointB.x) / (pointA.x - pointB.x),
						(p2.y - pointB.y) / (pointA.y - pointB.y)
					);

					if((p.x >= 0 && p.x <= 1) || (p.y >= 0 && p.y <= 1))
						m_Raycaster.WorldRaycast(lightTransform, p2);
				}
			}
		}

		protected void RaycastPoints(Transform lightTransform, Collider2D collider, Light2DRadius lightRadius, Vector2[] points, bool isClosedPolygon)
		{
			Vector2 lightPosition = lightTransform.position;

			int length = isClosedPolygon ? points.Length + 1 : points.Length;
			for(int i = 0; i < length; i++)
			{
				Vector2 pointA = points[i % points.Length];
				pointA = collider.transform.TransformPoint(pointA);

				Vector2 pointB = points[(i + 1) % points.Length];
				pointB = collider.transform.TransformPoint(pointB);

				float distA = Vector2.Distance(lightPosition, pointA);
				float distB = Vector2.Distance(lightPosition, pointB);

				if(distA <= lightRadius.worldRadius)
					m_Raycaster.RaycastColliderSurfacePoint(pointA);

				if(distA > lightRadius.worldRadius || distB > lightRadius.worldRadius)
					RaycastIntersectionPointsOfLine(lightTransform, lightRadius.worldRadius, pointA, pointB);
			}
		}
	}
}