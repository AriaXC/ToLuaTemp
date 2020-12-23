
require "Common/define"
require "Common/functions"
require "Common/protocal"

--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 		
end


--第二次加入lua路径  是动更目录 动更的文件无法加入游戏包体
function  UpdateSearchPath( ... )
	
end

function  AriaMain()

	print("Lua开始了")
	app  = require("Logic.Game").New()
	-- 不能再定义全局变量
	--  全局变量会一直占有这块内存 过多的话 消耗大
	--  全局变量任何地方都可以改动 不利于debug
	setmetatable(_G,{
		__newindex = function ( ... )
			error("不能创建全局变量了")
		end
	})

	--test

	--更新搜索路径  为动更准备
	UpdateSearchPath()

	--游戏开始
	app:StartUp()

end
--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	logError("场景切换了")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()

end