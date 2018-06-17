using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageEditorLoad))]   //target

public class StageMapEditor : Editor
{
    StageEditorLoad manager;


    void OnEnable()
    {
        //Character 컴포넌트를 얻어오기
        manager = (StageEditorLoad)target;


    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel("MapIndex");
        manager.m_StageNumber = EditorGUILayout.IntSlider(manager.m_StageNumber, 0, 11, null);

        EditorGUILayout.EndHorizontal();


  //      EditorGUILayout.BeginFadeGroup(2f);
        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("불러오기"))
        {
            manager.MapLoad();
        }

        if (GUILayout.Button("저장하기"))
        {
            manager.MapSave();
        }

        EditorGUILayout.EndHorizontal();
        //  EditorGUILayout.EndFadeGroup();

        EditorGUILayout.PrefixLabel("");
        EditorGUILayout.PrefixLabel("BlockType");
  
        manager.m_LoadType = EditorGUILayout.IntSlider(manager.m_LoadType, 0, 9, null);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("블록 추가"))
        {
            manager.BlockAdd();
        }

        if (GUILayout.Button("블록 삭제"))
        {
            manager.BlockSub();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PrefixLabel("");
        EditorGUILayout.PrefixLabel("카메라 조작");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("◀◀"))
        {
            manager.cameraLeft();
        }

        if (GUILayout.Button("▶▶"))
        {
            manager.cameraRight();
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("시작"))
        {
            manager.StartCoroutine("CameraStart");
        }

        if (GUILayout.Button("끝"))
        {
            manager.StartCoroutine("CameraEnd");
        }

        EditorGUILayout.EndHorizontal();
    }

}
