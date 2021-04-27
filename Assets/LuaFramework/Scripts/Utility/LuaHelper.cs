using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using LuaInterface;
using UnityEngine.UI;
using System;
using MoonScrpts;

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

            if (go.GetComponent<Button>() == null)
                go.AddComponent<Button>();

            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.GetComponent<Button>().onClick.AddListener(
                delegate (){
                    func.Call(go);
                });
        }
        public static void DelClick(GameObject go)
        {
            if (go == null)
            {
                return;
            }
            if (go.GetComponent<Button>() == null)
            {
                return;
            }

            go.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        /// <summary>
        /// 添加c#脚本  执行update等的回调
        /// </summary>
        /// <param name="go"></param>
        /// <param name="funNameList"></param>
        /// <param name="funList"></param>
        public static void AddAriaLuaBehaviour(GameObject go,string[] funNameList,LuaFunction[] funList)
        {
            if (go == null)
            {
                return;
            }
            if (funNameList.Length <= 0 && funList.Length <= 0)
            {
                Debug.Log("传入的方法长度不对   =="+ funNameList.Length);
                return;
            }
             AriaLuaBehaviour aira = go.GetComponent<AriaLuaBehaviour>();
            if (aira == null)
            {
                aira = go.AddComponent<AriaLuaBehaviour>();
            }
            else
            {
                //Debug.Log("添加了 已经有了这个脚本了  " + go.name);
            }
            aira.SetMess(funNameList, funList);
        }

        /// <summary>
        /// 添加拖拽脚本
        /// </summary>
        public static void AddDragDropEvent(GameObject go,LuaTable ed)
        {
            DragDropEventDispatcher dispatcher = go.GetComponent<DragDropEventDispatcher>();
            if (dispatcher == null)
                dispatcher = go.AddComponent<DragDropEventDispatcher>();
            dispatcher.ed = ed;
        }
        /// <summary>
        /// 添加触发器脚本
        /// </summary>
        public static void AddTriggerEvent(GameObject go,LuaTable ed)
        {
            TriggerEventDispatcher dispatcher = go.GetComponent<TriggerEventDispatcher>();
            if (dispatcher == null)
                dispatcher = go.AddComponent<TriggerEventDispatcher>();
            dispatcher.ed = ed;
        }
        /// <summary>
        /// 添加触发器脚本
        /// </summary>
        public static void AddCollisionEvent(GameObject go, LuaTable ed)
        {
            CollisionEventDispatcher dispatcher = go.GetComponent<CollisionEventDispatcher>();
            if (dispatcher == null)
                dispatcher = go.AddComponent<CollisionEventDispatcher>();
            dispatcher.ed = ed;
        }
        /// <summary>
        /// 判断传入对象是不是空
        /// </summary>
        /// <param name="go"></param>
        public static bool ObjIsNull(GameObject go)
        {
            if (go == null || go.Equals(null))
                return true;
            return false;

        }
    }
}