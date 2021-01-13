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
        /// 异步加载场景
        /// </summary>
        public void LoadSceneAsync(string sceneName,LuaFunction fun)
        {

            StartCoroutine(onLoadSceneAnsyn(sceneName, fun));
        }

        IEnumerator onLoadSceneAnsyn(string sceneName, LuaFunction fun)
        {
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            yield return op;

            if (fun != null)
                fun.Call();
        }

    }
}