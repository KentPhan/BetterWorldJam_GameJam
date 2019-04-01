using UnityEngine;
using UnityEngine.UI;

namespace Light2D.Example
{
	public class ChangeSpriteButton: MonoBehaviour
	{
		private Button m_Button = null;
		public Button button {
			get {
				if(m_Button == null)
					m_Button = GetComponent<Button>();
				return m_Button;
			}
		}

		[SerializeField]
		private Light2DSource m_TargetLight = null;

		[SerializeField]
		private Sprite m_Sprite = null;
		[SerializeField]
		private Color m_Color;

		void OnEnable()
		{
			button.onClick.AddListener(OnClicked);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(OnClicked);
		}

		void OnClicked()
		{
			m_TargetLight.SetSprite(m_Sprite);
			m_TargetLight.color = m_Color;
		}
	}
}