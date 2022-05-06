using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileConfig))]
public class TileConfigEditor : Editor
{
    private SerializedProperty m_ID;
    private SerializedProperty m_Sprite;
    private SerializedProperty m_Up;
    private SerializedProperty m_Right;
    private SerializedProperty m_Down;
    private SerializedProperty m_Left;

    private TileConfig tileConfig;
    private void OnEnable()
    {
        m_ID = serializedObject.FindProperty("ID");
        m_Sprite = serializedObject.FindProperty("Preview");
        tileConfig = (TileConfig)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_ID);
        EditorGUILayout.PropertyField(m_Sprite);
        if (tileConfig.Preview != null)
            GUILayout.Box(tileConfig.Preview.sprite.texture, GUILayout.Width(50), GUILayout.Height(50));

        //EditorGUILayout.PropertyField(m_Up);
        //if (m_Up.isExpanded)
        //{
        //    bool isGridEnd = true;
        //    if (tileConfig.Up !=null && tileConfig.Up.Count > 0)
        //    {
        //        for (int i = 0; i < tileConfig.Up.Count; i++)
        //        {
        //            if (isGridEnd)
        //            {
        //                EditorGUILayout.BeginHorizontal();
        //                isGridEnd = false;
        //            }

        //            if (tileConfig.Up[i] != null)
        //                GUILayout.Box(tileConfig.Up[i].Preview.sprite.texture, GUILayout.Width(80), GUILayout.Height(80));
        //            if (i != 0 && i % 2 == 0)
        //            {
        //                EditorGUILayout.EndHorizontal();
        //                isGridEnd = true;
        //            }
        //        }
        //        if (!isGridEnd)
        //            EditorGUILayout.EndHorizontal();
        //    }
        //}
        //EditorGUILayout.PropertyField(m_Right);
        //EditorGUILayout.PropertyField(m_Down);
        //EditorGUILayout.PropertyField(m_Left);
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawList(SerializedProperty sp, List<TileConfig> targetList)
    {
        EditorGUILayout.PropertyField(sp);
        if (sp.isExpanded)
        {
            bool isGridEnd = true;
            if (targetList != null && targetList.Count > 0)
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    if (isGridEnd)
                    {
                        EditorGUILayout.BeginHorizontal();
                        isGridEnd = false;
                    }

                    if (targetList[i] != null)
                        GUILayout.Box(targetList[i].Preview.sprite.texture, GUILayout.Width(80), GUILayout.Height(80));
                    if (i != 0 && i % 2 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        isGridEnd = true;
                    }
                }
                if (!isGridEnd)
                    EditorGUILayout.EndHorizontal();
            }
        }
    }
}
