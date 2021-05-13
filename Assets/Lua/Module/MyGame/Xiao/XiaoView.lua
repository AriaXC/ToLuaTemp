local XiaoView=class(XiaoView,View)

function  XiaoView:Ctor( )
	-- body
	XiaoView.super.Ctor(self,"Prefabs/MyGame/Xiao/XiaoView.prefab",GameConst.Layer.window)

end

function  XiaoView:OnInitialize( ... )
	-- body
	XiaoView.super.OnInitialize(self)


end
return XiaoView