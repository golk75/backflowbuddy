using UnityEditor;
using System.IO;
using System;
#if UNITY_IOS
using UnityEditor.Callbacks;
using System.Diagnostics;
using UnityEditor.iOS.Xcode;


public class IOSPostBuild
{

    // [PostProcessBuild]
    // public static void ChangeXCodeProperties(BuildTarget buildTarget, string pathToBuiltProject)
    // {
    //     if (buildTarget == BuildTarget.iOS)
    //     {
    //         string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

    //         var pbxProject = new PBXProject();
    //         pbxProject.ReadFromFile(projectPath);
    //         var bitcodeKey = "ENABLE_BITCODE";

    //         foreach (var target in new string[] {
    //             pbxProject.GetUnityMainTargetGuid(), //Main
    //             pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName()), //Unity Tests
    //             pbxProject.GetUnityFrameworkTargetGuid() //Unity Framework
    //         })
    //             pbxProject.SetBuildProperty(target, bitcodeKey, "NO");

    //         pbxProject.WriteToFile(projectPath);
    //     }
    // }
}
#endif