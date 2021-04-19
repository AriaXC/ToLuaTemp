
local Login2View=class(Login2View,View)

function  Login2View:Ctor( )
	-- body
	Login2View.super.Ctor(self,"Prefabs/Login/LoginView.prefab",GameConst.Layer.ui)

end

function  Login2View:OnInitialize( ... )
	-- body
	Login2View.super.OnInitialize(self)

	local obj= self.transform:Find("bg/titleText").gameObject
	GetComponentText(obj).text="场景2 ---  Login"
	GetComponentText(self.transform:Find("bg/btnScene2/Text").gameObject).text = "回到场景1"

	AddBtnClick(self.transform:Find("bg/btnScene2").gameObject,function( ... )
		  MySceneMgr:ShowScene("Module.Scene.Login.LoginScene","Login","Scene.Login.Login")
	end)

end
return Login2View