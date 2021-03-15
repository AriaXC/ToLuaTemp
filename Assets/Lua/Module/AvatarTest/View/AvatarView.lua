
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
	self:AddBehaviourScript(self.gameObject)
end
function  AvatarView:AddClick(  )
	AddBtnClick(self.transform:Find("bg/btnAdd").gameObject,function( ... )
		if not self.player then
			self:AddAvatar()
		end
	end)

	AddBtnClick(self.transform:Find("model").gameObject,function( ... )
		self:Hide()
	end)
end
function  AvatarView:AddAvatar( )
	self.player = Avatar.New("Polyart/Prefab/GruntHP.prefab",self.transform)
	self.player.transform.localScale = Vector3.New(100,100,100)
	self.player.transform.localRotation = Vector3.New(0,-180,0)
	self.player.transform.localPosition = Vector3.New(0,0,-200)

	self.player:SetAnimator("Polyart/Animations/Aria.controller")
	self.player:PlayAni("walk")
end
function  AvatarView:Update()
	if Input.GetKeyDown(KeyCode.W) then
		self.player:PlayAni("run")
	elseif Input.GetKeyDown(KeyCode.A) then
		self.player:PlayAni("walk")	
	end

end


return AvatarView