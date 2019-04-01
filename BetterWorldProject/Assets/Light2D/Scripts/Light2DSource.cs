using UnityEngine;

namespace Light2D
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Light2DSource: Light2DBase
	{
		// Cache of SpriteRenderer
		private SpriteRenderer m_SpriteRenderer = null;
		public SpriteRenderer spriteRenderer {
			get {
				if(m_SpriteRenderer == null)
					m_SpriteRenderer = GetComponent<SpriteRenderer>();
				return m_SpriteRenderer;
			}
		}

		// Synced value with sprite of SpriteRenderer.
		public Sprite sprite {
			get { return spriteRenderer.sprite; }
			set { spriteRenderer.sprite = value; }
		}

		// Synced value with color of SpriteRenderer.
		public Color color {
			get { return spriteRenderer.color; }
			set { spriteRenderer.color = value; }
		}

		public override Sprite GetRendererSprite()
		{
			Sprite sprite = spriteRenderer.sprite;
			return sprite;
		}

		public override void SetRendererSprite(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}
	}
}