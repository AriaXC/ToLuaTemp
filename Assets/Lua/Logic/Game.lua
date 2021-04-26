
--管理器--
--这里控制lua的game
local Game = class("Game");


function  Game:Ctor()
    
end

--进入游戏
function Game:StartUp( ... )
        
    --模块加载等的管理也在这里
    self:LoadGlobalModule()

    self:NotLoadGlobal()

    --初始化资源依赖等
    resMgr:InitRes()
    

    MySceneMgr:ShowScene("Module.Scene.Login.LoginScene","Login","Scene.Login.Login")

    -- require("Module.Login.LoginView").New()
end
function  Game:NotLoadGlobal()
    -- 不能再定义全局变量
    --  全局变量会一直占有这块内存 过多的话 消耗大
    --  全局变量任何地方都可以改动 不利于debug
    setmetatable(_G,{
        __newindex = function ( ... )
            error("不能创建全局变量了")
        end
    })
end
--加载全局module
function  Game:LoadGlobalModule()
    require("Aria.Core.initialize")
    require("Aria.Manager.Init")
    GameConst = require("Aria.Core.GameConst")
end


return Game
