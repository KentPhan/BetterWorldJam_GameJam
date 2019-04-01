using UnityEngine;

namespace Light2D.Example
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Tree: MonoBehaviour
	{
		[SerializeField]
		private Sprite[] m_SpriteSequence;

		private bool m_IsInLight = false;
		private float m_ExposedTime = 0f;

		private SpriteRenderer m_SpriteRenderer = null;
		public SpriteRenderer spriteRenderer {
			get {
				if(m_SpriteRenderer == null)
					m_SpriteRenderer = GetComponent<SpriteRenderer>();
				return m_SpriteRenderer;
			}
		}

		void Update()
		{
			m_ExposedTime += m_IsInLight ? Time.deltaTime * 5 : -Time.deltaTime * 5;
			m_ExposedTime = Mathf.Clamp(m_ExposedTime, 0, m_SpriteSequence.Length - 1);

			int spriteIndex = (int)m_ExposedTime;
			spriteRenderer.sprite = m_SpriteSequence[spriteIndex];
		}

		void OnEnterLight2D(Light2DBase light)
		{
			m_IsInLight = true;
			m_ExposedTime = 0f;
		}

		void OnExitLight2D(Light2DBase light)
		{
			m_IsInLight = false;
		}
	}
}