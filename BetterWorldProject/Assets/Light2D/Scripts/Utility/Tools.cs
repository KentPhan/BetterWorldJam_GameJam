using UnityEngine;

namespace Light2D
{
	public class Tools
	{
		public static float GetScaleBase(Vector3 lossyScale)
		{
			if(lossyScale.x < 0 || lossyScale.y < 0)
			{
				return Mathf.Min(lossyScale.x, lossyScale.y);
			}
			else
			{
				return Mathf.Max(lossyScale.x, lossyScale.y);
			}
		}
	}
}