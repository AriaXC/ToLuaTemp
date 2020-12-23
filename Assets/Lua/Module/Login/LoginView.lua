
local LoginView=class(LoginView,View)

function  LoginView:Ctor( )
	-- body
	LoginView.super.Ctor(self,"Prefabs/Login/LoginView.prefab",nil)

end

function  LoginView:OnInitialize( ... )
	-- body
	LoginView.super.OnInitialize(self)


	local obj= self.transform:Find("bg/titleText").gameObject
	GetComponentText(obj).text="成功了"

	AddBtnClick(self.transform:Find("bg/btnY").gameObject,function( ... )
			-- GetComponentText(obj).text="YYYYY"
			Stage.Test()
	end)
	AddBtnClick(self.transform:Find("bg/btnN").gameObject,function( ... )
			GetComponentText(obj).text="NNNNNN"
	end)
end
return LoginView