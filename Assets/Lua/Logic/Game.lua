
--管理器--
--这里控制lua的game
Game = class("Game");

--需要读配置表对应版本来判断
local isUpdate = false

function  Game:Ctor()
    
end

--进入游戏
function Game:StartUp( ... )
        

    --模块加载等的管理也在这里
    self:LoadGlobalModule()

    --初始化资源依赖等
    resMgr.InitRes()

    --动更等的逻辑再这里
    if isUpdate then
        --跳转到动更界面

    else
        MySceneMgr:LoadScene("Login")
    end
    -- require("Module.Login.LoginView").New()
end

--加载全局module
function  Game:LoadGlobalModule()
    require("Aria.Core.initialize")
    require("Aria.Manager.Init")

end



--销毁--
function Game:OnDestroy()
	--logWarn('OnDestroy--->>>');
end

return Game
