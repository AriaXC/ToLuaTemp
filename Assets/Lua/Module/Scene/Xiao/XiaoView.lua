local XiaoView=class(XiaoView,View)

function  XiaoView:Ctor( )
	-- body
	XiaoView.super.Ctor(self,"Prefabs/Xiao/XiaoView.prefab",GameConst.Layer.ui)

end

function  XiaoView:OnInitialize( ... )
	-- body
	XiaoView.super.OnInitialize(self)


end
return XiaoView