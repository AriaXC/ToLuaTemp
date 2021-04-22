 local MyGameView=class(MyGameView,View)

function  MyGameView:Ctor( )
	-- body
	MyGameView.super.Ctor(self,"Prefabs/MyGame/MyGameView.prefab",GameConst.Layer.ui)

end

function  MyGameView:OnInitialize( ... )
	-- body
	MyGameView.super.OnInitialize(self)
	self:AddClick()
end
function  MyGameView:AddClick( ... )
	AddBtnClick(self.transform:Find("bg/btnFootBall").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.MyGame.FootBall_1.FootBallScene","FootBall","Scene.MyGame.FootBall")
	end)
end

return MyGameView