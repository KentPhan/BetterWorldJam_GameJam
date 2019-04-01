using UnityEngine;

namespace Light2D
{
	[RequireComponent(typeof(SpriteMask))]
	public class Light2DMask: Light2DBase
	{
		// Cache of SpriteMask
		private SpriteMask m_SpriteMask = null;
		public SpriteMask spriteMask {
			get {
				if(m_SpriteMask == null)
					m_SpriteMask = GetComponent<SpriteMask>();
				return m_SpriteMask;
			}
		}

		/*
		// Synced value with color of SpriteMask.
		public Sprite sprite {
			get { return spriteMask.sprite; }
			set { spriteMask.sprite = value; }
		}
		*/

		public override Sprite GetRendererSprite()
		{
			Sprite sprite = spriteMask.sprite;
			return sprite;
		}

		public override void SetRendererSprite(Sprite sprite)
		{
			spriteMask.sprite = sprite;
		}
	}
}