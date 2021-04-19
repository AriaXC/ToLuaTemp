

local XiaoScene=class(XiaoScene,Scene)


function  XiaoScene:Ctor( )

	XiaoScene.super.Ctor(self)

	if not self.xiaoView then
		self.xiaoView = require("Module.Scene.Xiao.XiaoView").New()
	end
	
end

return XiaoScene