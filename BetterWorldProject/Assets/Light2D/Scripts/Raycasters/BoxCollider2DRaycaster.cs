using UnityEngine;

namespace Light2D
{
	public class BoxCollider2DRaycaster: Collider2DRaycaster
	{
		public BoxCollider2DRaycaster(Light2DRaycast raycaster) : base(raycaster)
		{
		}

		public override void Raycast(Transform lightTransform, Light2DRadius lightRadius, Collider2D collider)
		{
			BoxCollider2D boxCollider = collider as BoxCollider2D;
			if(boxCollider == null)
				return;

			Vector2 offset = boxCollider.offset;
			Vector2 boxSize = boxCollider.size;
			Vector2 extents = boxSize / 2f;

			Vector2 topLeft = collider.transform.TransformPoint(new Vector2(-extents.x, extents.y) + offset);
			Vector2 topRight = collider.transform.TransformPoint(new Vector2(extents.x, extents.y) + offset);
			Vector2 bottomRight = collider.transform.TransformPoint(new Vector2(extents.x, -extents.y) + offset);
			Vector2 bottomLeft = collider.transform.TransformPoint(new Vector2(-extents.x, -extents.y) + offset);

			Vector2 lightPosition = lightTransform.position;
			float worldRadius = lightRadius.worldRadius;

			if(Vector2.Distance(lightPosition, topLeft) <= worldRadius)
				m_Raycaster.RaycastColliderSurfacePoint(topLeft);

			if(Vector2.Distance(lightPosition, topRight) <= worldRadius)
				m_Raycaster.RaycastColliderSurfacePoint(topRight);

			if(Vector2.Distance(lightPosition, bottomRight) <= worldRadius)
				m_Raycaster.RaycastColliderSurfacePoint(bottomRight);


			if(Vector2.Distance(lightPosition, bottomLeft) <= worldRadius)
				m_Raycaster.RaycastColliderSurfacePoint(bottomLeft);

			RaycastIntersectionPointsOfLine(lightTransform, worldRadius, topLeft, topRight);
			RaycastIntersectionPointsOfLine(lightTransform, worldRadius, topRight, bottomRight);
			RaycastIntersectionPointsOfLine(lightTransform, worldRadius, bottomRight, bottomLeft);
			RaycastIntersectionPointsOfLine(lightTransform, worldRadius, bottomLeft, topLeft);
		}
	}
}