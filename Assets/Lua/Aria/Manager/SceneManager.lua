
local  SceneManager = class("SceneManager")

local  sceneMgr = sceneMgr

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
	local  sceneClass = string.format("Module.%s.%sScene",scene,scene)
	local  sceneUnity = string.format("Scene.%s.%s.unity",scene,scene)
	--别的管理器引用 清空

	self.isChangeScene =true

	if sceneClass then
		self.currLoadingSceneName = scene

		--上一个场景的 ab资源 依赖等 卸载
		resMgr:LoadSceneAsync(sceneUnity,function ()
			sceneMgr:LoadSceneAsync(scene,function()
				self:AddCanvas()

				require("Module.Login.LoginScene").New()
			end)
		end)

	end

end
function SceneManager:AddCanvas( )
	local obj=Instantiate(uiPath)
	obj.name = "UICanvas"

	_layer[GameConst.Layer.canvas] = obj
	_layer[GameConst.Layer.ui] = obj.transform:Find("ui")
	_layer[GameConst.Layer.window] = obj.transform:Find("window")

end

return SceneManager