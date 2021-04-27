local TriggerScene=class(TriggerScene,Scene)


function  TriggerScene:Ctor( )

	TriggerScene.super.Ctor(self)

	if not self.myView then
		self.myView = require("Module.Test.TriggerTest").New()
	end
	
end

return TriggerScene