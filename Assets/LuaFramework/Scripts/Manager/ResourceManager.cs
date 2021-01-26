
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
        public List<string> assetName;
        //依赖列表
        public List<string> deps;
    }
    public class ResourceManager : Manager
    {
        private string[] m_Variants = { };

        //记录总的ab名字和依赖的  todo
        private AssetBundleManifest manifest;
        //private AssetBundle assetbundle;

        //已经加载过的ab包
        private Dictionary<string, AssetBundle> bundles;
        //xml文件依赖
        private Dictionary<string, AssetBundleMoonInfo> bundleInfo = new Dictionary<string, AssetBundleMoonInfo>();
        //资源依赖 assetName,bundleName
        private Dictionary<string, string> assetInfo = new Dictionary<string, string>();

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
                //添加动更目录
                AddUpdateLuaPath(Path.Combine(Application.dataPath, AppConst.AiraUpdate));
                AddUpdateResPath(Path.Combine(Application.dataPath, AppConst.AiraUpdate));

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
                info.assetName = new List<string>();
                info.deps = new List<string>();
                foreach (XmlElement child in item.ChildNodes)
                {
                    if (child.Name == "bundleName")
                    {
                        info.bundleName = child.InnerText;
                    }
                    else if (child.Name == "assetName")
                    {
                        info.assetName.Add(child.InnerText);
                        if (info.bundleName != null)
                        {
                            assetInfo.Add(child.InnerText, info.bundleName);
                        }
                        else
                        {
                            Debug.LogError("xml配置出错了");
                        }
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
        /// <summary>
        /// 加载  lua层调用这个
        /// </summary>
        /// <param name="assetname"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public GameObject LoadPrefab(string assetName, LuaFunction func)
        {
            assetName = AppConst.ResPath + assetName;
            string abName = GetAbName(assetName);
            GameObject go = LoadAsset<GameObject>(abName, assetName);
            if (func != null)
            {
                func.Call(go);
            }
            return go;
            
        }
        /// <summary>
        /// 一次加载ab包中所有的资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public UnityEngine.Object[] LoadABAllAssets(string abName, string assetName)
        {
            return null;
        }
        /// <summary>
        /// 载入素材  同步 cs端调用
        /// </summary>
        public T LoadAsset<T>(string abName, string assetName) where T : UnityEngine.Object
        {
            //editor命名空间无法打包
            if (AppConst.LuaBundleMode)
            {
                AssetBundle bundle = LoadAssetBundle(abName);
                return bundle.LoadAsset<T>(assetName);
            }
            else
            {
#if UNITY_EDITOR
                //直接加载  打包不能使用这个AssetDatabase
                string path = assetName;
                //Debug.Log(path);
                return (T)UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif
                return null;
            }
        }
        /// <summary>
        /// 异步加载场景 
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="fun"></param>
        public void LoadSceneAsync(string assetName, LuaFunction fun)
        {
            assetName = AppConst.ResPath + assetName;
            string abName = GetAbName(assetName);
            LoadAsyncAsset<GameObject>(abName, assetName, fun);
        }
        /// <summary>
        /// 载入ab  异步  cs调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="fun"></param>
        public void LoadAsyncAsset<T>(string abName, string assetName, LuaFunction fun) where T : UnityEngine.Object
        {
            if (AppConst.LuaBundleMode)
            {
                LoadAssetBundleAsync(abName, fun);
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
        /// 异步加载ab
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="fun"></param>
        public void LoadAssetBundleAsync(string abName,LuaFunction fun)
        {
            if (!bundles.ContainsKey(abName))
            {
                byte[] stream = null;

                string path = FileSearchPath.Instance.GetResPath(abName);
                Debug.Log("找到的ab路径是 === " + path);

                string uri = FileSearchPath.Instance.GetResPath(abName);
                Debug.Log("LoadFile::>> " + uri);

                stream = File.ReadAllBytes(uri);

                StartCoroutine(IELoadAsync(abName, stream, fun));
            }
            else
            {
                if (fun != null)
                {
                    fun.Call(1);
                }
            }
        }
        //加载到一半 又来了一个请求加载这个ab  需要两个队列去记录 一个是加载过的 一个是正在加载的
        // 加载到一半  此时释放 先等加载完  然后立马加入清除队列
        IEnumerator IELoadAsync(string abName,byte[] stream,LuaFunction func)
        {
            AssetBundleCreateRequest op =  AssetBundle.LoadFromMemoryAsync(stream);
            if (func != null)
            {
                func.Call(op.progress);
            }
            yield return op;
            AssetBundle bundle = op.assetBundle;
            bundles.Add(abName, bundle);

            LoadDeps(abName,true);

            if (func != null)
            {
                func.Call(op.progress);
            }
        }
        /// <summary>
        /// 载入AssetBundle
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abName)
        {
            if (!abName.EndsWith(AppConst.ExtName))
            {
                abName += AppConst.ExtName;
            }
            AssetBundle bundle = null;
            if (!bundles.ContainsKey(abName))
            {
                byte[] stream = null;
                //这里需要用我自己的路径搜
                //string uri = Util.DataPath + abname;

                string path = FileSearchPath.Instance.GetResPath(abName);
                Debug.Log("找到的ab路径是 === " + path);

                string uri = FileSearchPath.Instance.GetResPath(abName);
                Debug.Log("LoadFile::>> " + uri);
             
                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
                bundles.Add(abName, bundle);
                //载入这个ab包的依赖

                LoadDeps(abName);
            }
            else
            {
                bundles.TryGetValue(abName, out bundle);
            }
            return bundle;
        }
        /// <summary>
        /// 循环载入需要的ab依赖
        /// </summary>
        public void LoadDeps(string abName,bool isAsync =false)
        {
            AssetBundleMoonInfo info;
            if (bundleInfo.TryGetValue(abName,out info))
            {
                if (info.deps != null && info.deps.Count > 0)
                {
                    foreach (string str in info.deps)
                    {
                        if (isAsync)
                        {
                            LoadDeps(str,true);
                            LoadAssetBundleAsync(str,null);
                        }
                        else
                        {
                            LoadDeps(str);
                            LoadAssetBundle(str);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取ab名字
        /// </summary>
        /// <param name="assetName"></param>
        public string GetAbName(string assetName)
        {
            if (AppConst.LuaBundleMode)
            {
                assetName = assetName.ToLower();
                if (assetInfo.Count > 0 && assetInfo != null)
                {
                    string value;
                    if (assetInfo.TryGetValue(assetName, out value))
                    {
                        Debug.Log("从ab中查找到的ab名字 === " + value);
                        return value;
                    }
                }
                Debug.LogError("ab包中没有这个资源  ==" + assetName);
            }
            return null;
        }
        /// <summary>
        /// 场景变换调用  清除ab
        /// </summary>
        public void  ClearAll()
        {
            if (bundles != null)
            {
                //场景加完直接 是true 会有材质丢失
                foreach (var item in bundles)
                { 
                    Debug.Log("卸载的ab包资源  == "+item.Key);
                    item.Value.Unload(false);
                }
                Resources.UnloadUnusedAssets();
            }
            bundles = new Dictionary<string, AssetBundle>();
            Debug.Log("场景切换完  我加载的ab的个数== " + bundles.Count);
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
