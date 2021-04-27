
--相关触发器事件的派发
local  TriggerEvent = {}

--触发器进入
TriggerEvent.ENTER = "Events_Trigger_ENTER"
--
TriggerEvent.STAY = "Events_Trigger_STAY"
--触发器出去
TriggerEvent.EXIT = "Events_Trigger_EXIT"


function  TriggerEvent.DispatchEvent(ed,type,eventData)

	eventMgr:DispatchEventSpecial(type,ed,eventData)
end

return TriggerEvent