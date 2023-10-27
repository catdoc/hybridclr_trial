using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : EditorWindow
{
    [MenuItem("打包/WebGL/资源包和场景")]
    public static void BuildAbsAndScenesWindows()
    {
        AssetBundleBuilder assetBundleBuilder = EditorWindow.GetWindow<AssetBundleBuilder>();
        assetBundleBuilder.Show();
    }

    static string outPath;

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("AssetBundle文件名称:", GUILayout.MaxWidth(160));
        outPath = EditorGUILayout.TextField(outPath);
        EditorGUI.EndDisabledGroup();
        //string s = EditorGUILayout.TextArea("s");
        if (GUILayout.Button("开始打包场景和资源"))
        {
            BuildAbsAndScenes(BuildTarget.WebGL);
        }
        //GUILayout.EndVertical();
        //EndWindows();
    }

    // [MenuItem("打包/Android/资源包和场景")]
    // public static void BuildAbsAndScenesAndroid()
    // {
    //     BuildAbsAndScenes(BuildTarget.Android);
    // }

    // [MenuItem("打包/IOS/资源包和场景")]
    // public static void BuildAbsAndScenesIOS()
    // {
    //     BuildAbsAndScenes(BuildTarget.iOS);
    // }

    // [MenuItem("打包/Android/资源包")]
    // public static void BuildAbsAndroid()
    // {
    //     BuildAssetBundles(BuildTarget.Android);
    // }

    [MenuItem("打包/WebGL/资源包")]
    public static void BuildAbsWebGL()
    {
        BuildAssetBundles(BuildTarget.WebGL);
    }

    // [MenuItem("打包/Android/场景")]
    // public static void BuildScenesAndroid()
    // {
    //     BuildScenes(BuildTarget.Android);
    // }

    // [MenuItem("打包/WebGL/场景")]
    // public static void BuildScenesIOS()
    // {
    //     BuildScenes(BuildTarget.WebGL);
    // }

    // 打包AssetBundle和Scenes
    public static void BuildAbsAndScenes(BuildTarget platform)
    {
        BuildAssetBundles(platform);
        BuildScenes(platform);
    }

    // 打包AssetBundles
    private static void BuildAssetBundles(BuildTarget platform)
    {
        // 输出路径
        string outPath = Application.streamingAssetsPath + "/../../res_ab";
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
        EditorUtility.DisplayProgressBar("信息", "打包资源包", 0f);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.DeterministicAssetBundle, platform);
        AssetDatabase.Refresh();
        Debug.Log("所有资源包打包完毕");
    }

    // 打包Scenes
    private static void BuildScenes(BuildTarget platform)
    {
        // 指定场景文件夹和输出路径
        string scenePath = Application.dataPath + "/Scenes";
        string outPath = Application.streamingAssetsPath + "/Scenes";
        if (Directory.Exists(scenePath))
        {
            // 创建输出文件夹
            if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

            // 查找指定目录下的场景文件
            string[] scenes = GetAllFiles(scenePath, "*.unity");
            for (int i = 0; i < scenes.Length; i++)
            {
                string url = scenes[i].Replace("\\", "/");
                int index = url.LastIndexOf("/");
                string scene = url.Substring(index + 1, url.Length - index - 1);
                string msg = string.Format("打包场景{0}", scene);
                EditorUtility.DisplayProgressBar("信息", msg, 0f);
                scene = scene.Replace(".unity", ".scene");
                Debug.Log(string.Format("打包场景{0}到{1}", url, outPath + scene));
                BuildPipeline.BuildPlayer(scenes, outPath + scene, platform, BuildOptions.BuildAdditionalStreamedScenes);
                AssetDatabase.Refresh();
            }
            EditorUtility.ClearProgressBar();
            Debug.Log("所有场景打包完毕");
        }
    }

    /// <summary> 获取文件夹和子文件夹下所有指定类型文件 </summary>
    private static string[] GetAllFiles(string directory, params string[] types)
    {
        if (!Directory.Exists(directory)) return new string[0];
        string searchTypes = (types == null || types.Length == 0) ? "*.*" : string.Join("|", types);
        string[] names = Directory.GetFiles(directory, searchTypes, SearchOption.AllDirectories);
        return names;
    }
}
