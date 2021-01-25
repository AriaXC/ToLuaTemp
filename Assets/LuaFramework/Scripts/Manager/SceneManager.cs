using UnityEngine;
using System.Collections;
using System;
using LuaInterface;

namespace LuaFramework
{
    public class SceneManager : Manager
    {
        void Start()
        {
            
        }

        public void ShowScene()
        {

        }
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="fun"></param>
        public void LoadScene(string sceneName, LuaFunction fun)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            if (fun != null)
                fun.Call();
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        public void LoadSceneAsync(string sceneName,LuaFunction fun)
        {

            StartCoroutine(onLoadSceneAnsyn(sceneName, fun));
        }

        IEnumerator onLoadSceneAnsyn(string sceneName, LuaFunction fun)
        {
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            //场景切换进度条 和资源卸载
            if (fun != null)
                fun.Call(op.progress);
            yield return op;

            if (fun != null)
                fun.Call();
        }

    }
}