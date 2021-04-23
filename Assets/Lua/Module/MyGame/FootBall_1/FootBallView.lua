 local FootBallView=class(FootBallView,View)

function  FootBallView:Ctor( )
	-- body
	FootBallView.super.Ctor(self,"Prefabs/MyGame/FootBall/FootView.prefab",GameConst.Layer.ui)
end

function  FootBallView:OnInitialize( ... )
	FootBallView.super.OnInitialize(self)
	self:AddClick()
end
function  FootBallView:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnStart").gameObject,function( ... )
		self:Hide()
		if not self.playControll then
			self.playControll = require("Module.MyGame.FootBall_1.FootPlayer").New()
		end
		self.playControll:Rest()
	end)
end

return FootBallView