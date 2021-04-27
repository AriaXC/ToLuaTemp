
--相关触发器事件的派发
local  TriggerEvent = {}

--开始拖动
TriggerEvent.ENTER = "Events_ENTER"
--拖动中
TriggerEvent.STAY = "Events_STAY"
--拖动结束
TriggerEvent.EXIT = "Events_EXIT"


function  TriggerEvent.DispatchEvent(ed,type,eventData)

	eventMgr:DispatchEventSpecial(type,ed,eventData)
end

return TriggerEvent