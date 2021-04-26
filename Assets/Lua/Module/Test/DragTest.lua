--拖拽测试
local DragTest = class(DragTest,View)

function  DragTest:Ctor()
	DragTest.super.Ctor(self,"Prefabs/Test/TestDrag.prefab",GameConst.Layer.window)

end
function  DragTest:OnInitialize( ... )
	-- body
	DragTest.super.OnInitialize(self)
	self.move = self.transform:Find("bg/move").gameObject
	self.moveRect = GetComponentRect(self.move)
	self:AddClick()
end

function  DragTest:AddClick()

	AddBtnClick(self.transform:Find("bg/btnDrag").gameObject,function( )
		eventMgr:AddEventListener(DragEvent.BEGIN_DRAG,self.StartDrag,self,self.move)
		eventMgr:AddEventListener(DragEvent.DRAG,self.DragHand,self,self.move)
		eventMgr:AddEventListener(DragEvent.END_DRAG,self.DragEnd,self,self.move)
	end)

	AddBtnClick(self.transform:Find("bg/btnDragDel").gameObject,function( )
		eventMgr:RemoveObjAllEventListener(self)		
	end)

end

function  DragTest:StartDrag(eventData)
	log("开始拖拽了   ")
	-- logError(tostring(eventData.delta))
	-- logErrorTable(eventData.data)

end
function  DragTest:DragHand(eventData)
	
	--屏幕坐标 转换为uiCanvas坐标  MySceneMgr._uiCanvas.worldCamera

	-- logError(" ==  "..tostring(RectTransformUtility))
	local  suce,pos = RectTransformUtility.ScreenPointToLocalPointInRectangle(
		GetComponentRect(self.moveRect.transform.parent.gameObject),eventData.position,
		nil,{})

	self.move.transform.localPosition= pos
end

function  DragTest:DragEnd(eventData)
	-- body
	log("拖拽结束了   ")
end

return DragTest