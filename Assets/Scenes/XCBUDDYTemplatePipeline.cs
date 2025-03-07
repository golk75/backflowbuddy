#if UNITY_EDITOR


using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;

public class XCBUDDYTemplatePipeline : ISceneTemplatePipeline
{
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
    {
        return true;
    }

    public virtual void BeforeTemplateInstantiation(
        SceneTemplateAsset sceneTemplateAsset,
        bool isAdditive,
        string sceneName
    ) { }

    public virtual void AfterTemplateInstantiation(
        SceneTemplateAsset sceneTemplateAsset,
        Scene scene,
        bool isAdditive,
        string sceneName
    ) { }
}
#endif
