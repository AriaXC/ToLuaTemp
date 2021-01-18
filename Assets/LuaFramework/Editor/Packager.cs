using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;
using System.IO;
using System.Xml;
using Debug = UnityEngine.Debug;

/// <summary>
/// 这个类是要去记录xml文件生成所需要的信息
/// </summary>
public class AssetBundleMoonInfo
{
    //ab包名字
    public string bundleName;
    //文件名字
    public List<string> assertName ;
    //依赖列表
    public List<string> deps;
}

public class Packager {
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    static List<AssetBundleBuild> LuaMaps = new List<AssetBundleBuild>();
    static Dictionary<string,AssetBundleMoonInfo> ResDic =new Dictionary<string, AssetBundleMoonInfo>();

    ///-----------------------------------------------------------
    static string[] exts = { ".txt", ".xml", ".lua", ".assetbundle", ".json" };
    static bool CanCopy(string ext) {   //能不能复制
        foreach (string e in exts) {
            if (ext.Equals(e)) return true;
        }
        return false;
    }
    
    /// <summary>
    /// 载入素材
    /// </summary>
    static UnityEngine.Object LoadAsset(string file) {
        UnityEngine.Debug.LogError("我不想走到这里的");
        if (file.EndsWith(".lua")) file += ".txt";
        return AssetDatabase.LoadMainAssetAtPath("Assets/LuaFramework/Examples/Builds/" + file);
    }

    [MenuItem("LuaFramework/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource() {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
        target = BuildTarget.iOS;
#endif
        BuildAssetResource(target);
    }

    ////[MenuItem("LuaFramework/Build Android Resource", false, 101)]
    //public static void BuildAndroidResource() {
    //    BuildAssetResource(BuildTarget.Android);
    //}

    ////[MenuItem("LuaFramework/Build Windows Resource", false, 102)]
    //public static void BuildWindowsResource() {
    //    BuildAssetResource(BuildTarget.StandaloneWindows);
    //}

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target) {
        string outPath = "Assets/" + AppConst.AssetDir;
        if (Directory.Exists(outPath))
            Directory.Delete(outPath, true);

        if (Directory.Exists(Util.DataPath))
        {
            Directory.Delete(Util.DataPath, true);
        }
        Directory.CreateDirectory(outPath);
        AssetDatabase.Refresh();

        LuaMaps.Clear();
        ResDic.Clear();
        if (AppConst.LuaBundleMode) {
            HandleLuaBundle();
        } else {
            //HandleLuaFile();
        }
        if (AppConst.ExampleMode)
        {
            //HandleExampleBundle();
        }
        List<string> resList = GetAllResDirs(AppConst.ResPath);
        if (resList.Count > 0 && resList != null)
        {
            for (int i = 0; i < resList.Count; i++)
            {
                Debug.Log("文件目录===" + resList[i]);
                SetAssetBundleName(resList[i]);
            }
        }

        BuildPipeline.BuildAssetBundles(outPath, LuaMaps.ToArray(), BuildAssetBundleOptions.None, target);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, target);
        BuildFileIndex();

        UnityEngine.Debug.Log("打出ab资源了,开始生成xml");

        //分析依赖
        foreach (var info in ResDic)
        {
            string[] str = AssetDatabase.GetAssetBundleDependencies(info.Value.bundleName, true);
            foreach (var s in str)
            {
                info.Value.deps.Add(s);
            }
        }
        //生成XML
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("bundles");

        foreach (var info in ResDic)
        {
            XmlElement item = doc.CreateElement("bundle");
            XmlElement bundleName = doc.CreateElement("bundleName");
            bundleName.InnerText = info.Value.bundleName;
            item.AppendChild(bundleName);
            foreach (string s in info.Value.assertName)
            {
                XmlElement AssertName = doc.CreateElement("AssertName");
                AssertName.InnerText =s;
                item.AppendChild(AssertName);
            }
            foreach (string s in info.Value.deps)
            {
                XmlElement deps = doc.CreateElement("deps");
                deps.InnerText = s;
                item.AppendChild(deps);
            }
            root.AppendChild(item);
        }
        doc.AppendChild(root);
        doc.Save(outPath+"/"+AppConst.MoonXml);
        UnityEngine.Debug.Log("xml生成完毕");

        //BuildPipeline
        AssetDatabase.Refresh();
    }

    // 添加到打包路径中
    static void AddBuildMap(string bundleName, string pattern, string path) {
        string[] files_lua = Directory.GetFiles(path, pattern);
        if (files_lua.Length == 0) return;
   
        for (int i = 0; i < files_lua.Length; i++) {
            files_lua[i] = files_lua[i].Replace('\\', '/');
        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundleName;
        build.assetNames = files_lua;
        LuaMaps.Add(build);
    }
    static void SetAssetBundleName(string fullPath)
    {
        string buildScenePath = "Assets/Res/Scene";
        Dictionary<string, bool> dirMap = new Dictionary<string, bool>();
        // 遍历所有文件设置bundleName
   
        string[] files = Directory.GetFiles(fullPath);
        if (files == null || files.Length == 0)
        {
            return;
        }
        // 处理dirBundleName
        string dirBundleName = fullPath.Substring(AppConst.ResPath.Length);
        //fullPath.Substring (AppConst.ResourcesPath.Length);
        if (dirBundleName == "")
        {
            dirBundleName = "res";
        }
        Debug.Log("dirBundleName: " + dirBundleName);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	        dirBundleName = dirBundleName.Replace ("\\", "/");
	        buildScenePath = buildScenePath.Replace ("\\", "/");
#endif
        dirBundleName = dirBundleName.Replace("/", "@") + AppConst.ExtName;

        foreach (string oneFile in files)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            filePath = filePath.Replace ("\\", "/");
#endif
            Debug.Log("EndsWith ==  " + Path.GetExtension(oneFile));
            if (oneFile.EndsWith(".meta")
                || oneFile.EndsWith(".DS_Store")
                || oneFile.EndsWith(".unity")
                || oneFile.EndsWith(".lua"))
            {
                continue;
            }
            else if (oneFile.StartsWith(buildScenePath) || oneFile.StartsWith(AppConst.LuaTempDir))
            {
                continue;
            } /*else if (filePath.StartsWith (audioPath)) {
                continue;
            }*/

            // 设置bundleName
            AssetImporter importer = AssetImporter.GetAtPath(oneFile);
            // 构建AssetBundle信息 用于xml
            AssetBundleMoonInfo info = new AssetBundleMoonInfo();
            info.assertName = new List<string>();
            info.deps = new List<string>();

            if (importer != null)
            {
                string ext = Path.GetExtension(oneFile);
                string bundleName = dirBundleName;
                if (null != ext && ext.Equals(".prefab"))
                {
                    // prefab单个文件打包
                    bundleName = oneFile.Substring(AppConst.ResPath.Length);
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
                //这个不知道是什么类型的
                if (null != ext && ext.Equals(".dds"))
                {
                    UnityEngine.Debug.Log("Error image format!!! " + oneFile);
                    continue;
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
                        string dir = Path.GetDirectoryName(oneFile);
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
                            bundleName = oneFile.Substring(AppConst.ResPath.Length);
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
                //重新把ab包的名字设置了下
                importer.assetBundleName = bundleName;
                //Debug.Log("assetBundleName :" + bundleName);

                // 存储bundleInfo
                if (ResDic.ContainsKey(bundleName))
                {
                    ResDic[bundleName].assertName.Add(oneFile);
                }
                else
                {
                    info.assertName.Add(oneFile);
                    info.bundleName = bundleName;
                    ResDic.Add(info.bundleName, info);
                }
                Debug.Log("info == " + info.assertName + "   bundleName==" + info.bundleName);   
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat("Set AssetName Fail, File:{0}, Msg:Importer is null", oneFile);
            }

        }
    }
    /// <summary>
    /// 获取该目录下的所有文件夹
    /// </summary>
    static List<string> GetAllResDirs(string path)
    {
        List<string> outList = new List<string>();

        GetAllDirs(path,outList);
        return outList;
    }
    static void GetAllDirs(string path, List<string> outList)
    {
        if (path == null && path.EndsWith(".meta"))
        {
            return;
        }
        string[] files = Directory.GetDirectories(path);
        if (files != null && files.Length > 0)
        {
            for (int i = 0; i < files.Length; i++)
            {
                outList.Add(files[i]);
                GetAllDirs(files[i], outList);
            }
        }
    }
    /// <summary>
    /// 处理Lua代码包
    /// </summary>
    static void HandleLuaBundle() {

        string streamDir = Application.dataPath + "/" + AppConst.LuaTempDir;
        if (Directory.Exists(streamDir))
            Directory.Delete(streamDir, true);

        Directory.CreateDirectory(streamDir);

        string[] srcDirs = { CustomSettings.luaDir, CustomSettings.FrameworkPath + "/ToLua/Lua" };
        for (int i = 0; i < srcDirs.Length; i++)
        {
            if (AppConst.LuaByteMode)
            {
                string sourceDir = srcDirs[i];
                string[] files = Directory.GetFiles(sourceDir, "*.lua", SearchOption.AllDirectories);
                int len = sourceDir.Length;

                if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
                {
                    --len;
                }
                for (int j = 0; j < files.Length; j++)
                {
                    string str = files[j].Remove(0, len);
                    string dest = streamDir + str + ".bytes";
                    string dir = Path.GetDirectoryName(dest);
                    Directory.CreateDirectory(dir);
                    EncodeLuaFile(files[j], dest);
                }
            }
            else
            {
                //复制到temp文件夹  然后添加后缀 打进ab
                ToLuaMenu.CopyLuaBytesFiles(srcDirs[i], streamDir);
            }
        }

        if (!Directory.Exists(streamDir)) Directory.CreateDirectory(streamDir);

        string[] dirs = Directory.GetDirectories(streamDir, "*", SearchOption.AllDirectories);
        for (int i = 0; i < dirs.Length; i++)
        {
            string name = dirs[i].Replace(streamDir, string.Empty);
            name = name.Replace('\\', '_').Replace('/', '_');
            name = "lua" + AppConst.ExtName;

            string path = "Assets" + dirs[i].Replace(Application.dataPath, ""); 
            AddBuildMap(name, "*.bytes", path);
        }
        AddBuildMap("lua" + AppConst.ExtName, "*.bytes","Assets/"+ AppConst.LuaTempDir);


        //-------------------------------处理非Lua文件----------------------------------
        string luaPath = Application.dataPath + "/" + AppConst.LuaTempDir +"lua/";
        for (int i = 0; i < srcDirs.Length; i++)
        {
            paths.Clear(); files.Clear();
            string luaDataPath = srcDirs[i].ToLower();
            Recursive(luaDataPath);
            foreach (string f in files)
            {
                if (f.EndsWith(".meta") || f.EndsWith(".lua")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string path = Path.GetDirectoryName(luaPath + newfile);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string destfile = path + "/" + Path.GetFileName(f);
                File.Copy(f, destfile, true);
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 处理Lua文件
    /// </summary>
    static void HandleLuaFile() { 
        string resPath = AppDataPath + "/StreamingAssets/";
        string luaPath = resPath + "/lua/";

        //----------复制Lua文件----------------
        if (!Directory.Exists(luaPath)) {
            Directory.CreateDirectory(luaPath); 
        }
        string[] luaPaths = { AppDataPath + "/Lua/", 
                              AppDataPath + "/LuaFramework/Tolua/Lua/" };

        for (int i = 0; i < luaPaths.Length; i++) {
            paths.Clear(); files.Clear();
            string luaDataPath = luaPaths[i].ToLower();
            Recursive(luaDataPath);
            int n = 0;
            foreach (string f in files) {
                if (f.EndsWith(".meta")) continue;
                string newfile = f.Replace(luaDataPath, "");
                string newpath = luaPath + newfile;
                string path = Path.GetDirectoryName(newpath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                if (File.Exists(newpath)) {
                    File.Delete(newpath);
                }
                if (AppConst.LuaByteMode) {
                    EncodeLuaFile(f, newpath);
                } else {
                    File.Copy(f, newpath, true);
                }
                UpdateProgress(n++, files.Count, newpath);
            } 
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    //写入files 信息
    static void BuildFileIndex() {
        string resPath = AppDataPath + "/StreamingAssets/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++) {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = Util.md5file(file);
            string value = file.Replace(resPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 数据目录
    /// </summary>
    static string AppDataPath {
        get { return Application.dataPath.ToLower(); }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path) {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names) {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs) {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc) {
        string title = "Processing...[" + progress + " - " + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }

    public static void EncodeLuaFile(string srcFile, string outFile) {
        if (!srcFile.ToLower().EndsWith(".lua")) {
            File.Copy(srcFile, outFile, true);
            return;
        }
        bool isWin = true; 
        string luaexe = string.Empty;
        string args = string.Empty;
        string exedir = string.Empty;
        string currDir = Directory.GetCurrentDirectory();
        if (Application.platform == RuntimePlatform.WindowsEditor) {
            isWin = true;
            luaexe = "luajit.exe";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            isWin = false;
            luaexe = "./luajit";
            args = "-b -g " + srcFile + " " + outFile;
            exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit_mac/";
        }
        Directory.SetCurrentDirectory(exedir);
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = luaexe;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = isWin;
        info.ErrorDialog = true;
        Util.Log(info.FileName + " " + info.Arguments);

        Process pro = Process.Start(info);
        pro.WaitForExit();
        Directory.SetCurrentDirectory(currDir);
    }

    [MenuItem("LuaFramework/Build Protobuf-lua-gen File")]
    public static void BuildProtobufFile() {
        if (!AppConst.ExampleMode) {
            UnityEngine.Debug.LogError("若使用编码Protobuf-lua-gen功能，需要自己配置外部环境！！");
            return;
        }
        string dir = AppDataPath + "/Lua/3rd/pblua";
        paths.Clear(); files.Clear(); Recursive(dir);

        string protoc = "d:/protobuf-2.4.1/src/protoc.exe";
        string protoc_gen_dir = "\"d:/protoc-gen-lua/plugin/protoc-gen-lua.bat\"";

        foreach (string f in files) {
            string name = Path.GetFileName(f);
            string ext = Path.GetExtension(f);
            if (!ext.Equals(".proto")) continue;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = protoc;
            info.Arguments = " --lua_out=./ --plugin=protoc-gen-lua=" + protoc_gen_dir + " " + name;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = true;
            info.WorkingDirectory = dir;
            info.ErrorDialog = true;
            Util.Log(info.FileName + " " + info.Arguments);

            Process pro = Process.Start(info);
            pro.WaitForExit();
        }
        AssetDatabase.Refresh();
    }
}