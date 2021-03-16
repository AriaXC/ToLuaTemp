
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




-------------------------------------------------

Camera = UnityEngine.Camera
Physics = UnityEngine.Physics
Physics2D = UnityEngine.Physics2D
Input = UnityEngine.Input
KeyCode =UnityEngine.KeyCode