using UnityEngine;

namespace Light2D
{
	public class Light2DSpriteUpdater
	{
		private readonly Light2DAngle m_LightAngle = new Light2DAngle();
		public Light2DAngle lightAngle {
			get { return m_LightAngle; }
		}

		private readonly Light2DRadius m_LightRadius = new Light2DRadius();
		public Light2DRadius lightRadius {
			get { return m_LightRadius; }
		}

		private Light2DOverlap m_LightOverlap = null;
		public Light2DOverlap lightOverlap {
			get { return m_LightOverlap; }
		}

		private Light2DRaycast m_LightRaycast = null;
		public Light2DRaycast lightRaycast {
			get { return m_LightRaycast; }
		}

		private Vector2[] m_Vertices = null;
		private ushort[] m_Triangles = null;

		public Light2DSpriteUpdater()
		{
			m_LightOverlap = new Light2DOverlap(1024);
			m_LightRaycast = new Light2DRaycast(1024);
		}

		public void UpdateLightSprite(Transform lightTransform, Sprite sprite, Light2DProperties properties)
		{
			if(properties.resolution < 3)
				return;
			
			// Temporarily stop transform sync to improve performance in 2017.2 above version.
#if UNITY_2017_2_OR_NEWER
			bool savedAutoSyncTransforms = Physics2D.autoSyncTransforms;
			Physics2D.autoSyncTransforms = false;

			Physics2D.SyncTransforms();
#endif

			// Set the initial values to the angle, radius data object.
			m_LightAngle.Init(properties.angle, properties.rotation);
			m_LightRadius.Init(lightTransform, sprite, properties.radius);

			Vector2 lightPosition = lightTransform.position;

			m_LightRaycast.Init(lightTransform, m_LightRadius, m_LightAngle, properties.layerMask);
			// Raycast to the default circle.
			m_LightRaycast.RaycastByResolution(properties.resolution);

			// Find colliders in the circle of radius.
			m_LightOverlap.CircleOverlap(lightTransform, m_LightRadius.worldRadius, properties.layerMask);
			if(m_LightOverlap.results.length > 0)
			{
				for(int i = 0; i < m_LightOverlap.results.length; i++)
				{
					Collider2D collider = m_LightOverlap.results[i];
					// Raycasts to the detected edge of coliiders.
					m_LightRaycast.RaycastCollider(collider);
				}
				// Sorting raycast results according to center and angle.
				m_LightRaycast.SortResults();
			}

#if UNITY_2017_2_OR_NEWER
			Physics2D.autoSyncTransforms = savedAutoSyncTransforms;
#endif

			// Update sprite geometry.
			DrawShadow(sprite, lightTransform, properties);
		}

		void DrawShadow(Sprite sprite, Transform lightTransform, Light2DProperties properties)
		{
			Vector2 lightScale = lightTransform.localScale;
			Vector2 spriteSize = sprite.rect.size;

			Vector2 spriteCenter = spriteSize / 2f;

			var results = m_LightRaycast.results;
			if(results.length > 0)
			{
				// Calculate vertex count.
				int vertexCount = results.length + 1;
				if(m_Vertices == null || m_Vertices.Length != vertexCount)
					m_Vertices = new Vector2[vertexCount];

				m_Vertices[0] = spriteCenter;
				for(int i = 0; i < results.length; i++)
				{
					RaycastInfo rayInfo = m_LightRaycast.results[i];
					Vector2 position = rayInfo.localPosition;

					// Calculate vertex point.
					Vector2 pixelPoint = spriteCenter + position * sprite.pixelsPerUnit;
					pixelPoint.x = Mathf.Clamp(pixelPoint.x, 0, spriteSize.x);
					pixelPoint.y = Mathf.Clamp(pixelPoint.y, 0, spriteSize.y);

					m_Vertices[i + 1] = pixelPoint;
				}

				int triangleCount = m_LightAngle.isFullAngle ? vertexCount - 1 : vertexCount - 2;
				if(m_Triangles == null || m_Triangles.Length != triangleCount * 3)
					m_Triangles = new ushort[triangleCount * 3];

				for(int i = 0; i < triangleCount; i++)
				{
					m_Triangles[i * 3] = 0;
					m_Triangles[i * 3 + 1] = (ushort)(i + 1);
					m_Triangles[i * 3 + 2] = (ushort)(i + 2 < m_Vertices.Length ? i + 2 : 1);
				}

				sprite.OverrideGeometry(m_Vertices, m_Triangles);
			}
		}
	}
}