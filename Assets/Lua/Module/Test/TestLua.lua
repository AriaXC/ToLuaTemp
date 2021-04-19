local TestLua=class("TestLua",View)


function  TestLua:Ctor( )
	TestLua.super.Ctor(self,"Prefabs/Test/TestView_Lua.prefab",GameConst.Layer.window)
end

function  TestLua:OnInitialize( ... )
	-- body
	TestLua.super.OnInitialize(self)


	self.titleText= GetComponentText(self.transform:Find("bg/titleText").gameObject)
	self.titleText.text="测试 Lua View"

	self.numTest = 1111
	self:AddClick()

end
function  TestLua:AddClick( ... )

	AddBtnClick(self.transform:Find("bg/btnP").gameObject,function( )
		local  arr = {1,2,3}
		-- self:FArray(arr,1)
		self:SubSet(arr,1)
	end)

	AddBtnClick(self.transform:Find("model").gameObject,function( ... )
		self:Hide()
	end)
	AddBtnClick(self.transform:Find("bg/btnXiao").gameObject,function( )
		MySceneMgr:ShowScene("Module.Scene.Xiao.XiaoScene","Xiao","Scene.Xiao.XiaoScene")
	end)
end

function  TestLua:PrintArr(arr,n )
	local  str = ""
	for i=1,n do
		str= str..arr[i]
	end
	log(str)
end
--所有子集
function  TestLua:SubSet(arr,n)
	if n <= #arr then
		self:PrintArr(arr,n)
		for  i=1,#arr do
			arr[i],arr[n] = arr[n],arr[i]
			self:SubSet(arr,n+1)
			arr[n],arr[i] = arr[i],arr[n]
		end
	end
end
--全排列
function TestLua:FArray(arr,n)
	if n == #arr then
		logTable(arr)
	else
		for  i=1,#arr do
			arr[i],arr[n] = arr[n],arr[i]
			self:FArray(arr,n+1)
			arr[n],arr[i] = arr[i],arr[n]
		end
	end
end



return TestLua