 
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
	caller = caller or EventStr

	if self._listeners[eventName] == nil then
		self._listeners[eventName] = {}
	end

	---  info  应该考虑是不是做成缓存池
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
	if caller then
		if caller._addEventListeners == nil then
			caller._addEventListeners ={}
		end
		caller._addEventListeners[eventName] = handStr
	end

	if target then
		if eventName == DragEvent.BEGIN_DRAG or eventName == DragEvent.DRAG 
		  or eventName == DragEvent.END_DRAG  or eventName == DragEvent.INITIALIZE_POTENTIAL_DRAG
		  or eventName == DragEvent.DROP then
		  	--
		  	LuaHelper.AddDragDropEvent(target.gameObject,caller)
		elseif eventName == TriggerEvent.ENTER or eventName == TriggerEvent.STAY
		  or eventName == TriggerEvent.EXIT  then
		  	
		  	LuaHelper.AddTriggerEvent(target.gameObject,caller)
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

	for k,v in pairs(self._listeners[eventName]) do
		if v.callback == callback and v.caller == caller then
			log(string.format("RemoveEventListener ====   eventName==%s",eventName))
			self._listeners[eventName][k] = nil
		end
	end
end
function  EventManager:RemoveObjAllEventListener(caller)
	caller = caller or EventStr
	if caller == nil then
		logError("这个已经是空了")
		return
	end
	if caller._addEventListeners then
		for k,v in pairs(caller._addEventListeners) do
			if self._listeners[k] then
				log(string.format("RemoveObjAllEventListener ===  eventName =%s , handStr = %s",k,v))			
				self._listeners[k][v] = nil
				caller._addEventListeners=nil
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
	if self._listeners[eventName] == nil then
		-- log("事件没有被监听啊   "..eventName)
		return
	end

	for k,v in pairs(self._listeners[eventName]) do
		if v then
			-- xpcall(function ()
				if v.caller == nil then
					v.callback(args)
				else
					v.callback(v.caller,args)
				end
			-- end,_Aira_Error_Fun)
		else
			log("事件的回调没有啊   "..eventName)
		end
	end
end

---??????  这里应该改一下子  但是我不知道怎么改 好
--特殊指定的派发
function  EventManager:DispatchEventSpecial(eventName,caller,... )
	if eventName == nil then
		logError ("eventName 是空的")
		return 
	end
	local  args = ...
	if self._listeners[eventName] == nil then
		-- log("事件没有被监听啊   "..eventName)
		return
	end
	for k,v in pairs(self._listeners[eventName]) do
		if v then
			if caller == v.caller then
				-- xpcall(function ()
				if v.caller == nil then
					v.callback(args)
				else
					v.callback(v.caller,args)
				end
				-- end,_Aira_Error_Fun)
			else
				logError("Drag的 指定派发成功了 不是报错")
			end
		else
			log("事件的回调没有啊   "..eventName)
		end
	end
end

return EventManager



