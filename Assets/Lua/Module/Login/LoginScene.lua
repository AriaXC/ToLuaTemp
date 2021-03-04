

--登录场景

local LoginScene=class(LoginScene,Scene)

--需要读配置表对应版本来判断
local isUpdate = false

function  LoginScene:Ctor( )

	LoginScene.super.Ctor(self)
	print("现在是登录场景了")

	if not self.loginView then
		self.loginView = require("Module.Login.LoginView").New()
	end
	-- self.loginView:Show()
	
	--如果需要热更  现在的热更是放在cs代码(我把lua打成了ab包 所以在一开始要先检测更新才能进游戏)
	if isUpdate then

	end
end

return LoginScene