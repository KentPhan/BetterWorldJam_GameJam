using System.Collections.Generic;
using UnityEngine;

namespace Light2D
{
	public struct RaycastInfo
	{
		public Collider2D collider;
		public float localAngle;

		public Vector2 localPosition;
	}

	public class Light2DRaycast
	{
		private const float kAngleDelta = 0.01f;

		private static readonly Quaternion kLeftRotation = Quaternion.Euler(0, 0, -kAngleDelta);
		private static readonly Quaternion kRightRotation = Quaternion.Euler(0, 0, kAngleDelta);

		private readonly RaycastInfoComparer m_RaycastSortComparer = new RaycastInfoComparer();

		private ScalableArray<RaycastInfo> m_Results = null;
		public ScalableArray<RaycastInfo> results {
			get { return m_Results; }
		}

		private Transform m_LightTransform = null;
		private Vector2 m_LightPosition;

		private Light2DAngle m_LightAngle = null;
		private Light2DRadius m_LightRadius = null;
		private int m_LayerMask = 0;

		private Collider2DRaycaster boxColliderRaycaster = null;
		private Collider2DRaycaster circleColliderRaycaster = null;
		private Collider2DRaycaster polygonColliderRaycaster = null;
		private Collider2DRaycaster edgeColliderRaycaster = null;

		public Light2DRaycast(int capacity)
		{
			m_Results = new ScalableArray<RaycastInfo>(capacity);

			boxColliderRaycaster = new BoxCollider2DRaycaster(this);
			circleColliderRaycaster = new CircleCollider2DRaycaster(this);
			polygonColliderRaycaster = new PolygonCollider2DRaycaster(this);
			edgeColliderRaycaster = new EdgeCollider2DRaycaster(this);
		}

		public void Init(Transform lightTransform, Light2DRadius lightRadius, Light2DAngle lightAngle, int layerMask)
		{
			m_LightTransform = lightTransform;
			m_LightPosition = lightTransform.position;

			m_LightRadius = lightRadius;
			m_LightAngle = lightAngle;

			m_LayerMask = layerMask;

			results.Clear();
		}

		public void RaycastByResolution(int resolution)
		{
			float angleGap = m_LightAngle.angle / resolution;
			float rayCount = m_LightAngle.isFullAngle ? resolution : resolution + 1;

			for(int i = 0; i < rayCount; i++)
			{
				float localAngle = angleGap * i + 180 - m_LightAngle.halfAngle;
				RaycastAngle(localAngle);
			}
		}

		public void RaycastCollider(Collider2D collider)
		{
			if(collider is BoxCollider2D)
			{
				boxColliderRaycaster.Raycast(m_LightTransform, m_LightRadius, collider);
			}
			else if(collider is CircleCollider2D)
			{
				circleColliderRaycaster.Raycast(m_LightTransform, m_LightRadius, collider);
			}
			else if(collider is CapsuleCollider2D)
			{
				// Todo
			}
			else if(collider is EdgeCollider2D)
			{
				edgeColliderRaycaster.Raycast(m_LightTransform, m_LightRadius, collider);
			}
			else if(collider is PolygonCollider2D)
			{
				polygonColliderRaycaster.Raycast(m_LightTransform, m_LightRadius, collider);
			}
		}

		public void SortResults()
		{
			System.Array.Sort<RaycastInfo>(results.array, 0, results.length, m_RaycastSortComparer);
		}

		public void RaycastAngle(float localAngle)
		{
			Vector2 localDirection = Quaternion.Euler(0, 0, localAngle + m_LightAngle.rotate) * Vector2.right;
			Vector2 worldDirection = m_LightTransform.TransformDirection(localDirection);

			Vector2 center = m_LightTransform.position;
			float worldRadius = m_LightRadius.worldRadius;

			RaycastHit2D hit = Raycast(center, worldDirection, worldRadius);
			Vector2 position = hit.collider != null ? hit.point : center + worldDirection.normalized * worldRadius;
			position = m_LightTransform.InverseTransformPoint(position);

			AddRaycastResult(hit.collider, position, localAngle);
		}

		private RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
		{
			RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, m_LayerMask);
			return hit;
		}

		public void WorldRaycast(Transform lightTransform, Vector2 worldPosition)
		{
			Vector2 center = lightTransform.position;
			Vector2 worldDirection = worldPosition - center;

			Vector2 localDirection = lightTransform.InverseTransformDirection(worldDirection);
			float localAngle = m_LightAngle.ToAngle(localDirection);
			if(!m_LightAngle.IsOnAngle(localAngle))
				return;

			float distance = Vector2.Distance(center, worldPosition);

			RaycastHit2D hit = Raycast(center, worldDirection, distance);
			Vector2 position = hit.collider != null ? hit.point : worldPosition;
			position = lightTransform.InverseTransformPoint(position);

			AddRaycastResult(hit.collider, position, localAngle);
		}

		public void WorldRaycast(Transform lightTransform, Vector2 worldDirection, float worldDistance)
		{
			Vector2 localDirection = lightTransform.InverseTransformDirection(worldDirection);
			float localAngle = m_LightAngle.ToAngle(localDirection);
			if(!m_LightAngle.IsOnAngle(localAngle))
				return;

			Vector2 center = lightTransform.position;

			RaycastHit2D hit = Raycast(center, worldDirection, worldDistance);
			Vector2 position = hit.collider != null ? hit.point : center + worldDirection.normalized * worldDistance;
			position = lightTransform.InverseTransformPoint(position);

			AddRaycastResult(hit.collider, position, localAngle);
		}

		public void RaycastColliderSurfacePoint(Vector2 worldPosition)
		{
			WorldRaycast(m_LightTransform, worldPosition);

			Vector2 worldDirection = worldPosition - m_LightPosition;

			Vector2 leftDirection = kLeftRotation * worldDirection;
			WorldRaycast(m_LightTransform, leftDirection, m_LightRadius.worldRadius);

			Vector2 rightDirection = kRightRotation * worldDirection;
			WorldRaycast(m_LightTransform, rightDirection, m_LightRadius.worldRadius);
		}

		public void AddRaycastResult(Collider2D collider, Vector2 localPosition, float angle)
		{
			RaycastInfo result = new RaycastInfo() {
				collider = collider,
				localAngle = angle,
				localPosition = localPosition
			};

			results.Add(result);
		}
	}

	class RaycastInfoComparer: IComparer<RaycastInfo>
	{
		public int Compare(RaycastInfo a, RaycastInfo b)
		{
			return a.localAngle.CompareTo(b.localAngle);
		}
	}
}