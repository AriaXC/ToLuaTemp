
local Avatar=class("Avatar",View)

local Animator = require("Aria.Views.Animator")

--Avatar的基类
function Avatar:Ctor(prefab,parent)
	Avatar.super.Ctor(self,prefab,parent)

end

function  Avatar:OnInitialize( ... )
	Avatar.super.OnInitialize(self)

	self:InitAnimator()
end

function  Avatar:InitAnimator( )
	--实例化 Animator
	self.animator  = Animator.New(self.gameObject)
end
--改变controller
function  Avatar:SetAnimator(path)
	local  animators = self.gameObject:GetComponentsInChildren(typeof(UnityEngine.Animator))

	if animators.Length == 0 then
		logError("animators的长度是0 错误了")
		return
	end

	local  animator  = animators[0]
	if path then
		animator.runtimeAnimatorController = resMgr:LoadAnimatorController(path)
	end
	self.animator = Animator.New(self.gameObject)

end

function  Avatar:PlayAni(AniName,callback,changeCallback,crossTime)
	if self.animator then
		self.animator:PlayAni(AniName,callback,changeCallback,crossTime)
	end
end

return Avatar