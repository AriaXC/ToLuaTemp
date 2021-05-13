local AStarView=class(AStarView,View)

function  AStarView:Ctor( )
	-- body
	AStarView.super.Ctor(self,"Prefabs/TestModel/AStar/AView.prefab",GameConst.Layer.window)

	self.mapG = self.transform:Find("map").gameObject
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