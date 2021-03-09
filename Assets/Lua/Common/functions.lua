
--这样写 可以转换为局部变量 是存放在栈上的  可以提高速度  我的理解是
local _typeof=typeof 
local traceback = debug.traceback

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
		callback()
	end)
end




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













