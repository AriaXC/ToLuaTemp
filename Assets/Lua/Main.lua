--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 		
end

function  AriaMain()
	-- body
	print("开始了啊")
	require("Aria.Core.initialize")
	--不能再声明全局变量了

	--暂时没有场景切换 直接加载界面
    require("Aria.Login.LoginView").New()
    
    -----
end
--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	log("场景切换了")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end