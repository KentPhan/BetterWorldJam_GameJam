using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Light2D
{
	public abstract class Light2DBaseEditor: Editor
	{
		SerializedProperty m_Properties = null;

		void OnEnable()
		{
			m_Properties = serializedObject.FindProperty("m_Properties");
		}

		public override void OnInspectorGUI()
		{
			DrawPropertiesGUI();
			DrawEventGUI();
			DrawUpdateGUI();
			DrawEditorGUI();

			serializedObject.ApplyModifiedProperties();
		}

		protected void DrawSpriteObject()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Sprite");

			SerializedProperty sourceSprite = serializedObject.FindProperty("m_SourceSprite");

			Sprite rendererSprite = GetSprite();
			if(rendererSprite != null && EditorUtility.IsPersistent(rendererSprite))
			{
				sourceSprite.objectReferenceValue = rendererSprite;
				if(((Light2DBase)serializedObject.targetObject).enabled)
					SetSprite(Object.Instantiate<Sprite>(rendererSprite));
			}

			Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(sourceSprite.objectReferenceValue, typeof(Sprite), false);
			if(sourceSprite.objectReferenceValue != newSprite)
			{
				sourceSprite.objectReferenceValue = newSprite;
				if(((Light2DBase)serializedObject.targetObject).enabled)
					((Light2DBase)serializedObject.targetObject).SetSprite(newSprite);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.HelpBox("To change the sprite in code, use Light2DBase.SetSprite() or Light2DBase.sourceSprite property instead of the SpriteRenderer", MessageType.Info);
		}

		void DrawPropertiesGUI()
		{
			SerializedProperty radius = m_Properties.FindPropertyRelative("m_Radius");
			SerializedProperty angle = m_Properties.FindPropertyRelative("m_Angle");
			SerializedProperty rotation = m_Properties.FindPropertyRelative("m_Rotation");
			SerializedProperty resolution = m_Properties.FindPropertyRelative("m_Resolution");
			SerializedProperty layerMask = m_Properties.FindPropertyRelative("m_LayerMask");

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);

			radius.floatValue = EditorGUILayout.FloatField("Radius", radius.floatValue);
			angle.floatValue = EditorGUILayout.Slider("Angle", angle.floatValue, 0f, 360f);
			rotation.floatValue = EditorGUILayout.Slider("Rotation", rotation.floatValue, 0f, 360f);
			resolution.intValue = EditorGUILayout.IntSlider("Resolution", resolution.intValue, 3, 300);
			EditorGUILayout.HelpBox("Too high resolution affect the performance.", MessageType.Info);

			int tempMaskValue = EditorGUILayout.MaskField("LayerMask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask.intValue), InternalEditorUtility.layers);
			layerMask.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMaskValue);

			EditorGUILayout.EndVertical();
		}

		void DrawEventGUI()
		{
			SerializedProperty useEvent = serializedObject.FindProperty("sendEventMessage");

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Event", EditorStyles.boldLabel);

			useEvent.boolValue = EditorGUILayout.Toggle("Send Event Message", useEvent.boolValue);
			EditorGUILayout.HelpBox("Send OnEnterLight2D(Light2DBase), OnExitLight2D(Light2DBase) message.", MessageType.Info);

			EditorGUILayout.EndVertical();
		}

		void DrawEditorGUI()
		{
			SerializedProperty executeInEditMode = serializedObject.FindProperty("executeInEditMode");

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

			executeInEditMode.boolValue = EditorGUILayout.Toggle("ExecuteInEditor", executeInEditMode.boolValue);

			EditorGUILayout.EndVertical();
		}

		void DrawUpdateGUI()
		{
			SerializedProperty autoUpdate = serializedObject.FindProperty("autoUpdate");

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.LabelField("Update", EditorStyles.boldLabel);

			autoUpdate.boolValue = EditorGUILayout.Toggle("Automatic Update", autoUpdate.boolValue);
			EditorGUILayout.HelpBox("You can update manually using \"UpdateGeometry()\"", MessageType.Info);

			EditorGUILayout.EndVertical();
		}

		protected abstract Sprite GetSprite();
		protected abstract void SetSprite(Sprite sprite);
	}
}