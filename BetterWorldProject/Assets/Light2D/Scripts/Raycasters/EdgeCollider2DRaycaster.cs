using UnityEngine;

namespace Light2D
{
	public class EdgeCollider2DRaycaster: Collider2DRaycaster
	{
		public EdgeCollider2DRaycaster(Light2DRaycast raycaster) : base(raycaster)
		{
		}

		public override void Raycast(Transform lightTransform, Light2DRadius lightRadius, Collider2D collider)
		{
			EdgeCollider2D edgeCollider = collider as EdgeCollider2D;
			if(edgeCollider == null)
				return;

			Vector2[] points = m_PointsPool.Get(edgeCollider);
			RaycastPoints(lightTransform, collider, lightRadius, points, false);
		}
	}
}