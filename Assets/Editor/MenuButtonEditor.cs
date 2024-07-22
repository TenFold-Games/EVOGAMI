using EVOGAMI.UI.Common;
using UnityEditor;
using UnityEditor.UI;

namespace EVOGAMI.Editor
{
    [CustomEditor(typeof(MenuButton), true), CanEditMultipleObjects]
    public class MenuButtonEditor : ButtonEditor
    {
        // Text
        private SerializedProperty _changeTextColor;
        private SerializedProperty _textGameObject;

        // Audio
        private SerializedProperty _hoverSfx;
        private SerializedProperty _clickSfx;

        protected override void OnEnable()
        {
            base.OnEnable();

            _changeTextColor = serializedObject.FindProperty("changeTextColor");
            _textGameObject = serializedObject.FindProperty("textGameObject");
            _hoverSfx = serializedObject.FindProperty("hoverSfx");
            _clickSfx = serializedObject.FindProperty("clickSfx");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_changeTextColor);
            EditorGUILayout.PropertyField(_textGameObject);


            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_hoverSfx);
            EditorGUILayout.PropertyField(_clickSfx);

            serializedObject.ApplyModifiedProperties();
        }
    }
}