
local Login2View=class(Login2View,View)

function  Login2View:Ctor( )
	-- body
	Login2View.super.Ctor(self,"Prefabs/Login/LoginView.prefab",nil)

end

function  Login2View:OnInitialize( ... )
	-- body
	Login2View.super.OnInitialize(self)

	local obj= self.transform:Find("bg/titleText").gameObject
	GetComponentText(obj).text="场景2"

	AddBtnClick(self.transform:Find("bg/btnY").gameObject,function( ... )
		  MySceneMgr:ShowScene("Module.Login.LoginScene","Login","Scene.Login.Login")
	end)
	AddBtnClick(self.transform:Find("bg/btnN").gameObject,function( ... )
			GetComponentText(obj).text="2222222"
	end)
	
end
return Login2View