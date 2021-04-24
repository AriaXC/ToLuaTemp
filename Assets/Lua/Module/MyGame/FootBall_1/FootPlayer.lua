 
local FootPlayer = class(FootPlayer)

function  FootPlayer:Ctor( )

	self.root = GameObject.Find("root")
	self.player = self.root.transform:Find("player").gameObject
	self.playerRig = GetComponentRigidbody(self.player)
	self.playerTra = self.player.transform.position
	self.playerLocalTra  =self.player.transform.localPosition

	self.playerTra = Vector3.New(1,1,1)
	--动态变化
	self.force = 0.7
	self:AddClick()
end
function  FootPlayer:AddClick()
	eventMgr:AddEventListener(UpdateEvent.UPDATE,self.Update,self)

end

function  FootPlayer:Update()
	if Input.GetMouseButtonUp(1) then
	-- 	-- local  worldPos = MainCamera:ViewportToWorldPoint(Input.mousePosition)
	-- 	local  worldPos  = Utils.ScreenToWorldPos(MainCamera,Input.mousePosition,self.playerTra) 
	-- 	local forward = worldPos- self.playerTra
	-- 	local  distance = Utils.Distance(worldPos,self.playerTra)
		
	-- 	log("playerTra==  "..tostring(self.playerTra ))
	-- 	log("==  "..tostring(worldPos))

	-- 	-- log(distance)
	-- 	-- self.playerRig:AddForce(forward*self.force*distance)
			
	-- 	self.playerTra = worldPos
	-- 	log("playerTra########==  "..tostring(self.playerTra ))
	-- end	
end


function  FootPlayer:Rest( )
		
end


function  FootPlayer:GameEnd( )
	eventMgr:RemoveEventListener(UpdateEvent.UPDATE,self.Update,self)
end
return FootPlayer