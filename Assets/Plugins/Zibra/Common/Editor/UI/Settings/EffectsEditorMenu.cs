#if UNITY_2019_4_OR_NEWER
using com.zibra.common.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace com.zibra.common
{
    /// <summary>
    /// Class that contains code for useful actions for the plugin
    /// Those actions exposed to user via MenuItem
    /// You can call them from C# via ExecuteMenuItem
    /// </summary>
    internal static class EffectsEditorMenu
    {
        [MenuItem(Effects.BaseMenuBarPath + "View License Details", false, 2)]
        static void OpenSettings()
        {
            var windowTitle = EffectsSettingsWindow.WindowTitle;
            EffectsSettingsWindow.ShowTowardsInspector(windowTitle.text, windowTitle.image);
        }

        static void OpenFile(string GUID)
        {
            string dataPath = Application.dataPath;
            string projectPath = dataPath.Replace("/Assets", "");
            string filePath = AssetDatabase.GUIDToAssetPath(GUID);
            Application.OpenURL("file://" + projectPath + "/" + filePath);
        }

        [MenuItem(Effects.BaseMenuBarPath + "Open User Guide", false, 30)]
        static void OpenUserGuide()
        {
            OpenFile("09ace81bf2ac0bd4e8c853cda11f7c84");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Open API Reference", false, 31)]
        static void OpenAPIDocumentation()
        {
            string dataPath = Application.dataPath;
            string projectPath = dataPath.Replace("/Assets", "");
            string documentationPath = AssetDatabase.GUIDToAssetPath("d9e57e1e9783349ffa44b5f943410fab");
            Application.OpenURL("file://" + projectPath + "/" + documentationPath + "/index.html");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Open Changelog", false, 33)]
        static void OpenChangelog()
        {
            OpenFile("b667af1f31c554a3299ea0e7db5ad45a");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Open Known Issues List", false, 33)]
        static void OpenKnownIssues()
        {
            OpenFile("8c104e5e29fc2bc48b6e83e32bb63679");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Browse Assets on Unity Asset Store", false, 34)]
        static void OpenAssetStore()
        {
            Application.OpenURL("https://effects.zibra.ai/open");
        }        

        [MenuItem(Effects.BaseMenuBarPath + "Contact us/Discord", false, 1000)]
        static void OpenDiscord()
        {
            Application.OpenURL("https://discord.gg/QzypP8n7uB");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Contact us/Support E-Mail", false, 1010)]
        static void OpenSupportEmail()
        {
            Application.OpenURL("mailto:support@zibra.ai");
        }

        [MenuItem(Effects.BaseMenuBarPath + "Open Sample Scene/Combined Liquid and Smoke", false, 29)]
        static void OpenCombinedEffectsSampleScene()
        {
            string GUID = "";
            switch (RenderPipelineDetector.GetRenderPipelineType())
            {
                case RenderPipelineDetector.RenderPipeline.BuiltInRP:
                    GUID = "2eaf9a27e7194a6797736a1b063e581f";
                    break;
                case RenderPipelineDetector.RenderPipeline.URP:
                    GUID = "d9a09277d7d7dc4449a5299b4f122bbb";
                    break;
                case RenderPipelineDetector.RenderPipeline.HDRP:
                    GUID = "f8335ff236204cc468c5c836e4bc180d";
                    break;
            }
            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(GUID));
        }
    }
}
#endif
