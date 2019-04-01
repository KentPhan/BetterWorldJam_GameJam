using UnityEngine;

namespace Light2D
{
	public class Light2DAngle
	{
		private float m_Angle = 360f;
		public float angle {
			get { return m_Angle; }
			set { m_Angle = Mathf.Clamp(value, 0f, 360f); }
		}

		public float halfAngle {
			get { return angle / 2f; }
		}

		public bool isFullAngle {
			get { return m_Angle >= 360f; }
		}

		private float m_Rotate = 0f;
		public float rotate {
			get { return m_Rotate; }
		}

		public void Init(float angle, float rotate)
		{
			this.angle = angle;
			this.m_Rotate = rotate;
		}

		public bool IsOnAngle(float localAngle)
		{
			if(this.angle >= 360)
				return true;

			float lowerAngle = 180 - halfAngle;
			float upperAngle = angle + lowerAngle;

			return localAngle >= lowerAngle && localAngle <= upperAngle;
		}

		public float ToAngle(Vector2 direction)
		{
			return Clamp360(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - m_Rotate);
		}

		public static float Clamp360(float angle)
		{
			return (angle % 360 + 360) % 360;
		}
	}
}