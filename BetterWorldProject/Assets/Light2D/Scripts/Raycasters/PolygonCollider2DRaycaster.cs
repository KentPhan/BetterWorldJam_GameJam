using UnityEngine;

namespace Light2D
{
	public class PolygonCollider2DRaycaster: Collider2DRaycaster
	{
		public PolygonCollider2DRaycaster(Light2DRaycast raycaster) : base(raycaster)
		{
		}

		public override void Raycast(Transform lightTransform, Light2DRadius lightRadius, Collider2D collider)
		{
			PolygonCollider2D polygonCollider = collider as PolygonCollider2D;
			if(polygonCollider == null)
				return;

			Vector2 lightPosition = lightTransform.position;

			Vector2[] points = m_PointsPool.Get(polygonCollider);
			RaycastPoints(lightTransform, collider, lightRadius, points, true);
		}
	}
}