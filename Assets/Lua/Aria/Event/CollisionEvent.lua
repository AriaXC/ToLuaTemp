
-- --相关碰撞器事件的派发
-- local  CollisionEvent = {}

-- --开始拖动
-- DragEvent.BEGIN_DRAG = "Events_DragDrop_BeginDrag"
-- --拖动中
-- DragEvent.DRAG = "Events_DragDrop_Drag"
-- --拖动结束
-- DragEvent.END_DRAG = "Events_DragDrop_EndDrag"
-- --拖动 按下还没有拖拽的时候
-- DragEvent.INITIALIZE_POTENTIAL_DRAG = "Events_DragDrop_InitializePotentialDrag"
-- --[[
-- //A、B对象必须均实现IDropHandler接口，且A至少实现IDragHandler接口
-- //当鼠标从A对象上开始拖拽，在B对象上抬起时 B对象响应此事件
-- //此时name获取到的是B对象的name属性
-- ]]
-- DragEvent.DROP = "Events_DragDrop_Drop"


-- function  DragEvent.DispatchEvent(ed,type,eventData)

-- 	eventMgr:DispatchEventSpecial(type,ed,eventData)
-- end

-- return CollisionEvent