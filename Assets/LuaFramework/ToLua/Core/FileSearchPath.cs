using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using LuaFramework;

namespace LuaInterface
{
    /// <summary>
    /// 设置你的文件和lua的搜索路径
    /// </summary>
    public class FileSearchPath
    {
        public static FileSearchPath Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileSearchPath();
                }

                return instance;
            }

            protected set
            {
                instance = value;
            }
        }
        //
        protected static FileSearchPath instance = null;

        //文件的搜索路径 先后顺序
        protected List<string> resSearchPath = new List<string>();
        //lua代码的搜索路径 先后顺序
        protected List<string> luaSearchPath = new List<string>();

        //找到的文件缓存
        protected Dictionary<string, string> resCaChe = new Dictionary<string, string>();
        //找到的lua缓存
        protected Dictionary<string, string> luaCaChe = new Dictionary<string, string>();

        //lua的ab包只有一个
        AssetBundle luaBundle;

        /// <summary>
        /// 添加文件搜索路径
        /// </summary>
        public void  AddResSearchPath(string path,bool isFirst =false)
        {
            Debug.Log("AddResSearchPath  == "+path);
            int index = resSearchPath.IndexOf(path);
            if (index > 0)
                resSearchPath.RemoveAt(index);
            if (isFirst)
            {
                resSearchPath.Insert(0, path);
            }
            else {
                resSearchPath.Add(path);
            }
            ResCaCheClear();
        }
        /// <summary>
        /// 添加lua的搜索路径
        /// </summary>
        public void AddLuaSearchPath(string path, bool isFirst = false)
        {
            Debug.Log("AddLuaSearchPath  == " + path);
            int index = luaSearchPath.IndexOf(path);
            if (index > 0)
                luaSearchPath.RemoveAt(index);
            if (isFirst)
            {
                luaSearchPath.Insert(0, path);
            }
            else
            {
                luaSearchPath.Add(path);
            }
            LuaCaCheClear();
        }
        /// <summary>
        /// 获取lua的文件地址
        /// </summary>
        /// <returns></returns>
        public string GetLuaPath(string fileName)
        {
            string cache = null;
            //out 不用初始化
            luaCaChe.TryGetValue(fileName, out cache);
            if (cache != null)
                return cache;
            if (!fileName.EndsWith(".lua"))
                fileName = fileName + ".lua";
            for (int i = 0; i < luaSearchPath.Count; i++)
            {
                string path = Path.Combine(luaSearchPath[i], fileName);
                if (IsFileExist(path))
                {
                    luaCaChe.Add(fileName, path);
                    return path;
                }
            }
            Debug.LogError(fileName + "   ,Lua文件没有找到");
            return null;
        }
        /// <summary>
        /// 卸载
        /// </summary>
        public void ClearLuaBundle()
        {
            if (luaBundle != null)
            {
                luaBundle.Unload(false);
                //Resources.UnloadAsset(luaBundle);
            }
        }
        /// <summary>
        /// 加载lua的ab包
        /// </summary>
        /// <param name="abName"></param>
        public void AddLuaBundle(string abName)
        {
            CString sb = CString.Alloc(256);
            sb = "lua";
            sb = sb.Append(AppConst.ExtName);
            sb = sb.ToLower();

            string path = GetResPath(sb.ToString());
            if (!File.Exists(path))
            {
                Debug.LogError("找不到luaAb");
                return;
            }
            Debug.Log("lua ab包加载完成");
            byte[] stream = null;

            stream = File.ReadAllBytes(path);
            // LoadFromFile 的话是加载一个路径
            luaBundle = AssetBundle.LoadFromMemory(stream);
        }
       /// <summary>
       /// 从ab中加载lua文件  lua的ab包只有一个
       /// </summary>
       /// <returns></returns>
        public byte[] GetLuaZip(string fileName)
        {
            //如果是ab包模式 那我所有的lua代码全部都读动更路径上面的
            // 目前这样写不太好 暂时先实现lua ab包版本

            if (!fileName.EndsWith(".lua"))
                fileName = fileName + ".lua";

            fileName += ".bytes";
            fileName = "Assets/LuaABTemp/" + fileName;
            byte[] buffer = null;

            if (luaBundle!=null)
            {
                TextAsset luaCode = luaBundle.LoadAsset<TextAsset>(fileName);
                if (luaCode != null)
                {
                    buffer = luaCode.bytes;
                    Resources.UnloadAsset(luaCode);
                }
                return buffer;
            }
            Debug.LogError(fileName + "   ,Lua文件没有读取成功，lua的ab包不存在，没有加载成功");
            return null;
        }
        /// <summary>
        /// 获取res的文件地址  没有做文件的子文件路径也搜索
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetResPath(string fileName)
        {
            string cache = null;
            resCaChe.TryGetValue(fileName, out cache);
            if (cache != null)
                return cache;

            for (int i = 0; i < resSearchPath.Count ; i++)
            {
                string path = Path.Combine(resSearchPath[i], fileName);
                //Debug.LogError(fileName+"    ????   "+path);
                if (IsFileExist(path))
                {
                    resCaChe.Add(fileName, path);
                    return path;
                }
            }
            Debug.LogError(fileName + "   ,Res文件没有找到");
            return null;
        }
        /// <summary>
        /// 获得lua文本字节流
        /// </summary>
        /// <returns></returns>
        public byte[] ReadFile(string fileName)
        {
            //fileName  是require传过来的字符串
            if (AppConst.LuaBundleMode)
            {
                return GetLuaZip(fileName);
            }
            else {
                string path = GetLuaPath(fileName);
                if (path == null)
                    return null;

                return File.ReadAllBytes(path);
            }
        }
        /// <summary>
        /// 获取text文件
        /// </summary>
        /// <returns></returns>
        public string ReadText(string fileName)
        {
            string path = GetResPath(fileName);
            if (path == null)
                return null;

            return File.ReadAllText(path);
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsFileExist(string path)
        {
            try {
              return File.Exists(path);
            }
            catch {
                return false;
            }

        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string FindFileError(string fileName)
        {
            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }

            using (CString.Block())
            {
                CString sb = CString.Alloc(512);

                for (int i = 0; i < luaSearchPath.Count; i++)
                {
                    sb.Append("\n\tMoon no file '").Append(luaSearchPath[i]).Append('\'');
                }

                sb = sb.Replace("?", fileName);

                if (AppConst.LuaBundleMode)
                {
                    int pos = fileName.LastIndexOf('/');

                    if (pos > 0)
                    {
                        int tmp = pos + 1;
                        sb.Append("\n\tMoon no file '").Append(fileName, tmp, fileName.Length - tmp).Append(".lua' in ").Append("lua_");
                        tmp = sb.Length;
                        sb.Append(fileName, 0, pos).Replace('/', '_', tmp, pos).Append(".unity3d");
                    }
                    else
                    {
                        sb.Append("\n\tMoon no file '").Append(fileName).Append(" in ").Append("lua.unity3d");
                    }
                }

                return sb.ToString();
            }
        }
        /// <summary>
        /// lua缓存清除
        /// </summary>
        public void LuaCaCheClear()
        {
            luaCaChe.Clear();
        }
        /// <summary>
        /// res缓存清除
        /// </summary>
        public void ResCaCheClear()
        {
            resCaChe.Clear();
        }
        public void Dispose()
        {
            if (instance != null)
            {
                instance = null;
                luaSearchPath.Clear();
                resSearchPath.Clear();
                ClearLuaBundle();
            }
        }
    }
}

