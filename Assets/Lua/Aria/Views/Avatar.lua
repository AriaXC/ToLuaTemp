
local Avatar=class("Avatar",View)

--Avatar的基类
function Avatar:Ctor(prefab,parent)
	Avatar.super.Ctor(self,prefab,parent)

end

function  Avatar:OnInitialize( ... )
	Avatar.super.OnInitialize(self)

	self:InitAnimator()
end

function  Avatar:InitAnimator( ... )
	--实例化 Animator

end
--改变controller
function  Avatar:SetAnimator( ... )
	-- body

end

function  Avatar:Play(AniName)
	
end

return Avatar