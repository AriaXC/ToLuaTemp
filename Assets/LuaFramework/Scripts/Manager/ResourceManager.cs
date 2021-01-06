
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaFramework;
using LuaInterface;
using UObject = UnityEngine.Object;
using UnityEditor;
using System;

namespace LuaFramework {
    public class ResourceManager : Manager {
        private string[] m_Variants = { };
        private AssetBundleManifest manifest;
        private AssetBundle shared, assetbundle;
        private Dictionary<string, AssetBundle> bundles;

        void Awake() {
        }

        /// <summary>
        /// 初始化加载器
        /// </summary>
        public void Initialize(string mainFestName,Action action) {

            if (!AppConst.LuaBundleMode)
            { 

                action();
                return;
            }
            //ab包模式代码还没写
            byte[] stream = null;
            string uri = string.Empty;
            bundles = new Dictionary<string, AssetBundle>();
            uri = Util.DataPath + AppConst.AssetDir;
            if (!File.Exists(uri)) return;
         
            stream = File.ReadAllBytes(uri);
            assetbundle = AssetBundle.LoadFromMemory(stream);
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
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
            //这里应该去处理  资源的依赖关系等



        }

        public void LoadSceneAsync(string assetName, LuaFunction fun)
        {

        }
        /// <summary>
        /// 载入素材
        /// </summary>
        public T LoadAsset<T>(string abname, string assetname) where T : UnityEngine.Object {
            if (AppConst.LuaBundleMode)
            {
                abname = abname.ToLower();
                AssetBundle bundle = LoadAssetBundle(abname);
                return bundle.LoadAsset<T>(assetname);
            }
            else
            {
                //直接加载  打包不能使用这个AssetDatabase
                string path =  assetname;
                //Debug.Log(path);
                return (T)AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }
        public void LoadAsyncAsset<T>(string assetName, LuaFunction fun) where T:UnityEngine.Object
        {
            if (AppConst.LuaBundleMode)
            {
                //还没写

            }
            else
            {
                string path = AppConst.ResPath + assetName;
                T o = (T)UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

                fun.Call();
                fun.Dispose();
            }
        }

        public GameObject MyLoadAsset(string abname, string assetname, LuaFunction func)
        {
            assetname = AppConst.ResPath + assetname;
            GameObject go = LoadAsset<GameObject>(abname, assetname);
            if (func != null)
            {
                func.Call(go);
            }
            return go;
              
        }


        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func) {
            abName = abName.ToLower();
            List<UObject> result = new List<UObject>();
            for (int i = 0; i < assetNames.Length; i++) {
                UObject go = LoadAsset<UObject>(abName, assetNames[i]);
                if (go != null) result.Add(go);
            }
            if (func != null) func.Call((object)result.ToArray());
        }

        /// <summary>
        /// 载入AssetBundle
        /// </summary>
        /// <param name="abname"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abname) {
            if (!abname.EndsWith(AppConst.ExtName)) {
                abname += AppConst.ExtName;
            }
            AssetBundle bundle = null;
            if (!bundles.ContainsKey(abname)) {
                byte[] stream = null;
                string uri = Util.DataPath + abname;
                Debug.LogWarning("LoadFile::>> " + uri);
                LoadDependencies(abname);

                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
                bundles.Add(abname, bundle);
            } else {
                bundles.TryGetValue(abname, out bundle);
            }
            return bundle;
        }

        /// <summary>
        /// 载入依赖
        /// </summary>
        /// <param name="name"></param>
        void LoadDependencies(string name) {
            if (manifest == null) {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }
            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = manifest.GetAllDependencies(name);
            if (dependencies.Length == 0) return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            for (int i = 0; i < dependencies.Length; i++) {
                LoadAssetBundle(dependencies[i]);
            }
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        string RemapVariantName(string assetBundleName) {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            // If the asset bundle doesn't have variant, simply return.
            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++) {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit) {
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
        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            if (manifest != null) manifest = null;
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}

