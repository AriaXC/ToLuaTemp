using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LuaFramework;

public class ImportSetting : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        //设置成安卓 我不是很懂
        TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;


        //打图集策略  每单个的文件夹是一个图集 
        string dirName = Path.GetDirectoryName(assetPath);
        string folderStr = Path.GetFileName(dirName);
        textureImporter.spritePackingTag = folderStr;
    }

	static void OnPostprocessAllAssets(
	string[] importedAssets,
	string[] deletedAssets,
	string[] modedAssets,
	string[] movedFromAssetPath)
	{
		//从命令行使用-batchmode标志启动Unity时返回true（只读
		if (!Application.isBatchMode)
		{
			SetABName(importedAssets);
		}
	}


	static void SetABName(string[] assets)
	{
		foreach (var asset in assets)
		{
            if (!asset.Contains("Assets/Res") && !asset.Contains("Assets\\Res"))
			{
				return;
			}
			SetAssetBundleName(asset);
		}
	}
    static void SetAssetBundleName(string fullPath)
    {
        Debug.Log("ImportSetAssetBundleName: " + fullPath);
        string buildScenePath = "Assets/Res/Scene";

        // string[] files = Directory.GetFiles (fullPath);
        // if (files == null || files.Length == 0) {
        //     return;
        // }
        Dictionary<string, bool> dirMap = new Dictionary<string, bool>();

        // 处理dirBundleName
        string dirBundleName = Path.GetDirectoryName(fullPath).Substring(AppConst.ResPath.Length); //fullPath.Substring (AppConst.ResourcesPath.Length);
        if (dirBundleName == "")
        {
            dirBundleName = "res";
        }
        Debug.Log("ImportDirBundleName: " + dirBundleName);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	        dirBundleName = dirBundleName.Replace ("\\", "/");
	        buildScenePath = buildScenePath.Replace ("\\", "/");
#endif
        dirBundleName = dirBundleName.Replace("/", "@") + AppConst.ExtName;


        // 判断是否需要merge
        bool mergePath = false;
        // if (mergePathMap.ContainsKey(fullPath)) {
        //     mergePath = true;
        //     dirBundleName = mergePathMap[fullPath];
        // }
        bool nopackPath = false;
        // if (nopackPathMap.ContainsKey(fullPath)) {
        //     nopackPath = true;
        // }

        // 遍历所有文件设置bundleName

        string filePath = fullPath;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            filePath = filePath.Replace ("\\", "/");
#endif

        if (filePath.EndsWith(".meta")
            || filePath.EndsWith(".DS_Store")
            || filePath.EndsWith(".unity")
            || filePath.EndsWith(".tpsheet")
            || filePath.EndsWith("split.txt")
            || filePath.EndsWith("merge.txt")
            || filePath.EndsWith("nopack.txt")
            || filePath.EndsWith(".lua"))
        {
            // continue;
            return;
        }
        else if (filePath.StartsWith(buildScenePath) || filePath.StartsWith(AppConst.LuaTempDir))
        {
            // continue;
            return;
        } /*else if (filePath.StartsWith (audioPath)) {
                continue;
            }*/
        string file = filePath;
        // 设置bundleName
        AssetImporter importer = AssetImporter.GetAtPath(file);
        if (importer != null)
        {
            string ext = Path.GetExtension(file);
            string bundleName = dirBundleName;
            if (null != ext && ext.Equals(".prefab") && !mergePath)
            {
                // prefab单个文件打包
                bundleName = filePath.Substring(AppConst.ResPath.Length);
                bundleName = bundleName.Replace("/", "@");
                if (null != ext)
                {
                    bundleName = bundleName.Replace(ext, AppConst.ExtName);
                }
                else
                {
                    bundleName += AppConst.ExtName;
                }
            }
            if (null != ext && ext.Equals(".dds"))
            {
                UnityEngine.Debug.Log("Error image format!!! " + file);
                // continue;
                return;
            }
            else
            {
                bool spritepack = false;
                if (importer is TextureImporter)
                {
                    TextureImporter textureImporter = importer as TextureImporter;
                    if (!string.IsNullOrEmpty(textureImporter.spritePackingTag))
                    {
                        // 图集打包
                        bundleName = "spritepack@" + textureImporter.spritePackingTag + AppConst.ExtName;
                        spritepack = true;
                    }
                }

                if (!spritepack)
                {
                    if (nopackPath)
                    {
                        // 无需打包
                        // continue;
                        return;
                    }
                    string dir = Path.GetDirectoryName(file);
                    bool pack = false;
                    if (dirMap.ContainsKey(dir))
                    {
                        pack = dirMap[dir];
                    }
                    else
                    {
                        pack = !File.Exists(Path.Combine(dir, "split.txt"));
                        dirMap.Add(dir, pack);
                    }

                    if (!pack)
                    {
                        // 当个文件打包
                        bundleName = filePath.Substring(AppConst.ResPath.Length);
                        bundleName = bundleName.Replace("/", "@");
                        if (null != ext)
                        {
                            bundleName = bundleName.Replace(ext, AppConst.ExtName);
                        }
                        else
                        {
                            bundleName += AppConst.ExtName;
                        }
                    }
                }
            }
            bundleName = bundleName.ToLower();
            importer.assetBundleName = bundleName;
            Debug.Log("ImportAssetBundleName :" + bundleName);
        }
        else
        {
            UnityEngine.Debug.LogWarningFormat("Set AssetName Fail, File:{0}, Msg:Importer is null", filePath);
        }

    }

}
