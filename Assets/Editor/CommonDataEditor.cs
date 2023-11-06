using Code.ScriptableObjects;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingCommonData))]
public sealed class CommonDataEditor : Editor
{
    private BuildingCommonData _commonData;
    private readonly int _maxStageNumber = 4;

    private void OnEnable() => _commonData = (BuildingCommonData)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _commonData.PrefabType = (PrefabType)EditorGUILayout.EnumPopup("PrefabType", _commonData.PrefabType);
        _commonData.Defense = EditorGUILayout.IntField("Defense", _commonData.Defense);
        _commonData.AttackRadius = EditorGUILayout.FloatField("AttackRadius", _commonData.AttackRadius);
        _commonData.TotalStages = EditorGUILayout.IntSlider("TotalStages", _commonData.TotalStages, 2, _maxStageNumber);

        EditorGUILayout.Space(7);
        ShowArrayProperty(serializedObject.FindProperty("<AutoUpgrades>k__BackingField"),
            serializedObject.FindProperty("<PriceList>k__BackingField"));

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowArrayProperty(SerializedProperty autos, SerializedProperty prices)
    {
        autos.arraySize = prices.arraySize = _maxStageNumber;
        EditorGUILayout.LabelField("Upgrade on click and prices:", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("PrebuildStage = Stage 0");
        for (int i = 0; i < _commonData.TotalStages - 1; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(autos.GetArrayElementAtIndex(i), new GUIContent($"AutoUpgrade to Stage {i + 1}:"));
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(prices.GetArrayElementAtIndex(i), new GUIContent($"Price for Stage {i + 1}:"));
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndHorizontal();
        }
    }
}