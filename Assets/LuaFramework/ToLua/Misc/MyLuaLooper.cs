using UnityEngine;
using System;
using LuaInterface;
using MoonScrpts;

public class MyLuaLooper : MonoBehaviour
{
    private LuaFunction m_dispatchEvent;

    public LuaState luaState = null;


    private string FIXED_UPDATE = "Events_FixedUpdate";
    private string UPDATE = "Events_Update";
    private string LATE_UPDATE = "Events_LateUpdate";


    private void Start()
    {
        m_dispatchEvent = luaState.GetFunction("UpdateEvent.DispatchEvent");
    }
    /// <summary>
    /// 给lua发事件
    /// </summary>
    /// <param name="eventName"></param>
    private void DispatchLuaEvent(string eventName)
    {
        if (m_dispatchEvent == null) return;

        m_dispatchEvent.BeginPCall();
        m_dispatchEvent.Push(eventName);

        //传入的是  当前程序运行的时间  暂时没有
        m_dispatchEvent.Push(TimeUtil.timeSec);
        m_dispatchEvent.PCall();
        m_dispatchEvent.EndPCall();
    }

    private void FixedUpdate()
    {
        TimeUtil.Update();
        DispatchLuaEvent(FIXED_UPDATE);
    }

    private void Update()
    {
        // 设备方向 尺寸等发现变化 应该这里通知  未
        TimeUtil.Update();
        DispatchLuaEvent(UPDATE);
    }

    private void LateUpdate()
    {
        TimeUtil.Update();
        DispatchLuaEvent(LATE_UPDATE);
    }

    public void Destroy()
    {
        if (luaState != null)
        {
            luaState = null;
        }
    }

    void OnDestroy()
    {
        if (luaState != null)
        {
            Destroy();
        }
    }
}


