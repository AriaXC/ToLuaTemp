using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;
using LuaInterface;

public class TriggerEventDispatcher : MonoBehaviour
{
    private const string EVENT_ENTER = "Events_ENTER";
    private const string EVENT_STAY = "Events_STAY";
    private const string EVENT_EXIT = "Events_EXIT";
    
    private static LuaFunction s_dispatchEvent;

    //指定的lua caller
    public LuaTable ed;

    private static void DispatchEvent(LuaTable ed, string type, Collider other)
    {
        LuaManager luaMgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
        if (ed == null)
            return;

        if (s_dispatchEvent == null)
            s_dispatchEvent = luaMgr.GetLuaState().GetFunction("TriggerEvent.DispatchEvent");

        s_dispatchEvent.BeginPCall();
        s_dispatchEvent.Push(ed);
        s_dispatchEvent.Push(type);
        s_dispatchEvent.Push(other);
        s_dispatchEvent.PCall();
        s_dispatchEvent.EndPCall();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("===");
        DispatchEvent(ed, EVENT_ENTER, other);
    }
    private void OnTriggerStay(Collider other)
    {
        DispatchEvent(ed, EVENT_STAY, other);
    }
    private void OnTriggerExit(Collider other)
    {
        DispatchEvent(ed, EVENT_EXIT, other);
    }
}
