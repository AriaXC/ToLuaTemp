
local  SceneManager = class("SceneManager")

local  sceneMgr = sceneMgr
local  stringFormat = string.format

-- ui节点
local  uiPath = "Prefabs/Core/UICanvas.prefab"

function  SceneManager:Ctor ( )

	log("初始化 SceneManager")
	--场景管理器 初始化
	self.currScene = nil
	self.currSceneId = nil
	self.currSceneName = nil

	self.currLoadingScene = nil
	self.currLoadingSceneName = nil

	self.isChangeScene = false

	self._currCanvas = nil
	self._layer ={}
end

function  SceneManager:ShowScene(sceneClass,scene,sceneUnity)

	sceneUnity = string.replace(sceneUnity,"%p.","/",1)

	sceneUnity = sceneUnity..".unity"
	--别的管理器引用 清空

	self.isChangeScene =true
	if sceneClass then
		self.currLoadingSceneName = scene
		log("resMgr:LoadSceneAsync:Start  "..scene)
		--场景切换进度条 和资源卸载
		-- 可以先跳转到一个中间场景去做  未

		resMgr:LoadSceneAsync(sceneUnity,function (value)
			log("sceneMgr:LoadSceneAsync:Start  "..scene)
			if value then
				log("ab异步加载的进度条值   "..value)
				if value >0.95 then
					self:LoadScene(scene,sceneClass)
				end
			else
				self:LoadScene(scene,sceneClass)
			end
		end)
	end
end

-- 场景的加载条 可以在这个地方做 value
function  SceneManager:LoadScene(scene,sceneClass)
	resMgr:ClearAll()

	sceneMgr:LoadSceneAsync(scene,function(value)
		if value then
			log("场景异步加载的进度条值   "..value)
			if value < 0.9 then
			elseif value >0.9 and value <0.95 then
			elseif value >0.95 and value <=1 then
				log("sceneMgr:LoadSceneAsync:End  "..scene)
				
				self:AddCanvas()
				require(sceneClass).New()
			end
		else
			logError("场景切换收不到值了")
		end
	end)
end	


function SceneManager:AddCanvas( )
		--删除uicanvas
	if self._currCanvas then
		GameObject.Destroy(self._currCanvas)
		self._currCanvas =nil
		self._layer  = {}
	end

	local obj=Instantiate(uiPath)
	obj:DontDestroyOnLoad()
	obj.name = "UICanvas"

	self._currCanvas =obj
	self._layer [GameConst.Layer.canvas] = obj
	self._layer[GameConst.Layer.ui] = obj.transform:Find("ui").transform
	self._layer[GameConst.Layer.window] = obj.transform:Find("window").transform

end

return SceneManager