local AStarView=class(AStarView,View)

function  AStarView:Ctor( )
	-- body
	AStarView.super.Ctor(self,"Prefabs/TestModel/AStar/AView.prefab",GameConst.Layer.window)

	self.mapG = self.transform:Find("bg/map").gameObject

	self.colorList = {}
	self.colorList.green = Vector3.New(7,221,12)
	self.colorList.red = Vector3.New(221,12,7)
	self.colorList.yellow = Vector3.New(226,231,19)
	
end

function  AStarView:OnInitialize( ... )
	-- body
	AStarView.super.OnInitialize(self)	

	AddBtnClick(self.transform:Find("bg/back").gameObject,function( ... )
		self:Hide()
	end)

	AddBtnClick(self.transform:Find("bg/btnAddMap").gameObject,function( ... )
		self:CreateMap()
	end)
end

function  AStarView:CreateMap()
	
end

return AStarView