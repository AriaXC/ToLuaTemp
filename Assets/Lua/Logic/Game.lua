require "3rd/pblua/login_pb"
require "3rd/pbc/protobuf"

local lpeg = require "lpeg"

local json = require "cjson"
local util = require "3rd/cjson/util"

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"
local print_r = require "3rd/sproto/print_r"


--管理器--
--这里控制lua的game


Game = {};
local this = Game;

local game; 
local transform;
local gameObject;
local WWW = UnityEngine.WWW;

--进入游戏
function  Game.StartUp( ... )
        

    --模块加载等的管理也在这里
    require("Aria.Core.initialize")

    --暂时没有场景切换 直接加载界面
    require("Module.Login.LoginView").New()

    --初始化资源等

    --动更等的逻辑再这里

end

--销毁--
function Game.OnDestroy()
	--logWarn('OnDestroy--->>>');
end
