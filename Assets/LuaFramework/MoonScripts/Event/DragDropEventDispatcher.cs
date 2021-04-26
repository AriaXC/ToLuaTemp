using UnityEngine;
using System.Collections;
using LuaInterface;
using UnityEngine.EventSystems;
using LuaFramework;

namespace MoonScrpts
{
    //拖拽相关的
    public class DragDropEventDispatcher : MonoBehaviour, IBeginDragHandler,IDragHandler,IDropHandler,IEndDragHandler, IInitializePotentialDragHandler
    {
        private const string EVENT_BEGIN_DRAG = "Events_DragDrop_BeginDrag";
        private const string EVENT_DRAG = "Events_DragDrop_Drag";
        private const string EVENT_END_DRAG = "Events_DragDrop_EndDrag";
        private const string EVENT_INITIALIZE_POTENTIAL_DRAG = "Events_DragDrop_InitializePotentialDrag";
        private const string EVENT_DROP = "Events_DragDrop_Drop";

        private static LuaFunction s_dispatchEvent;

        //指定的lua caller
        public LuaTable ed;

        private static void DispatchEvent(LuaTable ed, string type, PointerEventData eventData)
        {
            LuaManager luaMgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
            if (ed == null)
                return;

            if (s_dispatchEvent == null)
                s_dispatchEvent = luaMgr.GetLuaState().GetFunction("DragEvent.DispatchEvent");

            s_dispatchEvent.BeginPCall();
            s_dispatchEvent.Push(ed);
            s_dispatchEvent.Push(type);
            s_dispatchEvent.Push(eventData);
            s_dispatchEvent.PCall();
            s_dispatchEvent.EndPCall();
        }



        public void OnBeginDrag(PointerEventData eventData)
        {
            DispatchEvent(ed, EVENT_BEGIN_DRAG, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DispatchEvent(ed, EVENT_DRAG, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DispatchEvent(ed, EVENT_END_DRAG, eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            DispatchEvent(ed, EVENT_INITIALIZE_POTENTIAL_DRAG, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            DispatchEvent(ed, EVENT_DROP, eventData);
        }
    }
}
