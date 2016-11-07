using UnityEngine;
using UnityEditor;
using System.Collections;

namespace VVOSS.D2d
{

	[CustomEditor(typeof(VOSRectLocator2d))]
	[CanEditMultipleObjects]
	public class VOSRectLocator2dInspector : Editor
	{

		protected VOSRectLocator2d _locator;
		protected SerializedProperty _orientation;
		protected SerializedProperty _size;
		protected SerializedProperty _delta;
		protected SerializedProperty _rows;
		protected SerializedProperty _columns;
		protected SerializedProperty _gizmosIndices;
		protected SerializedProperty _gizmosColor;

		protected void OnEnable()
		{
			_locator = (VOSRectLocator2d)target;
			_orientation = serializedObject.FindProperty("_orientation");
			_size = serializedObject.FindProperty("_size");
			_delta = serializedObject.FindProperty("_delta");
			_rows = serializedObject.FindProperty("_rows");
			_columns = serializedObject.FindProperty("_columns");
			_gizmosIndices = serializedObject.FindProperty("_gizmosIndices");
			_gizmosColor = serializedObject.FindProperty("_gizmosColor");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_orientation);			

			if (_locator.orientation == VOSRectLocator2d.EOrientation.HORIZONTAL)
			{
				EditorGUILayout.PropertyField(_rows);
			}
			else
			{
				EditorGUILayout.PropertyField(_columns);
			}

			EditorGUILayout.PropertyField(_size);
			EditorGUILayout.PropertyField(_delta);

			EditorGUILayout.PropertyField(_gizmosIndices);
			EditorGUILayout.PropertyField(_gizmosColor);

			serializedObject.ApplyModifiedProperties();
		}
		
	}

}
