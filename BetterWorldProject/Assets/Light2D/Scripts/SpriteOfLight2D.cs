using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light2D
{
	// Gets the generated gemetry of Light2DSource or Light2DMask and applies it equally to the SpriteRenderer.
	// You can use this script to create a fuzzy light.

	[ExecuteInEditMode]
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteOfLight2D: MonoBehaviour
	{
		public Light2DBase source = null;

		private SpriteRenderer m_SpriteRenderer = null;
		public SpriteRenderer spriteRenderer {
			get {
				if(m_SpriteRenderer == null)
					m_SpriteRenderer = GetComponent<SpriteRenderer>();

				return m_SpriteRenderer;
			}
		}

		void LateUpdate()
		{
			if(source != null)
			{
				source.UpdateGeometry();

				Sprite sourceSprite = source.GetRendererSprite();
				if(spriteRenderer.sprite != sourceSprite)
				{
					spriteRenderer.sprite = sourceSprite;
				}
			}
		}
	}
}
