--用来测试 场景切换ab包的卸载

local Login2Scene=class(Login2Scene,Scene)


function  Login2Scene:Ctor( )
	print("现在是场景2 测试")

	require("Module.Login.Login2View").New()
end

return Login2Scene