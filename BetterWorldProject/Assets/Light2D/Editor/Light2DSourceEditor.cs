using UnityEngine;
using UnityEditor;

namespace Light2D
{
	[CustomEditor(typeof(Light2D.Light2DSource))]
	[CanEditMultipleObjects]
	public class Light2DSourceEditor: Light2DBaseEditor
	{
		public override void OnInspectorGUI()
		{
			DrawSpriteGUI();
			base.OnInspectorGUI();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnSceneGUI()
		{
			Handles.BeginGUI();

			Handles.EndGUI();
		}

		void DrawSpriteGUI()
		{
			SpriteRenderer spriteRenderer = ((MonoBehaviour)serializedObject.targetObject).GetComponent<SpriteRenderer>();

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Sprite", EditorStyles.boldLabel);

			DrawSpriteObject();
			spriteRenderer.color = EditorGUILayout.ColorField("Color", spriteRenderer.color);

			EditorGUILayout.EndVertical();
		}

		protected override Sprite GetSprite()
		{
			SpriteRenderer spriteRenderer = ((MonoBehaviour)serializedObject.targetObject).GetComponent<SpriteRenderer>();
			return spriteRenderer.sprite;
		}

		protected override void SetSprite(Sprite sprite)
		{
			SpriteRenderer spriteRenderer = ((MonoBehaviour)serializedObject.targetObject).GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
		}
	}
}