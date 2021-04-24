local  UpdateEvent = {}

local  floor = math.floor


--物理帧 更新 固定时间
UpdateEvent.FIXED_UPDATE = "Events_FixedUpdate"
--帧更新
UpdateEvent.UPDATE = "Events_Update"
--渲染帧更新之前
UpdateEvent.LATE_UPDATE = "Events_LateUpdate"


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