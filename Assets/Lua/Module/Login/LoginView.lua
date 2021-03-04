
local LoginView=class(LoginView,View)

function  LoginView:Ctor( )
	-- body
	LoginView.super.Ctor(self,"Prefabs/Login/LoginView.prefab",GameConst.Layer.ui)

end

function  LoginView:OnInitialize( ... )
	-- body
	LoginView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="场景1 ————- ogin"
	GetComponentText(self.transform:Find("bg/btn2/Text").gameObject).text = "切换到场景2"

	self:AddClick()

end

function  LoginView:AddClick()
	-- body
	AddBtnClick(self.transform:Find("bg/btn2").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.Login.Login2Scene","Login2","Scene.Login.Login2")
	end)

	AddBtnClick(self.transform:Find("bg/btn1").gameObject,function( ... )
		self:Hide()
		if not self.avatarView then
			self.avatarView = require("Module.AvatarTest.View.AvatarView").New()
		end
		self.avatarView:Show()
	end)

end


return LoginView