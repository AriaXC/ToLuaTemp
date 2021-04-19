local  Utils = {}



--只读表
function  Utils.ReadOnly( tab )
	local  lock = {}
	setmetaTable(lock,{
		__index = tab,
		__newindex = function( ... )
			logError("这个是个只读表啊")
		end
	})
	return lock
end

function  Utils.DOTweenFloat(startValue,endValue,duration,setFun,callback,ease )
	local  value = startValue
	local  function  getValue( )
		return value
	end 
	local  function setValue(Fvalue)
		setFun(Fvalue)
		value = Fvalue
	end 
	local  getter = DG.Tweening.Core.DOGetter_float (getValue)
	local  setter = DG.Tweening.Core.DOSetter_float (setValue)
	
	if not ease then 
		ease = DOTween_Enum.Ease.Linear
	end
	return DOTween.To(getter,setter,endValue,duration):SetEase(ease):OnComplete(function( ... )
		if callback then
			callback(value)
		end
	end)

end

--为文本插入图片
function  SimpleTextPic(text,str)
	-- body


end


--数字跳动   anim(是否停止已经存在的这个跳动，重新开始) 
function  Utils.NumUpDown(objText,oldScore,newScore,setFun,callback,speed,anim)
	speed = speed or 3
	local  updateTime = 0   -- 累计时间
	local  initUpdateScore  = 0    --初始值
	local  updateScore = initUpdateScore  -- 变化中的值
	local  scoreSequence = nil

	if anim then
		coroutine.stop(anim)
	end 
	anim = coroutine.start(function()
		
		if scoreSequence then
			scoreSequence:Kill(false)
			scoreSequence = nil
		end 
		if isNull(objText) then
			logError("NumUpDown 的 objText 是空")
			return	
		end
		if not oldScore or not newScore then
			logError("NumUpDown 数字是空")
			return 
		end 
		oldScore = tonumber(oldScore)
		newScore = tonumber(newScore)

		while(not isNull(objText)) do
			if	updateTime >1 or Mathf.Abs(updateTime - 1) <0.05 then
				--结束
				oldScore = newScore
				if setFun then
					objText.text = setFun(newScore)
				else
					objText.text =newScore
				end

				updateTime  = 0 

				if callback then
					callback()
					callback = nil
				end
				coroutine.stop(anim)
				return
			end

			--中间值计算
			updateScore  = Mathf.Lerp(oldScore,newScore,updateTime)
			if setFun then
				objText.text = Mathf.Round(setFun(updateScore))
			else
				objText.text = Mathf.Round(updateScore)
			end

			updateTime = updateTime + speed * Time.deltaTime
			coroutine.step(1)
		end

	end)
	return anim
end

return Utils