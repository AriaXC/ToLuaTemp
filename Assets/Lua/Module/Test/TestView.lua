local TestView =class("TestView",View)


function  TestView:Ctor( )
	TestView.super.Ctor(self,"Prefabs/Test/TestView.prefab",GameConst.Layer.window)
end

function  TestView:OnInitialize( ... )
	-- body
	TestView.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="测试View"

	self.numTest = 1111
	self:AddClick()
	-- self:AddBehaviourScript(self.gameObject)

	-- self.titleText.transform:DOScale(1.3,2):OnComplete(function( ... )
	-- 	-- body
	-- 	log("Dotween 回调")
	-- end)
	-- self:InsertTest()
end
function  TestView:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnAddEvent").gameObject,function( ... )
		self.handlerMy = eventMgr:AddEventListener(EventStr.Test1,self.EventTest1,self,self)
	end)

	AddBtnClick(self.transform:Find("bg/btnDelEvent").gameObject,function( ... )
		eventMgr:RemoveEventListener(EventStr.Test1,self.EventTest1,self)
		-- eventMgr:RemoveObjAllEventListener(self)
	end)
	AddBtnClick(self.transform:Find("bg/btnSendEvent").gameObject,function( ... )
		eventMgr:DispatchEvent(EventStr.Test1,{my=1,aria=2})
	end)

	AddBtnClick(self.transform:Find("model").gameObject,function( ... )
		self:Hide()
	end)
	AddBtnClick(self.transform:Find("bg/btnNumChange").gameObject,function ( ... )

		local  objText = GetComponentText(self.transform:Find("bg/btnNumChange/num").gameObject)
		Utils.NumUpDown(objText,self.numTest,self.numTest+1234,nil,function()
			log("数字变化完了")
			self.numTest = self.numTest+1234
		end)
	end)
	AddBtnClick(self.transform:Find("bg/btnDo").gameObject,function( ... )
		Utils.DOTweenFloat(1,4,1,function(value )
			log(value)
		end,function( ... )
			log("输出结束")
		end)
	end)
	AddBtnClick(self.transform:Find("bg/btnUpdate").gameObject,function( ... )
		-- eventMgr:AddEventListener(UpdateEvent.FIXED_UPDATE,handler(self.MyFixedUpdate,self))
		eventMgr:AddEventListener(UpdateEvent.UPDATE,handler(self.MyUpdate,self))
		-- eventMgr:AddEventListener(UpdateEvent.LATE_UPDATE,handler(self.MyLateUpdate,self))
	end)

	AddBtnClick(self.transform:Find("bg/btnDelayCall").gameObject,function( ... )
		log("注册了一个2秒的定时器")
		self.del =  DelayCall(2,function( ... )
			log("我是延时2秒后触发的")
		end)
	end)
	AddBtnClick(self.transform:Find("bg/btnCancel").gameObject,function( ... )
		CancelDelayCall(self.del)
		log("清除计时器")
	end)
	AddBtnClick(self.transform:Find("bg/btnAddImage").gameObject,function( ... )
		log("文字加图片")
		Utils.SimpleTextPic(self.titleText,"Textures/A/a1.png",2)
	end)
	AddBtnClick(self.transform:Find("bg/btnDrag").gameObject,function( )	

		if not self.dragView then
			self.dragView = require("Module.Test.DragTest").New()
		end
		self.dragView:Show()
	end)
	AddBtnClick(self.transform:Find("bg/btnTrigger").gameObject,function( ... )
		MySceneMgr:ShowScene("Module.Test.TriggerScene","TriggerScene","Scene.Test.TriggerScene")
	end)

end

function  TestView:InsertTest()
	local  arr = {}

	for i=1,10 do
		table.insert(arr,i)
	end
	table.insert(arr,"SS")

	logTable(arr)
	log(arr[1])
end

function TestView:MyUpdate()
	log("MyUpdate  "..self.gameObject.name)
end
function TestView:MyFixedUpdate()
	log("MyFixedUpdate  "..self.gameObject.name)
end
function TestView:MyLateUpdate()
	log("MyLateUpdate   "..self.gameObject.name)
end
function  TestView:Update()
	if Input.GetMouseButtonUp(0) then
		local  ray  = Camera.main:ScreenPointToRay(Input.mousePosition)
		local  hitList = Physics.RaycastAll(ray,100)
		--2d 的没成功
		-- local  hitList = Physics2D.RaycastAll(Input.mousePosition,Vector2.up)
		-- logError(hitList.Length)
		if hitList.Length >0 then
			for i=0,hitList.Length-1 do
				logError(hitList[i].collider.gameObject.name)
			end
		end
	end		
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