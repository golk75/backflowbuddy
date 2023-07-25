using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeScriptableObject
{
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        TestCock asset = ScriptableObject.CreateInstance<TestCock>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
