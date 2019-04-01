using UnityEngine;

namespace Light2D
{
	public class Light2DRadius
	{
		private float m_LocalRadius;
		public float localRadius {
			get { return m_LocalRadius; }
		}

		private float m_WorldRadius;
		public float worldRadius {
			get { return m_WorldRadius; }
		}

		private float m_SpriteRadius;
		public float spriteRadius {
			get { return m_SpriteRadius; }
		}

		public void Init(Transform transform, Sprite sprite, float radius)
		{
			// Minium length of sprite,
			m_SpriteRadius = Mathf.Min(sprite.bounds.size.x, sprite.bounds.size.y) / 2f;

			m_LocalRadius = Mathf.Min(sprite.rect.size.x, sprite.rect.size.y);
			m_WorldRadius = GetWorldRadius(transform, radius);
		}

		public static float GetWorldRadius(Transform transform, float radius)
		{
			float worldRadius = radius;
			if(transform.parent != null)
			{
				Vector3 lossyScale = transform.parent.lossyScale;
				worldRadius *= Tools.GetScaleBase(lossyScale);
			}

			return Mathf.Abs(worldRadius);
		}
	}
}