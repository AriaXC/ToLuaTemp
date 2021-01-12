

--登录场景

local LoginScene=class(LoginScene,Scene)

--需要读配置表对应版本来判断
local isUpdate = false

function  LoginScene:Ctor( )
	print("现在是登录场景了")

	require("Module.Login.LoginView").New()

	--如果需要热更  现在的热更是放在cs代码
	if isUpdate then

	end
end

return LoginScene