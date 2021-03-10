 
local  EventManager = class("EventManager")

function EventManager:Ctor()

	log("初始化 EventManager")
	--列表
	self._listeners = {}
	--index
	self._handlerIndex = 0

	--obj 上绑定的事件
	self._objHandler = {}
end

--添加事件
function EventManager:AddEventListener(eventName,listener,target)
	if type(eventName) ~= "string" then
		logError("eventName 的类型错了")
		return
	end
	eventName = string.lower(eventName)
	self._handlerIndex = self._handlerIndex +1
	local  handStr  = string.format("Hand_%s",self._handlerIndex)
	
	self._listeners[eventName][handStr] = listener

	if target then
		if target._addEventListeners == nil then
			target._addEventListeners ={}
		end

		----
		--  同一个target 默认只能绑定一个同名的eventName  否则清除target有问题 
		----
		target._addEventListeners[eventName]= handStr

	end

	return handStr
end
--移除
function  EventManager:RemoveEventListener(eventName,key)
	if type(eventName) ~= "string" then
		logError("eventName 的类型错了")
		return
	end
	if type(key) ~= "string" then
		logError("key 的类型错了")
		return
	end

	for k,v in pairs(self._listeners[eventName]) do
		if key == self._listeners[eventName][k] then
			log(string.format("RemoveEventListener ====   eventName==%s",eventName))
			self._listeners[eventName][key] = nil
		end
	end
end
function  EventManager:RemoveObjAllEventListener(target)
	if target == nil then
		logError("这个已经是空了")
		return
	end
	if target._addEventListeners then
		for k,v in pairs(target._addEventListeners) do
			if self._listeners[k] then
				log(string.format("RemoveObjAllEventListener ===  eventName =%s , handStr = %s",k,v))			
				self._listeners[k][v] = nil
			end
		end
	end
end
--派发
function  EventManager:DispatchEvent( eventName,... )
	

	
end

return EventManager