local  UpdateEvent = {}


function  UpdateEvent.DispatchEvent(type,time)
	
	eventMgr:DispatchEvent(type)
end

return UpdateEvent