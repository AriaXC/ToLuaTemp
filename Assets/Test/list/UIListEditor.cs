using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(UIList))]
public class UIListEditor : Editor
{
    //protected GUIContent my_Direction = new GUIContent("Direction", "滚动方向");


    protected SerializedProperty m_viewPort;
    protected SerializedProperty m_Count;
    protected SerializedProperty m_s;

    //public static Vector2 vv = Vector2.zero;

    public void OnEnable()
    {
        m_viewPort = serializedObject.FindProperty("m_viewPort");
        m_Count = serializedObject.FindProperty("Count");
        m_s = serializedObject.FindProperty("s");
 
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        //UIList.IsH = EditorGUILayout.Toggle("水平", UIList.IsH);
        //if (UIList.IsH)
        //{
        //    UIList.IsV = false;
        //}

        ////UIList.IsV = EditorGUILayout.Toggle("垂直", UIList.IsV);
        //if (UIList.IsV)
        //{
        //    UIList.IsH = false;
        //}



        //vv= EditorGUILayout.Vector2Field("@@@", vv);

        //UIList.gap = EditorGUILayout.Vector2Field("间距", UIList.gap);
        ////EditorGUILayout.LabelField("CountC", m.ToString());

        //m_Count.vector2Value = EditorGUILayout.Vector2Field("视窗@@大小", m_Count.vector2Value);

        //serializedObject.ApplyModifiedProperties();


    }

    protected void MarkScene(bool isDirty = true)
    {
        if (Application.isPlaying && isDirty)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
