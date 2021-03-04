
View=class("View")

--还没有些异步加载
function View:Ctor(prefab,parent,isAsync)
	-- body
	-- View.super.Ctor(self)
	
	if prefab == nil then
		return
	end

	local  parent = parent

	if type(parent) == "string" then
		parent = MySceneMgr._layer[parent]
	end

	if not parent then
		if MySceneMgr._layer ~= nil then
			parent = MySceneMgr._layer[GameConst.Layer.window]
		end
	end

	if parent == nil then
		logError("不应该为空的 有错误")
	end

	self.gameObject=Instantiate(prefab,parent)

	log("ViewName == "..self.gameObject.name)

	self:OnInitialize()
end
function View:OnInitialize( ... )
	-- body
	self.transform=self.gameObject.transform

end

function  View:Show( ... )
	-- body
	self.gameObject:SetActive(true)
end

function   View:Hide( ... )
	-- body
	self.gameObject:SetActive(false)
end



return View
