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
	
end

return FootBallView