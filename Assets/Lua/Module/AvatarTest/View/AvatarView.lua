
--Avatar测试界面
local AvatarView = class(AvatarView,View)

function  AvatarView:Ctor( )
	-- body
	AvatarView.super.Ctor(self,"Prefabs/Avatar/AvatarView.prefab",nil)

end
function AvatarView:OnInitialize( ... )
	AvatarView.super.OnInitialize(self)
	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="Avatar测试界面"

	self:AddClick()
	
end
function  AvatarView:AddClick(  )
	AddBtnClick(self.transform:Find("bg/btn1").gameObject,function( ... )
		if not self.player then
			self:AddAvatar()
		end

	end)
end
function  AvatarView:AddAvatar( )
	self.player = Avatar.New("Polyart/Prefab/GruntHP.prefab",self.transform)
	self.player.transform.localScale = Vector3.New(100,100,100)
	self.player.transform.localRotation = Vector3.New(0,-180,0)
	self.player.transform.localPosition = Vector3.New(0,0,-200)

	self.player:SetAnimator("Polyart/Animations/Aria.controller")
	self.player:PlayAni("run")
end

return AvatarView