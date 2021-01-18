
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaFramework;
using LuaInterface;
using UObject = UnityEngine.Object;
using System;

namespace LuaFramework
{
    public class ResourceManager : Manager
    {
        private string[] m_Variants = { };

        //记录总的ab名字和依赖的
        private AssetBundleManifest manifest;
        private AssetBundle assetbundle;
        //已经加载过的ab包
        private Dictionary<string, AssetBundle> bundles;

        void Awake()
        {

        }

        /// <summary>
        /// 初始化加载器
        /// </summary>
        public void Initialize(string mainFestName, Action action)
        {
            FileSearchPath.Instance.AddLuaSearchPath(Application.streamingAssetsPath);
            //FileSearchPath.Instance.AddLuaSearchPath(Application.persistentDataPath);
            FileSearchPath.Instance.AddResSearchPath(Application.streamingAssetsPath);

            if (!AppConst.LuaBundleMode)
            {
                FileSearchPath.Instance.AddLuaSearchPath(AppConst.MoonLua);
                FileSearchPath.Instance.AddLuaSearchPath(AppConst.MoontoluaDir);
                action();
            }
            else
            {
                FileSearchPath.Instance.ClearLuaBundle();
                bundles = new Dictionary<string, AssetBundle>();
                action();
            }
        }
        /// <summary>
        /// 加载资源依赖相关
        /// </summary>
        public void InitRes()
        {
            if (!AppConst.LuaBundleMode)
            {
                return;
            }
            //这里应该去处理  资源的依赖关系等  AssetBundleManifest在这里初始化
            InitInfo();

        }
        /// <summary>
        /// 初始化依赖
        /// </summary>
        public void InitInfo()
        {
            // 图片等ab包的时候 资源和ab名字依赖
            string str = FileSearchPath.Instance.GetResPath("StreamingAssets");
            assetbundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(str));
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            string[] allInfo = manifest.GetAllAssetBundles();
            for (int i = 0; i < allInfo.Length; ++i)
            {
                Debug.LogError("Info =   " + allInfo[i]);
                string[] allDep = manifest.GetAllDependencies(allInfo[i]);
                for (int j = 0; j < allDep.Length; j++)
                {
                    Debug.LogError("allDep =   " + allDep[j]);
                }
            }

        }
        /// <summary>
        /// lua层添加动更的目录
        /// </summary>
        public void AddUpdateLuaPath(string path)
        {
            FileSearchPath.Instance.AddLuaSearchPath(path, true);
        }
        /// <summary>
        /// lua层添加动更的res目录
        /// </summary>
        /// <param name="path"></param>
        public void AddUpdateResPath(string path)
        {
            FileSearchPath.Instance.AddResSearchPath(path, true);
        }

        public GameObject LoadPrefab(string abname, string assetname, LuaFunction func)
        {
            assetname = AppConst.ResPath + assetname;
            abname = abname + AppConst.ExtName;
            GameObject go = LoadAsset<GameObject>(abname, assetname);
            if (func != null)
            {
                func.Call(go);
            }
            return go;

        }
        /// <summary>
        /// 载入素材
        /// </summary>
        public T LoadAsset<T>(string abname, string assetname) where T : UnityEngine.Object
        {
            //editor命名空间无法打包
            if (AppConst.LuaBundleMode)
            {
                abname = abname.ToLower();
                
                AssetBundle bundle = LoadAssetBundle(abname);

                return bundle.LoadAsset<T>(assetname);
            }
            else
            {
#if UNITY_EDITOR
                //直接加载  打包不能使用这个AssetDatabase
                string path = assetname;
                //Debug.Log(path);
                return (T)UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif
            }
        }

        public void LoadSceneAsync(string assetName, LuaFunction fun)
        {
            LoadAsyncAsset<GameObject>(assetName, fun);
        }
        public void LoadAsyncAsset<T>(string assetName, LuaFunction fun) where T : UnityEngine.Object
        {

            if (AppConst.LuaBundleMode)
            {
                //还没写

            }
            else
            {
#if UNITY_EDITOR
                string path = AppConst.ResPath + assetName;
                T o = (T)UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

                fun.Call();
                fun.Dispose();
#endif
            }

        }




        //public void LoadPrefab(string abName, string[] assetNames, LuaFunction func)
        //{
        //    abName = abName.ToLower();
        //    List<UObject> result = new List<UObject>();
        //    for (int i = 0; i < assetNames.Length; i++)
        //    {
        //        UObject go = LoadAsset<UObject>(abName, assetNames[i]);
        //        if (go != null) result.Add(go);
        //    }
        //    if (func != null) func.Call((object)result.ToArray());
        //}

        /// <summary>
        /// 载入AssetBundle
        /// </summary>
        /// <param name="abname"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abname)
        {
            if (!abname.EndsWith(AppConst.ExtName))
            {
                abname += AppConst.ExtName;
            }
            AssetBundle bundle = null;
            if (!bundles.ContainsKey(abname))
            {
                byte[] stream = null;
                //这里需要用我自己的路径搜
                string uri = Util.DataPath + abname;
                Debug.LogWarning("LoadFile::>> " + uri);
                LoadDependencies(abname);

                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
                bundles.Add(abname, bundle);
            }
            else
            {
                bundles.TryGetValue(abname, out bundle);
            }
            return bundle;
        }

        /// <summary>
        /// 载入依赖
        /// </summary>
        /// <param name="name"></param>
        void LoadDependencies(string name)
        {
            if (manifest == null)
            {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }
            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = manifest.GetAllDependencies(name);
            if (dependencies.Length == 0) return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            for (int i = 0; i < dependencies.Length; i++)
            {
                LoadAssetBundle(dependencies[i]);
            }
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            // If the asset bundle doesn't have variant, simply return.
            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }
            if (bestFitIndex != -1)
                return bundlesWithVariant[bestFitIndex];
            else
                return assetBundleName;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        void OnDestroy()
        {
            if (manifest != null) manifest = null;
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}
