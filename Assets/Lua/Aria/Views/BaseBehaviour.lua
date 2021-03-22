
----基类  
  
local  BaseBehaviour =class("BaseBehaviour")

local  insert = table.insert

--注册 可以接收回调的函数
local funName = {
	"Start","FixedUpdate","Update","LateUpdate","OnDestroy"
}

function  BaseBehaviour:Ctor()
	-- body
	if self.gameObject then
		if not isNull(self.gameObject) then
			self:AddBehaviourScript(self.gameObject,{"OnDestroy"})
		end
	end
end

----  当物体被删除 由cs脚本调用
function BaseBehaviour:OnDestroy( )
 	-- body
 	eventMgr:RemoveObjAllEventListener(self)
 end 


---Update 等每帧都触发事件 注册后 接收
function  BaseBehaviour:AddBehaviourScript(obj,otherList)
	if obj then
		local  funNameList = {}
		local  funList = {}

		for k,v in pairs(otherList or funName) do
			local  fun = function()
				if self[v] then
				 return self[v](self) 
				end
			end
			if fun then
				--方法存在 注册
				insert(funNameList,v)
				insert(funList,fun)
			end
		end

		if #funNameList>0 and #funList>0 then
			LuaHelper.AddAriaLuaBehaviour(obj,funNameList,funList)
		end
	end
end


return BaseBehaviour