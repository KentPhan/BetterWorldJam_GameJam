using System.Collections.Generic;
using UnityEngine;

namespace Light2D
{
	public class Light2DEventManager
	{
		public const string kEnterMessageName = "OnEnterLight2D";
		public const string kExitMessageName = "OnExitLight2D";

		private HashSet<GameObject> m_CurrentSet = new HashSet<GameObject>();
		private HashSet<GameObject> m_PreviousSet = new HashSet<GameObject>();

		public void Add(GameObject go)
		{
			m_CurrentSet.Add(go);
		}

		public void SendEnterMessage(Light2DBase light)
		{
			foreach(GameObject go in m_CurrentSet)
			{
				if(!m_PreviousSet.Contains(go))
				{
					go.SendMessage(kEnterMessageName, light);
				}
			}
		}

		public void SendExitMessage(Light2DBase light)
		{
			foreach(GameObject go in m_PreviousSet)
			{
				if(!m_CurrentSet.Contains(go))
				{
					go.SendMessage(kExitMessageName, light);
				}
			}
		}

		public void Complete()
		{
			var temp = m_PreviousSet;

			m_PreviousSet = m_CurrentSet;
			m_CurrentSet = temp;

			m_CurrentSet.Clear();
		}
	}
}