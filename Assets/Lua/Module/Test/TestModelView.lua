

local TestModelView=class("TestModelView",View)


function  TestModelView:Ctor( )
	TestModelView.super.Ctor(self,"Prefabs/Test/TestModel.prefab",GameConst.Layer.ui)
end

function  TestModelView:OnInitialize( ... )
	-- body
	TestModelView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="测试 model View"
	self:AddClick()
end
function  TestModelView:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnA").gameObject,function( )
		if not self.testA then
			self.testA = require("Module.TestModel.AStar.AStarView").New()
		end
		self.testA:Show()
	end)

	AddBtnClick(self.transform:Find("model").gameObject,function( ... )
		self:Hide()
	end)
end



return TestModelView