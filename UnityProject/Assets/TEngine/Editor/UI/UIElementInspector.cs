using TEngine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIElement))]
public class UIElementInspector : Editor
{
    private UIElement m_Target;
    private SerializedProperty m_BindDatas;


    private void OnEnable()
    {
        m_Target = (UIElement)target;
        m_BindDatas = serializedObject.FindProperty("Elements");
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        DrawKvData();
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 绘制键值对数据
    /// </summary>
    private void DrawKvData()
    {
        //绘制key value数据

        int needDeleteIndex = -1;

        EditorGUILayout.BeginVertical();
        SerializedProperty property;
        for (int i = 0; i < m_BindDatas.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(25));

            EditorGUI.BeginDisabledGroup(true);
            property = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Name");
            property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
            property = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("BindCom");
            property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Component), true);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("X"))
            {
                //将元素下标添加进删除list
                needDeleteIndex = i;
            }

            EditorGUILayout.EndHorizontal();

        }

        //删除data
        if (needDeleteIndex != -1)
        {
            m_BindDatas.DeleteArrayElementAtIndex(needDeleteIndex);
        }

        EditorGUILayout.EndVertical();

    }
}