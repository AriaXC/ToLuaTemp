--回调函数

local  Handler = class("Handler")

function  Handler.Ctor(callback,caller)
	-- body
end

--设置属性
function  Handler:SetImg(callback,caller)
	-- body
end

--执行回调
function  Handler:Execute( ... )
	-- body
end

--回收池
function  Handler:Recycle( ... )
	-- body
end

--清除引用
function  Handler:Clear( ... )
	-- body
end

-----对象池
Handler._pool = {}

--从池子里获取一个
function  Handler:GetItem()
	-- body
end

renturn Handler