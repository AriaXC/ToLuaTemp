
--这样写 可以转换为局部变量 是存放在栈上的  可以提高速度  我的理解是
local _typeof=typeof 
local traceback = debug.traceback
local _isNull  = tolua.isnull
local _insert  = table.insert
local _remove = table.remove
local _sort = table.sort

--输出日志--
function log(str,trace)
    Util.Log(tostring(str),trace);
end
function logTable( str ,trace)
	log(Json2.encode(str),trace)
end

---这里还没有记录lua堆栈信息
--错误日志
function logError(str) 
	Util.LogError(tostring(str),traceback("",2));
end
function logErrorTable(str)
	logError(Json2.encode(str))
end

--警告日志--
function logWarn(str) 
	Util.LogWarning(tostring(str),debug.traceback("",2));
end

--查找对象--
function find(str)
	return GameObject.Find(str);
end

function destroy(obj)
	GameObject.Destroy(obj);
end

function newObject(prefab)
	return GameObject.Instantiate(prefab);
end


function child(str)
	return transform:FindChild(str);
end

function subGet(childNode, typeName)		
	return child(childNode):GetComponent(typeName);
end

function findPanel(str) 
	local obj = find(str);
	if obj == nil then
		error(str.." is null");
		return nil;
	end
	return obj:GetComponent("BaseLua");
end

function  isNull(obj)
	if obj == nil then
		return true
	end
	return _isNull(obj)
end
--------------------------------------------------------------------------------



--lua  class  实现lua的class
function class(className,superClass)
	-- body
	local cls ={}
	cls._classname=className
	cls._class=cls
	cls.__index=cls

	if superClass ~= nil then
		-- 子类的原表是父类  实现继承
		--所以子类查不到的方法 会去父类查 
		setmetatable(cls,superClass)
		cls.super=superClass
	else
		cls.Ctor=function( ... )
			-- body
		end
	end

	function cls.New( ... )
		-- body
		local ins=setmetatable({},cls)
		ins:Ctor(...)
		return ins
	end

	return cls
end
-- 加载预设
function Instantiate(prefab,parent,callback)
	-- body
	if prefab == nil then
		logError("你的prefab为空")
	end

	-- 如果传入的是string  就去加载这个prefab
	if type(prefab) == "string" then
		-- local  prefabPath = prefab
		-- local  abName = string.replace(prefab,"/","@")
		-- --可以直接把.prefab 给取消掉
		-- local  abName = string.sub(abName,1,string.len(prefab)-7)
		prefab = resMgr:LoadPrefab(prefab,callback)
		if prefab == nil then
			logError("你的prefab为空_1")
		end
	end

	local go=GameObject.Instantiate(prefab)
	
	if parent then
		--还需要加入图层
		-- go.transform:SetParent(parent,false)
		LuaHelper.SetParent(go.transform,parent,false)
	end

	log("AssetName == "..go.name)

	return go
end
--添加按钮点击事件
--audio  是否播放音乐 暂时没
function AddBtnClick(go,callback,audio)
	if go == nil then
		logError("你的按钮是个空")
		return
	end
	if callback == nil then
		logError("你的回调是空的")
		return
	end
	return LuaHelper.AddClick(go,function( ... )
		-- body
		log("点击的按钮  == "..go.name)
		callback()
	end)
end

function  Destroy( obj,delay)
	if obj then
		if delay then
			GameObject.Destroy(obj,delay)
		else
			GameObject.Destroy(obj)
		end
	end
end


-----------------------------------------------------------------------------------------------


function  GetComponentText(go)
	return go:GetComponent(_typeof(UnityEngine.UI.Text))
end

function  GetComponentImage(go)
	return go:GetComponent(_typeof(UnityEngine.UI.Image))
end

function  GetComponentRect(go)
	return go:GetComponent(_typeof(UnityEngine.RectTransform))
end

function  GetComponentAnimator(go)
	return go:GetComponent(_typeof(UnityEngine.Animator))
end

------------------------------------------------------------------------------------------------



function  handler(callback,caller,once)
	local  once = once or false
	return Handler.GetItem(callback,caller,once)
end


local  _dc_List = {}
local  _dc_AddList = {}
local  _dc_RemoveList = {}

local function UpdateCall()
	local  num =  #_dc_List

	--要加入的 列表
	for i=#_dc_AddList,1，-1 do
		num= num+1
		_dc_List[num] = _dc_AddList[i]
		_remove(_dc_AddList,i)
	end

	if num == 0 then
		log("么有计时器")
		eventMgr:RemoveEventListener(EventStr.UPDATE,handler(UpdateCall))
	end

	--根据时间判断 是否可以了

end

--计时器
--时间 帧数
function  DelayCall( delay,callback,caller,... )

	if #_dc_List == 0 then
		eventMgr:AddEventListener(EventStr.UPDATE,handler(UpdateCall))
	end 

	local  hand = handler(callback,caller,true)
	hand.args = {...}
	hand.delayedTime = delay
	hand.delayStartTime =  TimeUtil.time

	_dc_AddList[#_dc_AddList] = hand
	return hand
end

--取消回调
function  CancelDelayCall(hand)
	-- body
	
end







