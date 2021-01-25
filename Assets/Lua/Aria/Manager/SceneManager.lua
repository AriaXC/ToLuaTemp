
local  SceneManager = class("SceneManager")

local  sceneMgr = sceneMgr
local  stringFormat = string.format

-- ui节点
local  uiPath = "Prefabs/Core/UICanvas.prefab"
local  _layer ={}

function  SceneManager:Ctor ( )
	--场景管理器 初始化
	self.currScene = nil
	self.currSceneId = nil
	self.currSceneName = nil

	self.currLoadingScene = nil
	self.currLoadingSceneName = nil

	self.isChangeScene = false
end

function  SceneManager:ShowScene(scene)

	local  sceneClass = stringFormat("Module.%s.%sScene",scene,scene)
	local  sceneUnity = stringFormat("Scene/%s/%s.unity",scene,scene)
	--别的管理器引用 清空

	self.isChangeScene =true
	if sceneClass then
		self.currLoadingSceneName = scene
		log("resMgr:LoadSceneAsync:Start  "..scene)
		resMgr:LoadSceneAsync(sceneUnity,function (value)
			log("sceneMgr:LoadSceneAsync:Start  "..scene)
			logError("ab异步加载的进度条值   "..value)
			if value >0.95 then
				self:LoadScene(scene)
			end
		end)
	end
end


	
function  SceneManager:LoadScene(scene)
	sceneMgr:LoadSceneAsync(scene,function(value)
		if value then
			logError("场景异步加载的进度条值   "..value)
			if value < 0.9 then
			elseif value >0.9 and value <1 then
			elseif value >0.95 and value <=1 then
				log("sceneMgr:LoadSceneAsync:End  "..scene)
				self:AddCanvas()
				require(sceneClass).New()
			end
		end
	end)
end	


function SceneManager:AddCanvas( )
	local obj=Instantiate(uiPath)
	obj:DontDestroyOnLoad()
	obj.name = "UICanvas"

	_layer[GameConst.Layer.canvas] = obj
	_layer[GameConst.Layer.ui] = obj.transform:Find("ui")
	_layer[GameConst.Layer.window] = obj.transform:Find("window")

end

return SceneManager