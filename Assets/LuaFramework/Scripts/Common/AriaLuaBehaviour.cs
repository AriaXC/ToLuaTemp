using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace LuaFramework {
    public class AriaLuaBehaviour : MonoBehaviour
    {
        private Dictionary<string, LuaFunction> luafunDic = new Dictionary<string, LuaFunction>();


        public void SetMess(string[] funNameList,LuaFunction[] funList)
        {
            luafunDic.Clear();
            for (int i = 0; i < funNameList.Length; ++i)
            {
                luafunDic.Add(funNameList[i], funList[i]);
            }
        }


        private void Start()
        {
            if (luafunDic.ContainsKey("Start"))
            {
                if (luafunDic["Start"].GetLuaState() == null)
                    return;
                luafunDic["Start"].Call();
            }
        }
        private void FixedUpdate()
        {
            if (luafunDic.ContainsKey("FixedUpdate"))
            {
                if (luafunDic["FixedUpdate"].GetLuaState() == null)
                    return;
                luafunDic["FixedUpdate"].Call();
            }
        }
        private void Update()
        {
            if (luafunDic.ContainsKey("Update"))
            {
                if (luafunDic["Update"].GetLuaState() == null)
                    return;
                luafunDic["Update"].Call();
            }
        }
        private void LateUpdate()
        {
            if (luafunDic.ContainsKey("LateUpdate"))
            {
                if (luafunDic["LateUpdate"].GetLuaState() == null)
                    return;
                luafunDic["LateUpdate"].Call();
            }
        }
        private void OnDestroy()
        {
            if (luafunDic.ContainsKey("OnDestroy"))
            {
                if (luafunDic["OnDestroy"].GetLuaState() == null)
                    return;
                luafunDic["OnDestroy"].Call();
            }
        }
    }
}

