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
	AddBtnClick(self.transform:Find("bg/back").gameObject,function( ... )
		self:Hide()
	end)

	AddBtnClick(self.transform:Find("bg/btnFootBall").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.MyGame.FootBall_1.FootBallScene","FootBall","Scene.MyGame.FootBall")
	end)

	AddBtnClick(self.transform:Find("bg/btnXiao").gameObject,function( )
		MySceneMgr:ShowScene("Module.MyGame.Xiao.XiaoScene","Xiao","Scene.MyGame.XiaoScene")
	end)

end

return MyGameView