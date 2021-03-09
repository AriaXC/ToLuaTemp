
local Animator=class("Animator")

--Animator的基类
function Animator:Ctor(avatar)

	self.avatar = avatar.gameObject
	self.gameObject = self.avatar
	self.transform = self.gameObject.transform
	--animator 列表
	self._animators={}
	-- 动画播放器
	self._clips ={}
	--上一个动作
	self._lastAniName=""

	self:InitAnimator()

end

function Animator:InitAnimator()
	-- body
	local animators = self.gameObject:GetComponentsInChildren(typeof(UnityEngine.Animator))

	for i=0,animators.Length-1 do
		local  animator = animators[i]
		table.insert(self._animators,animator)
	end

	self:SetSpeed(1)
end

--changeCallback  切换回调

function  Animator:PlayAni(AniName,callback,changeCallback,crossTime)
	if self.animator then
		self.animator:PlayAni(AniName)
	end

	if self._lastAniName and self._lastAniName ~= AniName and changeCallback then
		changeCallback()
	end

	--- 计时器  然后 动画播放完的回调 

	crossTime = crossTime or 0.1
	for k,v in pairs(self._animators) do
		
		-- CrossFadeInFixedTime  Play
		v:CrossFadeInFixedTime(AniName,crossTime)
	end

	self._lastAniName = AniName
end

--设置速度
function  Animator:SetSpeed( speed )
	speed = speed or 1
	self.speed =speed
	for k,v in pairs(self._animators) do
		v.speed = speed
	end
end
return Animator