
local LoginView=class(LoginView,View)

function  LoginView:Ctor( )
	-- body
	LoginView.super.Ctor(self,"Prefabs/Login/LoginView.prefab",nil)

end

function  LoginView:OnInitialize( ... )
	-- body
	LoginView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="场景1"
	
	self:AddClick()

end

function  LoginView:AddClick()
	-- body
	AddBtnClick(self.transform:Find("bg/btnY").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.Login.Login2Scene","Login2","Scene.Login.Login2")
	end)
	AddBtnClick(self.transform:Find("bg/btnN").gameObject,function( ... )
		-- self.titleText.text="11111"
		local rect = GetComponentRect(self.transform:Find("bg/btnN").gameObject)
		-- logErrorTable(rect.anchoredPosition)
	end)
	AddBtnClick(self.transform:Find("bg/btn1").gameObject,function( ... )
		self.titleText.text="Avatar"
		self:Hide()
		if not self.avatarView then
			self.avatarView = require("Module.AvatarTest.View.AvatarView").New()
		end
		self.avatarView:Show()
	end)

end


return LoginView