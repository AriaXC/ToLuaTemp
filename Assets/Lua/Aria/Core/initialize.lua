
local  require = require

--扩展库也在这里添加
require "Tool/String"
require "Tool/Json2"

BaseBehaviour = require("Aria.Views.BaseBehaviour")
View=require("Aria.Views.View")
Scene = require("Aria.Views.Scene")
Avatar =require("Aria.Views.Avatar")


Handler = require("Aria.Utils.Handler")
EventStr = require("Aria.Event.Events")
Utils = require("Aria.Utils.Utils") 
Timer = require("Aria.Utils.Timer")
TimeUtil = require("Aria.Utils.TimeUtil")
Countdown = require("Aria.Utils.Countdown")
PrefabPool = require("Aria.Utils.PrefabPool")

-------------------------------------------------

UpdateEvent = require("Aria.Event.UpdateEvent")
-- DragEvent 


-------------------------------------------------

MainCamera = UnityEngine.Camera.main
Physics = UnityEngine.Physics
Physics2D = UnityEngine.Physics2D
Input = UnityEngine.Input
KeyCode =UnityEngine.KeyCode
Time = UnityEngine.Time
Application = UnityEngine.Application

------------------------------------------------

DOTween = DG.Tweening.DOTween
DOTween_Enum ={
	AutoPlay = DG.Tweening.AutoPlay,
	AxisConstraint = DG.Tweening.AxisConstraint,
	Color2 = DG.Tweening.Color2,
	Ease = DG.Tweening.Ease,
	LogBehaviour = DG.Tweening.LogBehaviour,
	LoopType = DG.Tweening.LoopType,
	PathMode = DG.Tweening.PathMode,
	PathType = DG.Tweening.PathType,
	RotateMode = DG.Tweening.RotateMode,
	ScrambleMode = DG.Tweening.ScrambleMode,
	TweenType = DG.Tweening.TweenType,
	UpdateType = DG.Tweening.UpdateType,
}

--------------------------------------------------

if Application.isEditor then
	require("Aria.Utils.requireEx")
end





