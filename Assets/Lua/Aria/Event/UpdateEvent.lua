local  UpdateEvent = {}

local  floor = math.floor

function  UpdateEvent.DispatchEvent(type,time)
	
	TimeUtil.time = time
	TimeUtil.timeMsec  = floor(time*1000+0.5)

	TimeUtil.frameCount  = Time.frameCount
	TimeUtil.deltaTime = Time.deltaTime
	TimeUtil.totalDeltaTime = TimeUtil.totalDeltaTime + TimeUtil.deltaTime
	TimeUtil.timeSinceLevelLoad = Time.timeSinceLevelLoad

	eventMgr:DispatchEvent(type)
end

return UpdateEvent