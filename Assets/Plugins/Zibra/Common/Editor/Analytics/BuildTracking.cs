
using com.zibra.common.Editor;
using com.zibra.common.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.UIElements;

namespace com.zibra.common.Analytics
{
    internal class BuildTracking : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostprocessBuild(BuildReport report)
        {
            Dictionary<string, object> eventProperties = new Dictionary<string, object>
            {
                { "Render_pipeline", RenderPipelineDetector.GetRenderPipelineType().ToString() }
            };
            switch (report.summary.platform)
            {
                case BuildTarget.StandaloneOSX:
                    eventProperties.Add("Built_platform", "MacOS");
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    eventProperties.Add("Built_platform", "Windows");
                    break;
                case BuildTarget.iOS:
                    eventProperties.Add("Built_platform", "IOS");
                    break;
                case BuildTarget.Android:
                    eventProperties.Add("Built_platform", "Android");
                    break;
                case BuildTarget.WSAPlayer:
                    eventProperties.Add("Built_platform", "UWP");
                    break;
                case BuildTarget.StandaloneLinux64:
                    eventProperties.Add("Built_platform", "Linux");
                    break;
            }
#if ZIBRA_EFFECTS_OTP_VERSION
            if (PluginManager.IsAvailable(PluginManager.Effect.Liquid))
            {
                AnalyticsManager.GetInstance(PluginManager.Effect.Liquid).TrackEvent(new AnalyticsManager.AnalyticsEvent
                {
                    EventType = "Liquid_built",
                    Properties = eventProperties
                });
            }
            if (PluginManager.IsAvailable(PluginManager.Effect.Smoke))
            {
                AnalyticsManager.GetInstance(PluginManager.Effect.Smoke).TrackEvent(new AnalyticsManager.AnalyticsEvent
                {
                    EventType = "SF_built",
                    Properties = eventProperties
                });
            }
#else
            AnalyticsManager.GetInstance("effects").TrackEvent(new AnalyticsManager.AnalyticsEvent
            {
                EventType = "Effects_built",
                Properties = eventProperties
            });
#endif
        }
    }
}