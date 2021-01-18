
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaFramework;
using LuaInterface;
using UObject = UnityEngine.Object;
using System;
using System.Xml;

namespace LuaFramework
{
    public class AssetBundleMoonInfo
    {
        //ab包名字
        public string bundleName;
        //文件名字
        public List<string> assertName;
        //依赖列表
        public List<string> deps;
    }
    public class ResourceManager : Manager
    {
        private string[] m_Variants = { };

        //记录总的ab名字和依赖的  todo
        private AssetBundleManifest manifest;
        private AssetBundle assetbundle;

        //已经加载过的ab包
        private Dictionary<string, AssetBundle> bundles;
        //xml文件依赖
        private Dictionary<string, AssetBundleMoonInfo> bundleInfo = new Dictionary<string, AssetBundleMoonInfo>();

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
            //加载映射的xml文件
            string xmlPath = FileSearchPath.Instance.GetResPath(AppConst.MoonXml);
            Debug.Log("Xml=== "+xmlPath);
            if (xmlPath == null)
            {
                Debug.LogError("xml没找到");
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.SelectSingleNode("bundles").ChildNodes;
            AssetBundleMoonInfo info = new AssetBundleMoonInfo();
            //遍历每一个节点，拿节点的属性以及节点的内容
            foreach (XmlElement item in nodeList)
            {
                info = new AssetBundleMoonInfo();
                info.assertName = new List<string>();
                info.deps = new List<string>();
                foreach (XmlElement child in item.ChildNodes)
                {
                    if (child.Name == "bundleName")
                    {
                        info.bundleName = child.InnerText;
                    }
                    else if (child.Name == "AssertName")
                    {
                        info.assertName.Add(child.InnerText);
                    }
                    else if (child.Name == "deps")
                    {
                        info.deps.Add(child.InnerText);
                    }
                }
                bundleInfo.Add(info.bundleName, info);
            }

            // 图片等ab包的时候 资源和ab名字依赖
            //string str = FileSearchPath.Instance.GetResPath("StreamingAssets");
            //assetbundle = AssetBundle.LoadFromMemory(File.ReadAllBytes(str));
            //manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            //string[] allInfo = manifest.GetAllAssetBundles();
            //for (int i = 0; i < allInfo.Length; ++i)
            //{
            //    Debug.LogError("Info =   " + allInfo[i]);
            //    string[] allDep = manifest.GetAllDependencies(allInfo[i]);
            //    for (int j = 0; j < allDep.Length; j++)
            //    {
            //        Debug.LogError("allDep =   " + allDep[j]);
            //    }
            //}
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
        /// 载入素材  同步
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
        /// <summary>
        /// 载入ab  异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="fun"></param>
        public void LoadAsyncAsset<T>(string assetName, LuaFunction fun) where T : UnityEngine.Object
        {
            if (AppConst.LuaBundleMode)
            {
              

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
                Debug.Log("LoadFile::>> " + uri);
             
                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
                bundles.Add(abname, bundle);
                //载入这个ab包的依赖


            }
            else
            {
                bundles.TryGetValue(abname, out bundle);
            }
            return bundle;
        }
        public void AddBundleDep(string abName)
        {

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
