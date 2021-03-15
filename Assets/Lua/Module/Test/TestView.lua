local TestView =class("TestView",View)


function  TestView:Ctor( )
	TestView.super.Ctor(self,"Prefabs/Test/TestView.prefab",GameConst.Layer.window)
end

function  TestView:OnInitialize( ... )
	-- body
	TestView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="测试View"

	self:AddClick()
	-- self:AddBehaviourScript(self.gameObject)
	self.titleText.transform:DOScale(1.3,2):OnComplete(function( ... )
		-- body
		log("Dotween 回调")
	end)
end
function  TestView:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnAddEvent").gameObject,function( ... )
		self.handlerMy = eventMgr:AddEventListener(EventStr.Test1,handler(self.EventTest1,self),self)
	end)

	AddBtnClick(self.transform:Find("bg/btnDelEvent").gameObject,function( ... )
		-- eventMgr:RemoveEventListener(EventStr.Test2,self.handlerMy,self)
		eventMgr:RemoveObjAllEventListener(self)
	end)
	AddBtnClick(self.transform:Find("bg/btnSendEvent").gameObject,function( ... )
		eventMgr:DispatchEvent(EventStr.Test1,{my=1,aria=2})
	end)

	AddBtnClick(self.transform:Find("model").gameObject,function( ... )
		self:Hide()
	end)
end

	
function  TestView:Update( ... )
	-- body
	logError(self)
	logError(self.gameObject.name)
end
function  TestView:EventTest1( event )

	log("收到了啊")
	logTable(event)
end
function  TestView:EventTest2( event )

	log("收到了啊2")
	logTable(event)
end

return TestView