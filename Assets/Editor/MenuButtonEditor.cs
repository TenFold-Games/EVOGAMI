using EVOGAMI.UI.Common;
using UnityEditor;
using UnityEditor.UI;

namespace EVOGAMI.Editor
{
    [CustomEditor(typeof(MenuButton), true), CanEditMultipleObjects]
    public class MenuButtonEditor : ButtonEditor
    {
        SerializedProperty textGameObject;

        SerializedProperty hoverSfx;
        SerializedProperty clickSfx;

        protected override void OnEnable()
        {
            base.OnEnable();

            textGameObject = serializedObject.FindProperty("textGameObject");
            hoverSfx = serializedObject.FindProperty("hoversfx");
            clickSfx = serializedObject.FindProperty("clicksfx");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(textGameObject);


            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(hoverSfx);
            EditorGUILayout.PropertyField(clickSfx);

            serializedObject.ApplyModifiedProperties();
        }
    }
}