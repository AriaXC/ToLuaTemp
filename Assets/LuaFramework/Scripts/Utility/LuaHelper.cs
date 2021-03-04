﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using LuaInterface;
using UnityEngine.UI;
using System;

namespace LuaFramework {
    public static class LuaHelper {

        /// <summary>
        /// getType
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static System.Type GetType(string classname) {
            Assembly assb = Assembly.GetExecutingAssembly();  //.GetExecutingAssembly();
            System.Type t = null;
            t = assb.GetType(classname); ;
            if (t == null) {
                t = assb.GetType(classname);
            }
            return t;
        }

        /// <summary>
        /// 场景管理器
        /// </summary>
        /// <returns></returns>
        public static SceneManager GetSceneManager() {
            return AppFacade.Instance.GetManager<SceneManager>(ManagerName.Scene);
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static ResourceManager GetResManager() {
            return AppFacade.Instance.GetManager<ResourceManager>(ManagerName.Resource);
        }

        /// <summary>
        /// 网络管理器
        /// </summary>
        public static NetworkManager GetNetManager() {
            return AppFacade.Instance.GetManager<NetworkManager>(ManagerName.Network);
        }

        /// <summary>
        /// 音乐管理器
        /// </summary>
        public static SoundManager GetSoundManager() {
            return AppFacade.Instance.GetManager<SoundManager>(ManagerName.Sound);
        }


        /// <summary>
        /// pbc/pblua函数回调
        /// </summary>
        /// <param name="func"></param>
        public static void OnCallLuaFunc(LuaByteBuffer data, LuaFunction func) {
            if (func != null) func.Call(data);
            Debug.LogWarning("OnCallLuaFunc length:>>" + data.buffer.Length);
        }

        /// <summary>
        /// cjson函数回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="func"></param>
        public static void OnJsonCallFunc(string data, LuaFunction func) {
            Debug.LogWarning("OnJsonCallback data:>>" + data + " lenght:>>" + data.Length);
            if (func != null) func.Call(data);
        }


        //图层也要改了
        public static void SetParent(Transform target, Transform parent,bool isT)
        {
            target.transform.SetParent(parent, isT);
          
            foreach (Transform value in target.GetComponentInChildren<Transform>())
            {
                value.gameObject.layer = parent.gameObject.layer;
            }
        }
       
        //给按钮添加点击事件
        public static void AddClick(GameObject go,LuaFunction func)
        {
            if (go == null || func == null)
            {
                return;
            }

            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(
                delegate (){
                    func.Call(go);
                });
        }
    }
}