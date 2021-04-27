--相关触发器事件的派发
local  CollisionEvent = {}


CollisionEvent.ENTER = "Events_Collision_ENTER"

CollisionEvent.STAY = "Events_Collision_STAY"

CollisionEvent.EXIT = "Events_Collision_EXIT"


function  CollisionEvent.DispatchEvent(ed,type,eventData)

	eventMgr:DispatchEventSpecial(type,ed,eventData)
end

return CollisionEvent