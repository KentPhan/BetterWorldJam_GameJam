using System.Collections;
using UnityEngine;

namespace Light2D
{
	[ExecuteInEditMode]
	public abstract class Light2DBase: MonoBehaviour
	{
		[SerializeField]
		private Light2DProperties m_Properties = new Light2DProperties();
		public Light2DProperties properties {
			get { return m_Properties; }
			set { m_Properties = value; }
		}

		private Sprite m_AppliedSprite = null;

		// Sprite visible in inspector
		[SerializeField]
		private Sprite m_SourceSprite = null;
		public Sprite sprite {
			get { return m_SourceSprite; }
			set { m_SourceSprite = value; }
		}

		protected Sprite rendererSprite {
			get {
				return GetRendererSprite();
			}
			set {
				SetRendererSprite(m_SourceSprite);
			}
		}

		public bool autoUpdate = true;
		public bool sendEventMessage = false;
		public bool executeInEditMode = false;

		private int m_LastUpdateFrame = -1;

		private readonly Light2DSpriteUpdater m_LightUpdater = new Light2DSpriteUpdater();
		private readonly Light2DEventManager m_EventManager = new Light2DEventManager();

		public float radius {
			get { return m_Properties.radius; }
			set { m_Properties.radius = value; }
		}

		public float angle {
			get { return m_Properties.angle; }
			set { m_Properties.angle = value; }
		}

		public float rotation {
			get { return m_Properties.rotation; }
			set { m_Properties.rotation = value; }
		}

		public int resolution {
			get { return m_Properties.resolution; }
			set { m_Properties.resolution = value; }
		}

		public int layerMask {
			get { return m_Properties.layerMask; }
			set { m_Properties.layerMask = value; }
		}

		private bool m_IsEnabled = false;

		void OnEnable()
		{
			m_IsEnabled = true;
		}

		void OnDisable()
		{
			rendererSprite = m_SourceSprite;
		}

		public void UpdateGeometry()
		{
			if(m_LastUpdateFrame >= Time.frameCount)
				return;

			if(m_Properties != null)
			{
				if(m_LightUpdater != null)
				{
					if(m_IsEnabled)
					{
						if(m_SourceSprite != null)
						{
							SetSprite(m_SourceSprite);
						}
						else
						{
							Sprite rendererSprite = GetRendererSprite();
							SetSprite(rendererSprite);
						}
						
						m_IsEnabled = false;
					}

					// If sprite have changed, apply new sprite
					if(m_SourceSprite != m_AppliedSprite)
					{
						SetSprite(m_SourceSprite);
					}

					// Get the sprite to apply geometry.
					Sprite sprite = GetRendererSprite();

					if(sprite != null)
					{
						// Set a scale corresponding to the radius to the transform.
						UpdateLightScale(sprite);

						// Update sprite geometry.
						m_LightUpdater.UpdateLightSprite(transform, sprite, m_Properties);
					}
				}

#if UNITY_EDITOR
				if(Application.isPlaying && sendEventMessage)
#else
				if(sendEventMessage)
#endif
				{
					// Send lighting messages
					SendEvetMessage();
				}
			}

			m_LastUpdateFrame = Time.frameCount;
		}

		public void SetSprite(Sprite sourceSprite)
		{
			m_SourceSprite = sourceSprite;

			if(m_SourceSprite != null)
			{
				Sprite cloneSprite = Object.Instantiate<Sprite>(m_SourceSprite);
				SetRendererSprite(cloneSprite);
			}
			else
			{
				SetRendererSprite(null);
			}
			/*
			else
			{
				// If sprite is null then create white sprite texture.
				Sprite emptySprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), Vector2.one * 0.5f);
				emptySprite.name = kEmptySpriteName;
				SetRendererSprite(emptySprite);
			}
			*/

			m_AppliedSprite = sourceSprite;
		}

		void LateUpdate()
		{
#if UNITY_EDITOR
			if(!Application.isPlaying && !executeInEditMode)
				return;
#endif

			if(autoUpdate)
			{
				UpdateGeometry();
			}
		}

		void SendEvetMessage()
		{
			var raycastResults = m_LightUpdater.lightRaycast.results;
			for(int i = 0; i < raycastResults.length; i++)
			{
				var result = raycastResults[i];
				if(result.collider != null)
				{
					GameObject go = result.collider.gameObject;
					m_EventManager.Add(go);
				}
			}

			// Send Enter and Exit messages.
			m_EventManager.SendEnterMessage(this);
			m_EventManager.SendExitMessage(this);

			// End of sending of current frame.
			m_EventManager.Complete();
		}

		// Set a scale corresponding to the radius to the transform.
		void UpdateLightScale(Sprite sprite)
		{
			Vector2 spriteBoundSize = sprite.bounds.size;
			float spriteRadius = Mathf.Min(spriteBoundSize.x, spriteBoundSize.y) / 2f;

			Vector3 scale = Vector3.one * properties.radius / spriteRadius;
			if(transform.parent != null)
			{
				Vector3 lossyScale = transform.parent.lossyScale;
				Vector3 baseScaleVector = lossyScale / Tools.GetScaleBase(lossyScale);

				if(baseScaleVector.x != 0 && baseScaleVector.y != 0 && baseScaleVector.z != 0)
				{
					scale.x /= baseScaleVector.x;
					scale.y /= baseScaleVector.y;
					scale.z /= baseScaleVector.z;
				}
				else
				{
					scale = Vector3.zero;
				}
			}

			transform.localScale = scale;
		}

		public abstract Sprite GetRendererSprite();
		public abstract void SetRendererSprite(Sprite sprite);

		private void OnDrawGizmosSelected()
		{
			if(m_LightUpdater != null)
			{
				float worldRadius = m_LightUpdater.lightRadius.worldRadius;
				var rayResults = m_LightUpdater.lightRaycast.results;

				if(rayResults != null)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawWireSphere(transform.position, worldRadius);

					Gizmos.color = Color.blue;
					for(int i = 0; i < rayResults.length; i++)
					{
						RaycastInfo info = rayResults[i];
						Vector3 position = transform.TransformPoint(info.localPosition);
						Gizmos.DrawLine(transform.position, position);
					}
				}
			}
		}
	}
}