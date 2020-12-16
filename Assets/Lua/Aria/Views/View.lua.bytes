
View=class("View")

--还没有些异步加载
function View:Ctor(prefab,parent)
	-- body
	-- View.super.Ctor(self)
	if prefab == nil then
		return
	end

	if parent == nil then
		local go = GameObject.Find("window")
		parent = go.transform
	end
	self.gameObject=Instantiate(prefab,parent)
	
	self:OnInitialize()
end
function View:OnInitialize( ... )
	-- body
	self.transform=self.gameObject.transform

end

return View
