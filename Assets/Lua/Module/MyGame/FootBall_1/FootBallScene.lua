local FootBallScene=class(FootBallScene,Scene)


function  FootBallScene:Ctor( )

	FootBallScene.super.Ctor(self)

	if not self.ballView then
		self.ballView = require("Module.MyGame.FootBall_1.FootBallView").New()
	end
	
end

return FootBallScene