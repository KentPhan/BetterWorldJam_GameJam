using UnityEngine;

namespace Light2D
{
	public class Light2DOverlap
	{
		private ScalableArray<Collider2D> m_Results = null;
		public ScalableArray<Collider2D> results {
			get { return m_Results; }
		}

		public Light2DOverlap(int capacity)
		{
			m_Results = new ScalableArray<Collider2D>(capacity);
		}

		public void CircleOverlap(Transform lightTransform, float radius, int layerMask)
		{
			Vector3 center = lightTransform.position;
			while(true)
			{
				int count = Physics2D.OverlapCircleNonAlloc(center, radius, results.array, layerMask);
				if(count < m_Results.capacity)
				{
					m_Results.SetLength(count);
					break;
				}

				m_Results.Rescale(m_Results.capacity * 2);
			}
		}
	}
}