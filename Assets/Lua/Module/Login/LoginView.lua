
local LoginView=class(LoginView,View)

function  LoginView:Ctor( )
	-- body
	LoginView.super.Ctor(self,"Prefabs/Login/LoginView.prefab",nil)

end

function  LoginView:OnInitialize( ... )
	-- body
	LoginView.super.OnInitialize(self)


	local obj= self.transform:Find("bg/titleText").gameObject
	GetComponentText(obj).text="场景1"

	AddBtnClick(self.transform:Find("bg/btnY").gameObject,function( ... )
		  MySceneMgr:ShowScene("Module.Login.Login2Scene","Login2","Scene.Login.Login2")
	end)
	AddBtnClick(self.transform:Find("bg/btnN").gameObject,function( ... )
			GetComponentText(obj).text="11111"
	end)
	
end
return LoginView