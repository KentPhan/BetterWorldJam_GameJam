using UnityEngine;

namespace Light2D
{
	[System.Serializable]
	public class Light2DProperties
	{
		[SerializeField]
		private float m_Radius = 10f;
		public float radius {
			get { return m_Radius; }
			set { m_Radius = Light2DAngle.Clamp360(value); }
		}

		[SerializeField, Range(0f, 360f)]
		private float m_Angle = 360f;
		public float angle {
			get { return m_Angle; }
			set { m_Angle = Light2DAngle.Clamp360(value); }
		}

		[SerializeField, Range(0f, 360f)]
		private float m_Rotation = 0f;
		public float rotation {
			get { return m_Rotation; }
			set { m_Rotation = Light2DAngle.Clamp360(value); }
		}

		[SerializeField, Range(3, 300)]
		private int m_Resolution = 10;
		public int resolution {
			get { return m_Resolution; }
			set { m_Resolution = Mathf.Max(value, 3); }
		}

		[SerializeField]
		private int m_LayerMask = ~0;
		public int layerMask {
			get { return m_LayerMask; }
			set { m_LayerMask = value; }
		}
	}
}