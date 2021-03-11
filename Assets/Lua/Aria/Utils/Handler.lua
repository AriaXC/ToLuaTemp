--回调函数

local  Handler = class("Handler")

function  Handler:Ctor(callback,caller,once)
	-- body

	self:SetImg(callback,caller,once)
end

--设置属性 
--once 默认是true
function  Handler:SetImg(callback,caller,once)
	self.callback = callback 
	self.caller = caller
	self.once =once 
end

--执行回调
function  Handler:Execute( ... )
	local  callback = self.callback
	local  caller = self.caller
	local  args = ...

	if self.once then
		self:Recycle()
	end

	if callback then
		if caller then
			callback(caller,args)
		else
			callback(args)
		end
	end
end

--回收池
function  Handler:Recycle( )
	-- body
	self:Clear()
	Handler._pool[#Handler._pool] = self
end

--清除引用
function  Handler:Clear( )
	-- body
	self.callback = nil
	self.caller = nil
	self.once =nil
end

-----对象池
Handler._pool = {}

--从池子里获取一个 
function  Handler.GetItem(callback,caller,once)
	local  hand = nil
	if #Handler._pool >0 then
		hand = table.remove(Handler._pool)
		hand:SetImg(callback,caller,once)
	else
		hand =  Handler.New(callback,caller,once)
	end
	
	return hand
end

return Handler