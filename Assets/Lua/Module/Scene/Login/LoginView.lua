
local LoginView=class(LoginView,View)

function  LoginView:Ctor( )
	-- body
	LoginView.super.Ctor(self,"Prefabs/Login/LoginView.prefab",GameConst.Layer.ui)
end

function  LoginView:OnInitialize( ... )
	-- body
	LoginView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="场景1 ————- Login"

	self:AddClick()
end

function  LoginView:AddClick()
	-- body
	AddBtnClick(self.transform:Find("bg/btnScene2").gameObject,function( ... )
		-- MySceneMgr:ShowScene("Module.Login.Login2Scene","Login2","Scene.Login.Login2")
		
		MySceneMgr:ShowScene("Module.Scene.Login.Login2Scene","Login2","Scene.Login.Login2")
	end)

	AddBtnClick(self.transform:Find("bg/btnAvatar").gameObject,function( ... )
		-- self:Hide()
		-- self:Destroy()
		if not self.avatarView then
			self.avatarView = require("Module.AvatarTest.View.AvatarView").New()
		end
		self.avatarView:Show()
	end)


	AddBtnClick(self.transform:Find("bg/btnCe").gameObject,function( ... )
		-- self:Hide()
		if not self.testView then
			self.testView = require("Module.Test.TestView").New()
		end
		self.testView:Show()
	end)

	AddBtnClick(self.transform:Find("bg/btnCe_lua").gameObject,function( ... )
		-- self:Hide()
		if not self.testLuaView then
			self.testLuaView = require("Module.Test.TestLua").New()
		end
		self.testLuaView:Show()
	end)
	
	AddBtnClick(self.transform:Find("bg/btnCe_Model").gameObject,function( ... )
		if not self.testModel then
			self.testModel = require("Module.Test.TestModelView").New()
		end
		self.testModel:Show()
	end)
	AddBtnClick(self.transform:Find("bg/btnGame").gameObject,function( ... )
		if not self.testBtnGame then
			self.testBtnGame = require("Module.MyGame.MyGameView").New()
		end
		self.testBtnGame:Show()
	end)
end


return LoginView