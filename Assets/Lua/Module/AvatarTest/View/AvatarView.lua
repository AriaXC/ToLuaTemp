
--Avatar测试界面
local AvatarView = class(AvatarView,View)

function  AvatarView:Ctor( )
	-- body
	AvatarView.super.Ctor(self,"Prefabs/Avatar/AvatarView.prefab",nil)

end


return AvatarView