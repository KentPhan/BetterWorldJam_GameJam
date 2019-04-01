using UnityEngine;
using UnityEditor;

namespace Light2D
{
	[CustomEditor(typeof(Light2D.Light2DMask))]
	[CanEditMultipleObjects]
	public class Light2DMaskEditor: Light2DBaseEditor
	{
		public override void OnInspectorGUI()
		{
			DrawSpriteGUI();
			base.OnInspectorGUI();

			serializedObject.ApplyModifiedProperties();
		}

		void DrawSpriteGUI()
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Sprite", EditorStyles.boldLabel);
			DrawSpriteObject();

			EditorGUILayout.EndVertical();
		}

		protected override Sprite GetSprite()
		{
			SpriteMask spriteMask = ((MonoBehaviour)serializedObject.targetObject).GetComponent<SpriteMask>();
			return spriteMask.sprite;
		}

		protected override void SetSprite(Sprite sprite)
		{
			SpriteMask spriteMask = ((MonoBehaviour)serializedObject.targetObject).GetComponent<SpriteMask>();
			spriteMask.sprite = sprite;
		}
	}
}