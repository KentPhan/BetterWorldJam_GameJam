using UnityEngine;

namespace Light2D
{
	public class CircleCollider2DRaycaster: Collider2DRaycaster
	{
		public CircleCollider2DRaycaster(Light2DRaycast raycaster) : base(raycaster)
		{
		}

		public override void Raycast(Transform lightTransform, Light2DRadius lightRadius, Collider2D collider)
		{
			CircleCollider2D circleCollider = collider as CircleCollider2D;
			if(circleCollider == null)
				return;

			Vector2 lightPosition = lightTransform.position;
			Vector2 position = circleCollider.bounds.center;
			float radius = circleCollider.bounds.size.x / 2f;

			Vector2 d = position - lightPosition;
			float dd = Vector2.Distance(lightPosition, position);

			float a = Mathf.Asin(radius / dd);
			float b = Mathf.Atan2(d.y, d.x);

			float t = b - a;
			Vector2 positionA = position + new Vector2(radius * Mathf.Sin(t), radius * -Mathf.Cos(t));

			t = b + a;
			Vector2 positionB = position + new Vector2(radius * -Mathf.Sin(t), radius * Mathf.Cos(t));

			if(Vector2.Distance(lightPosition, positionA) < lightRadius.worldRadius && Vector2.Distance(lightPosition, positionB) < lightRadius.worldRadius)
			{
				m_Raycaster.RaycastColliderSurfacePoint(positionA);
				m_Raycaster.RaycastColliderSurfacePoint(positionB);
			}
		}
	}
}