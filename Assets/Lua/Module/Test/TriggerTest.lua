 local TriggerTestView=class(TriggerTestView,View)

function  TriggerTestView:Ctor( )
	-- body
	TriggerTestView.super.Ctor(self,"Prefabs/Test/TestTrigger.prefab",GameConst.Layer.ui)
end

function  TriggerTestView:OnInitialize( ... )
	TriggerTestView.super.OnInitialize(self)

	self.bg=self.transform:Find("bg").gameObject
	self.root = GameObject.Find("root")

	self.move = self.root.transform:Find("move").gameObject
	self:AddClick()
end
function  TriggerTestView:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnTrigger").gameObject,function( ... )
		eventMgr:AddEventListener(TriggerEvent.ENTER,self.TriggerEnter,self,self.move)
		eventMgr:AddEventListener(TriggerEvent.STAY,self.TriggerStay,self,self.move)
		eventMgr:AddEventListener(TriggerEvent.EXIT,self.TriggerExit,self,self.move)
	end)

	AddBtnClick(self.transform:Find("bg/btnTriggerDel").gameObject,function( ... )
		eventMgr:RemoveObjAllEventListener(self)
	end)	
	AddBtnClick(self.transform:Find("bg/btnCollision").gameObject,function( ... )
		eventMgr:AddEventListener(CollisionEvent.ENTER,self.CollisionEnter,self,self.move)
		eventMgr:AddEventListener(CollisionEvent.STAY,self.CollisionStay,self,self.move)
		eventMgr:AddEventListener(CollisionEvent.EXIT,self.CollisionExit,self,self.move)
	end)

	AddBtnClick(self.transform:Find("bg/btnCollisionDel").gameObject,function( ... )
		eventMgr:RemoveObjAllEventListener(self)	
	end)

	AddBtnClick(self.transform:Find("btnShow").gameObject,function( ... )
		self.bg:SetActive(true)
	end)

	AddBtnClick(self.transform:Find("btnHide").gameObject,function( ... )
		self.bg:SetActive(false)
	end)
end

function TriggerTestView:TriggerEnter( eventData )
	-- body
	logError("TriggerEnter ==  "..eventData.gameObject.name)
end
function TriggerTestView:TriggerStay( eventData )
	-- body
	
	-- logError("TriggerStay ==  "..eventData.gameObject.name)
end
function TriggerTestView:TriggerExit( eventData )
	-- body
	logError("TriggerExit ==  "..eventData.gameObject.name)
end

function TriggerTestView:CollisionEnter( eventData )
	logError("CollisionEnter ==  "..eventData.gameObject.name.."    "..eventData.collider.gameObject.name)
end
function TriggerTestView:CollisionStay( eventData )
	-- logError("CollisionEnter ==  "..eventData.gameObject.name)	
end
function TriggerTestView:CollisionExit( eventData )
	logError("CollisionEnter ==  "..eventData.gameObject.name)
end
return TriggerTestView


