#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

// ItemSpawner스크립트 인스펙터 커스텀
[CustomEditor(typeof(ItemSpawner))]
public class ItemSpanwerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 인스펙터 표시

        // target(선택한 오브젝트)형변환 Object -> ItemSpawner
        ItemSpawner spawner = (ItemSpawner)target; 

        EditorGUILayout.Space(10); // 빈 공간
        EditorGUILayout.LabelField("컨트롤", EditorStyles.boldLabel); // 라벨

        /* ----------- 버튼 생성 ---------- */
        if (GUILayout.Button("자원 생성", GUILayout.Height(30)))
        {
            spawner.SpawnAllResources();
        }

        if (GUILayout.Button("모두 제거", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("확인", "생성된 모든 오브젝트를 삭제하시겠습니까?", "예", "아니오"))
            {
                spawner.ClearSpawnedObjects();
            }
        }
    }
}
#endif