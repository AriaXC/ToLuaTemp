 
local FootPlayer = class(FootPlayer)

function  FootPlayer:Ctor( )

	self.root = GameObject.Find("root")
	self.player = self.root.transform:Find("player").gameObject
	self.playerRig = GetComponentRigidbody(self.player)
	self.playerTra = self.player.transform.position
	self.playerLocalTra  =self.player.transform.localPosition

	--动态变化
	self.force = 0.7
	self:AddClick()
end
function  FootPlayer:AddClick()
	eventMgr:AddEventListener(EventStr.UPDATE,self.Update,self)

end

function  FootPlayer:Update()
	if Input.GetMouseButtonUp(0) then
		local  worldPos = MainCamera:ViewportToWorldPoint(Input.mousePosition) 
		local forward = worldPos- self.playerTra
		local  distance = Utils.Distance(worldPos,self.playerTra)
		

		log(distance)
		self.playerRig:AddForce(forward*self.force*distance)
			

	end	
end


function  FootPlayer:Rest( )
		
end


function  FootPlayer:GameEnd( )
	eventMgr:RemoveEventListener(EventStr.UPDATE,self.Update,self)
end
return FootPlayer