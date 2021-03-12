
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
	-- self:AddBehaviourScript(self.gameObject)
end

function  LoginView:AddClick()
	-- body
	AddBtnClick(self.transform:Find("bg/btn2").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.Login.Login2Scene","Login2","Scene.Login.Login2")
	end)

	AddBtnClick(self.transform:Find("bg/btn1").gameObject,function( ... )
		-- self:Hide()
		self:Destroy()
		if not self.avatarView then
			self.avatarView = require("Module.AvatarTest.View.AvatarView").New()
		end
		self.avatarView:Show()
	end)

	eventMgr:AddEventListener(EventStr.Test1,handler(self.EventTest1,self),self)

	AddBtnClick(self.transform:Find("bg/addEventBtn").gameObject,function( ... )
		self.handlerMy = eventMgr:AddEventListener(EventStr.Test2,handler(self.EventTest2,self),self)
	end)

	AddBtnClick(self.transform:Find("bg/delEventBtn").gameObject,function( ... )
		-- eventMgr:RemoveEventListener(EventStr.Test2,self.handlerMy)
		eventMgr:RemoveObjAllEventListener(self)
	end)
end

function  LoginView:Update( ... )
	-- body
	logError(self)
	logError(self.gameObject.name)
end
function  LoginView:EventTest1( event )

	log("收到了啊")
	log(event)
end
function  LoginView:EventTest2( event )

	log("收到了啊2")
	log(event)
end


return LoginView