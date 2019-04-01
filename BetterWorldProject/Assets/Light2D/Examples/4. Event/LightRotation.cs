using UnityEngine;

namespace Light2D.Example
{
	public class LightRotation: MonoBehaviour
	{
		private Light2DBase m_Light = null;
		public Light2DBase light2d {
			get {
				if(m_Light == null)
					m_Light = GetComponent<Light2DBase>();
				return m_Light;
			}
		}

		public void OnChangeRotation(float rotation)
		{
			light2d.rotation = rotation;
		}
	}
}