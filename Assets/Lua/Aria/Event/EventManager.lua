 
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

---------需要继续的优化
--添加事件
function EventManager:AddEventListener(eventName,callback,caller,target)
	if type(eventName) ~= "string" then
		logError("eventName 的类型错了")
		return
	end
	target = target or EventStr
	eventName = string.lower(eventName)

	if self._listeners[eventName] == nil then
		self._listeners[eventName] = {}
	end

	local info 
	for k,v in pairs(self._listeners[eventName]) do
		info = v
		if info.callback == callback and info.caller ==caller then
			log("已经注册了  = "..eventName)
			return
		end

	end
	self._handlerIndex = self._handlerIndex +1
	local  handStr  = string.format("Hand_%s",self._handlerIndex)

	info = {eventName = eventName,callback = callback ,caller =caller}
	self._listeners[eventName][handStr] = info

	--可以到时候一下子全清
	if target then
		if target._addEventListeners == nil then
			target._addEventListeners ={}
		end
		target._addEventListeners[eventName] = handStr
	end

	if target.gameObject then
		--碰撞器 触发器等的添加
		if eventName == "OnCollisionEnter"  then
		end

	end

	-----OnDestroy  
	return handStr
end
--移除
function  EventManager:RemoveEventListener(eventName,callback,caller)
	if type(eventName) ~= "string" then
		logError("eventName 的类型错了  ")
		return
	end

	eventName = string.lower(eventName)

	for k,v in pairs(self._listeners[eventName]) do
		if v.callback == callback and v.caller == caller then
			log(string.format("RemoveEventListener ====   eventName==%s",eventName))
			self._listeners[eventName][k] = nil
		end
	end
end
function  EventManager:RemoveObjAllEventListener(target)
	target = target or EventStr
	if target == nil then
		logError("这个已经是空了")
		return
	end
	if target._addEventListeners then
		for k,v in pairs(target._addEventListeners) do
			if self._listeners[k] then
				log(string.format("RemoveObjAllEventListener ===  eventName =%s , handStr = %s",k,v))			
				self._listeners[k][v] = nil
				target._addEventListeners=nil
			end
		end
	end
end

--派发
function  EventManager:DispatchEvent(eventName,... )
	if eventName == nil then
		logError ("eventName 是空的")
		return 
	end
	local  args = ...
	local  eventName  = string.lower(eventName)
	if self._listeners[eventName] == nil then
		-- log("事件没有被监听啊   "..eventName)
		return
	end

	for k,v in pairs(self._listeners[eventName]) do
		if v then
			xpcall(function ()
				if v.caller == nil then
					v.callback(args)
				else
					v.callback(v.caller,args)
				end
			end,_Aira_Error_Fun)
		else
			log("事件的回调没有啊   "..eventName)
		end
	end

end


return EventManager



