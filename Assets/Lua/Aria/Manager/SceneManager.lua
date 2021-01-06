
local  SceneManager = class("SceneManager")

local  stage = sceneMgr

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
	local  sceneClass = string.format("Module.%d.%dScene",scene,scene)
	local  sceneUnity = string.format("Scene.%d.%d.unity",scene,scene)
	--别的管理器 清空


	self.isChangeScene =true
	logError(sceneClass)
	if sceneClass then
		self.currLoadingSceneName = scene

		--上一个场景的 ab资源 依赖等 卸载
		resMgr:LoadSceneAsync(sceneUnity,function ()
			logError("load")
				
		end)
	end

end

return SceneManager