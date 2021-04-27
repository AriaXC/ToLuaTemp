using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;
using LuaInterface;

public class CollisionEventDispatcher : MonoBehaviour
{
    private const string EVENT_ENTER = "Events_Collision_ENTER";
    private const string EVENT_STAY = "Events_Collision_STAY";
    private const string EVENT_EXIT = "Events_Collision_EXIT";

    private static LuaFunction s_dispatchEvent;

    //指定的lua caller
    public LuaTable ed;

    private static void DispatchEvent(LuaTable ed, string type, Collision collision)
    {
        LuaManager luaMgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
        if (ed == null)
            return;

        if (s_dispatchEvent == null)
            s_dispatchEvent = luaMgr.GetLuaState().GetFunction("CollisionEvent.DispatchEvent");

        s_dispatchEvent.BeginPCall();
        s_dispatchEvent.Push(ed);
        s_dispatchEvent.Push(type);
        s_dispatchEvent.Push(collision);
        s_dispatchEvent.PCall();
        s_dispatchEvent.EndPCall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        DispatchEvent(ed, EVENT_ENTER, collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        DispatchEvent(ed, EVENT_STAY, collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        DispatchEvent(ed, EVENT_EXIT, collision);
    }
}
